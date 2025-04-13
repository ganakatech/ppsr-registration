using Xunit;
using Moq;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PpsrRegistration.Data;
using PpsrRegistration.Models;
using PpsrRegistration.Services;
using System.Text;

namespace PpsrRegistration.Tests;

public class CsvProcessorTests
{
    private readonly AppDbContext _dbContext;
    private readonly IValidator<VehicleRegistration> _validator;
    private readonly CsvProcessor _processor;

    public CsvProcessorTests()
    {
        // Set up in-memory database
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;
        _dbContext = new AppDbContext(options);

        // Set up validator
        _validator = new VehicleRegistrationValidator();

        // Create processor
        _processor = new CsvProcessor(_dbContext, _validator);
    }

    [Fact]
    public async Task ProcessAsync_ValidCsvData_ShouldSaveToDatabase()
    {
        // Arrange
        var csv = @"Grantor First Name,Grantor Middle Names,Grantor Last Name,VIN,Registration start date,Registration duration,SPG ACN,SPG Organization Name
John,,Doe,1HGCM82633A123456,2025-04-13,7,123456789,Test Org";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));

        // Act
        var result = await _processor.ProcessAsync(stream);

        // Assert
        Assert.Equal(1, result.Processed);
        Assert.Equal(0, result.Invalid);

        var savedRegistration = await _dbContext.Registrations.FirstOrDefaultAsync();
        Assert.NotNull(savedRegistration);
        Assert.Equal("John", savedRegistration.GrantorFirstName);
        Assert.Equal("Doe", savedRegistration.GrantorLastName);
        Assert.Equal("1HGCM82633A123456", savedRegistration.VIN);
    }

    [Fact]
    public async Task ProcessAsync_InvalidData_ShouldNotSaveAndReturnErrors()
    {
        // Arrange
        var csv = @"Grantor First Name,Grantor Middle Names,Grantor Last Name,VIN,Registration start date,Registration duration,SPG ACN,SPG Organization Name
,,,invalid_vin,2025-04-13,invalid,invalid_acn,"; // All invalid data
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));

        // Act
        var result = await _processor.ProcessAsync(stream);

        // Assert
        Assert.Equal(0, result.Processed);
        Assert.Equal(1, result.Invalid);
        Assert.Empty(await _dbContext.Registrations.ToListAsync());
    }

    [Fact]
    public async Task ProcessAsync_MixedValidAndInvalidData_ShouldSaveValidOnly()
    {
        // Arrange
        var csv = @"Grantor First Name,Grantor Middle Names,Grantor Last Name,VIN,Registration start date,Registration duration,SPG ACN,SPG Organization Name
John,,Doe,1HGCM82633A123456,2025-04-13,7,123456789,Test Org
,,,invalid_vin,2025-04-13,invalid,invalid_acn,"; // Second row invalid
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));

        // Act
        var result = await _processor.ProcessAsync(stream);

        // Assert
        Assert.Equal(1, result.Processed);
        Assert.Equal(1, result.Invalid);
        Assert.Single(await _dbContext.Registrations.ToListAsync());
    }

    [Fact]
    public async Task ProcessAsync_EmptyFile_ShouldReturnZeroCounts()
    {
        // Arrange
        var csv = "Grantor First Name,Grantor Middle Names,Grantor Last Name,VIN,Registration start date,Registration duration,SPG ACN,SPG Organization Name";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));

        // Act
        var result = await _processor.ProcessAsync(stream);

        // Assert
        Assert.Equal(0, result.Processed);
        Assert.Equal(0, result.Invalid);
        Assert.Empty(await _dbContext.Registrations.ToListAsync());
    }

    [Fact]
    public async Task ProcessAsync_DuplicateVIN_ShouldNotSaveAndReturnError()
    {
        // Arrange
        var csv = @"Grantor First Name,Grantor Middle Names,Grantor Last Name,VIN,Registration start date,Registration duration,SPG ACN,SPG Organization Name
John,,Doe,1HGCM82633A123456,2025-04-13,7,123456789,Test Org
Jane,,Smith,1HGCM82633A123456,2025-04-13,7,987654321,Other Org"; // Same VIN
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));

        // Act
        var result = await _processor.ProcessAsync(stream);

        // Assert
        Assert.Equal(2, result.Processed);
        Assert.Equal(0, result.Invalid);
        Assert.Equal(2, await _dbContext.Registrations.CountAsync());
    }
}