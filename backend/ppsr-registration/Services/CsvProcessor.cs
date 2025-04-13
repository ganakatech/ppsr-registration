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
            MissingFieldFound = null
        };
        using var csv = new CsvReader(reader, config);

        csv.Context.RegisterClassMap<VehicleRegistrationMap>();

        var records = csv.GetRecords<VehicleRegistration>().ToList();
        var summary = new BatchSummary { Submitted = records.Count };

        foreach (var record in records)
        {
            ValidationResult result = await _validator.ValidateAsync(record);
            if (!result.IsValid)
            {
                summary.Invalid++;
                continue;
            }

            var existing = await _db.Registrations.FirstOrDefaultAsync(r =>
                r.VIN == record.VIN &&
                r.GrantorFirstName == record.GrantorFirstName &&
                r.GrantorLastName == record.GrantorLastName &&
                r.SPG_ACN == record.SPG_ACN);

            if (existing != null)
            {
                existing.StartDate = record.StartDate;
                existing.Duration = record.Duration;
                existing.SPG_OrgName = record.SPG_OrgName;
                _db.Registrations.Update(existing);
                summary.Updated++;
            }
            else
            {
                await _db.Registrations.AddAsync(record);
                summary.Added++;
            }
        }

        summary.Processed = summary.Added + summary.Updated;
        await _db.SaveChangesAsync();
        return summary;
    }
}
