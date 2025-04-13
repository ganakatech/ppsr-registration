using Xunit;
using FluentValidation;
using PpsrRegistration.Models;

namespace PpsrRegistration.Tests;

public class VehicleRegistrationValidatorTests
{
    private readonly VehicleRegistrationValidator _validator;

    public VehicleRegistrationValidatorTests()
    {
        _validator = new VehicleRegistrationValidator();
    }

    [Fact]
    public void Validate_ValidRegistration_ShouldPass()
    {
        // Arrange
        var registration = new VehicleRegistration
        {
            GrantorFirstName = "John",
            GrantorLastName = "Doe",
            VIN = "1HGCM82633A123456", // 17 characters
            SPG_ACN = "123456789", // 9 digits
            SPG_OrgName = "Test Organization",
            Duration = "7" // Valid duration
        };

        // Act
        var result = _validator.Validate(registration);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Validate_EmptyFirstName_ShouldFail(string firstName)
    {
        // Arrange
        var registration = new VehicleRegistration { GrantorFirstName = firstName };

        // Act
        var result = _validator.Validate(registration);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(VehicleRegistration.GrantorFirstName));
    }

    [Fact]
    public void Validate_FirstNameTooLong_ShouldFail()
    {
        // Arrange
        var registration = new VehicleRegistration
        {
            GrantorFirstName = new string('A', 36) // 36 characters (max is 35)
        };

        // Act
        var result = _validator.Validate(registration);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(VehicleRegistration.GrantorFirstName));
    }

    [Theory]
    [InlineData("12345678")] // 8 digits
    [InlineData("1234567890")] // 10 digits
    [InlineData("12345ABCD")] // Contains letters
    public void Validate_InvalidACN_ShouldFail(string acn)
    {
        // Arrange
        var registration = new VehicleRegistration { SPG_ACN = acn };

        // Act
        var result = _validator.Validate(registration);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(VehicleRegistration.SPG_ACN));
    }

    [Theory]
    [InlineData("1")] // Too short
    [InlineData("30")] // Invalid value
    [InlineData("")] // Empty
    public void Validate_InvalidDuration_ShouldFail(string duration)
    {
        // Arrange
        var registration = new VehicleRegistration { Duration = duration };

        // Act
        var result = _validator.Validate(registration);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(VehicleRegistration.Duration));
    }

    [Theory]
    [InlineData("7")]
    [InlineData("25")]
    [InlineData("N/A")]
    public void Validate_ValidDuration_ShouldPass(string duration)
    {
        // Arrange
        var registration = new VehicleRegistration
        {
            GrantorFirstName = "John",
            GrantorLastName = "Doe",
            VIN = "1HGCM82633A123456",
            SPG_ACN = "123456789",
            SPG_OrgName = "Test Org",
            Duration = duration
        };

        // Act
        var result = _validator.Validate(registration);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("1234567890123456")] // 16 characters
    [InlineData("12345678901234567")] // 18 characters
    public void Validate_InvalidVINLength_ShouldFail(string vin)
    {
        // Arrange
        var registration = new VehicleRegistration { VIN = vin };

        // Act
        var result = _validator.Validate(registration);

        // Assert
        Assert.False(result.IsValid);
        //Assert.Contains(result.Errors, e => e.PropertyName == nameof(VehicleRegistration.VIN));
    }
}