using Xunit;
using CsvHelper;
using CsvHelper.Configuration;
using PpsrRegistration.Models;
using PpsrRegistration.Services;
using System.Globalization;
using System.Text;

namespace PpsrRegistration.Tests;

public class VehicleRegistrationMapTests
{
    [Fact]
    public void Map_AllFieldsProvided_ShouldMapCorrectly()
    {
        // Arrange
        var csv = @"Grantor First Name,Grantor Middle Names,Grantor Last Name,VIN,Registration start date,Registration duration,SPG ACN,SPG Organization Name
John,Middle,Doe,1HGCM82633A123456,2025-04-13,7,123456789,Test Org";
        using var reader = new StringReader(csv);
        using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
        csvReader.Context.RegisterClassMap<VehicleRegistrationMap>();

        // Act
        var records = csvReader.GetRecords<VehicleRegistration>().ToList();

        // Assert
        Assert.Single(records);
        var record = records[0];
        Assert.Equal("John", record.GrantorFirstName);
        Assert.Equal("Middle", record.GrantorMiddleNames);
        Assert.Equal("Doe", record.GrantorLastName);
        Assert.Equal("1HGCM82633A123456", record.VIN);
        Assert.Equal(new DateOnly(2025, 4, 13), record.StartDate);
        Assert.Equal("7", record.Duration);
        Assert.Equal("123456789", record.SPG_ACN);
        Assert.Equal("Test Org", record.SPG_OrgName);
    }

    [Fact]
    public void Map_OptionalFieldsEmpty_ShouldMapCorrectly()
    {
        // Arrange
        var csv = @"Grantor First Name,Grantor Middle Names,Grantor Last Name,VIN,Registration start date,Registration duration,SPG ACN,SPG Organization Name
John,,Doe,1HGCM82633A123456,2025-04-13,7,123456789,Test Org";
        using var reader = new StringReader(csv);
        using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
        csvReader.Context.RegisterClassMap<VehicleRegistrationMap>();

        // Act
        var records = csvReader.GetRecords<VehicleRegistration>().ToList();

        // Assert
        Assert.Single(records);
        var record = records[0];
        Assert.Equal("John", record.GrantorFirstName);
        //Assert.Null(record.GrantorMiddleNames);
        Assert.Equal("Doe", record.GrantorLastName);
    }

    [Fact]
    public void Map_HeadersInDifferentOrder_ShouldMapCorrectly()
    {
        // Arrange
        var csv = @"VIN,SPG Organization Name,Grantor First Name,Grantor Middle Names,Grantor Last Name,Registration start date,Registration duration,SPG ACN
1HGCM82633A123456,Test Org,John,Middle,Doe,2025-04-13,7,123456789";
        using var reader = new StringReader(csv);
        using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
        csvReader.Context.RegisterClassMap<VehicleRegistrationMap>();

        // Act
        var records = csvReader.GetRecords<VehicleRegistration>().ToList();

        // Assert
        Assert.Single(records);
        var record = records[0];
        Assert.Equal("John", record.GrantorFirstName);
        Assert.Equal("Middle", record.GrantorMiddleNames);
        Assert.Equal("Doe", record.GrantorLastName);
        Assert.Equal("1HGCM82633A123456", record.VIN);
    }

    [Fact]
    public void Map_InvalidDateFormat_ShouldThrowException()
    {
        // Arrange
        var csv = @"Grantor First Name,Grantor Middle Names,Grantor Last Name,VIN,Registration start date,Registration duration,SPG ACN,SPG Organization Name
John,Middle,Doe,1HGCM82633A123456,13-04-2025,7,123456789,Test Org"; // Invalid date format
        using var reader = new StringReader(csv);
        using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
        csvReader.Context.RegisterClassMap<VehicleRegistrationMap>();

        // Act & Assert
        Assert.Throws<CsvHelper.ReaderException>(() =>
        {
            csvReader.GetRecords<VehicleRegistration>().ToList();
        });
    }
}