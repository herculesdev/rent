using Rent.Backoffice.Core.Features.Motorbike.Create;

namespace Rent.Backoffice.Tests.Core.Features.Create;

public class CreateMotorbikeCommandValidatorTests
{
    private readonly CreateMotorbikeCommandValidator _validator = new();
    private readonly CreateMotorbikeCommand _motorbikeSample = new(2024, "Honda Titan 160", "FMK0B49");
    private const int FirstMotorcycleManufactureYear = 1885;

    [Fact]
    public void Validate_ShouldBeValid_WhenAllFieldsAreValid()
    {
        var motorbike = _motorbikeSample;
        var expectedErrorCount = 0;
        
        var result = _validator.Validate(motorbike);
        
        Assert.True(result.IsValid);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
    }
    
    [Theory]
    [InlineData(1884)]
    [InlineData(1883)]
    [InlineData(1700)]
    public void Validate_ShouldBeInvalid_WhenManufactureYearIsLessThanFirstMotorcycleManufactureYear(int manufactureYear)
    {
        var motorbike = _motorbikeSample with { ManufactureYear = manufactureYear };
        var expectedErrorMessage = $"Minimum Manufacture Year is {FirstMotorcycleManufactureYear}";
        var expectedErrorCount = 1;
        
        var result = _validator.Validate(motorbike);
        
        Assert.False(result.IsValid);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
        Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault()?.ErrorMessage);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Validate_ShouldBeInvalid_WhenModelNameIsEmpty(string modelName)
    {
        var motorbike = _motorbikeSample with { ModelName = modelName };
        var expectedErrorMessage = "Model is required";
        var expectedErrorCount = 1;
        
        var result = _validator.Validate(motorbike);
        
        Assert.False(result.IsValid);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
        Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault()?.ErrorMessage);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Validate_ShouldBeInvalid_WhenLicensePlateIsEmpty(string licensePlate)
    {
        var motorbike = _motorbikeSample with { LicensePlate = licensePlate };
        var expectedErrorMessage = "License Plate is required";
        var expectedErrorCount = 1;
        
        var result = _validator.Validate(motorbike);
        
        Assert.False(result.IsValid);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
        Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault()?.ErrorMessage);
    }
    
    [Theory]
    [InlineData("ABC0B4J")]
    [InlineData("4J0BABC")]
    [InlineData("123ABCD")]
    [InlineData("ABCDEFG")]
    [InlineData("0123456")]
    public void Validate_ShouldBeInvalid_WhenIsNotValidBrazilianLicensePlate(string licensePlate)
    {
        var motorbike = _motorbikeSample with { LicensePlate = licensePlate };
        var expectedErrorMessage = "License Plate should be in the following format: AAA0A00 or AAA0000";
        var expectedErrorCount = 1;
        
        var result = _validator.Validate(motorbike);
        
        Assert.False(result.IsValid);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
        Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault()?.ErrorMessage);
    }
}