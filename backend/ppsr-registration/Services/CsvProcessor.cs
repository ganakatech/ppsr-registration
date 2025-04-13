using CsvHelper;
using CsvHelper.Configuration;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using PpsrRegistration.Data;
using PpsrRegistration.Models;
using System.Globalization;

namespace PpsrRegistration.Services;

public class CsvProcessor
{
    private readonly AppDbContext _db;
    private readonly IValidator<VehicleRegistration> _validator;
    private const int BatchSize = 1000; // Process records in batches for better performance

    public CsvProcessor(AppDbContext db, IValidator<VehicleRegistration> validator)
    {
        _db = db;
        _validator = validator;
    }

    public async Task<BatchSummary> ProcessAsync(Stream csvStream)
    {
        using var reader = new StreamReader(csvStream);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null,
            BadDataFound = null
        };
        using var csv = new CsvReader(reader, config);

        csv.Context.RegisterClassMap<VehicleRegistrationMap>();

        var records = csv.GetRecords<VehicleRegistration>().ToList();
        var summary = new BatchSummary { Submitted = records.Count };

        // Create a lookup of existing registrations by grantor name for efficient querying
        // Since we assume no grantor shares the same full name, we can use it as a unique key
        var existingRegistrations = await _db.Registrations
            .ToDictionaryAsync(r => $"{r.GrantorFirstName} {r.GrantorLastName}");

        // Process records in batches
        for (int i = 0; i < records.Count; i += BatchSize)
        {
            var batch = records.Skip(i).Take(BatchSize);

            foreach (var record in batch)
            {
                ValidationResult result = await _validator.ValidateAsync(record);
                if (!result.IsValid)
                {
                    summary.Invalid++;
                    continue;
                }

                var grantorKey = $"{record.GrantorFirstName} {record.GrantorLastName}";

                if (existingRegistrations.TryGetValue(grantorKey, out var existing))
                {
                    // Update existing registration
                    existing.VIN = record.VIN;
                    existing.StartDate = record.StartDate;
                    existing.Duration = record.Duration;
                    existing.SPG_OrgName = record.SPG_OrgName;
                    existing.SPG_ACN = record.SPG_ACN;
                    _db.Registrations.Update(existing);
                    summary.Updated++;

                    // Update the cache
                    existingRegistrations[grantorKey] = existing;
                }
                else
                {
                    // Add new registration
                    await _db.Registrations.AddAsync(record);
                    summary.Added++;

                    // Update the cache
                    existingRegistrations.Add(grantorKey, record);
                }
            }

            // Save changes for the current batch
            await _db.SaveChangesAsync();
        }

        summary.Processed = summary.Added + summary.Updated;
        return summary;
    }
}
