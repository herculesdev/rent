using Rent.Renter.Core.Features.DeliveryPerson.Create;

namespace Rent.Renter.Tests.Core.Features.DeliveryPerson.Create;

public class CreateDeliveryPersonCommandValidatorTests
{
    private readonly CreateDeliveryPersonCommandValidator _validator = new();
    private readonly CreateDeliveryPersonCommand _createDeliveryPersonCommandSample = new("Hércules", "16599855000110", "42997067713", "AB", DateOnly.Parse("1997-03-22"), Consts.Base64BmpCnh);
    
    [Theory]
    [InlineData("    ")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Validate_ShouldBeInvalid_WhenNameIsEmpty(string name)
    {
        var sample = _createDeliveryPersonCommandSample with { Name = name };
        var expectedErrorCount = 1;
        var expectedErrorMessage = "Name is required";
        
        var result = _validator.Validate(sample);
        
        Assert.False(result.IsValid);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
        Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault()?.ErrorMessage);
    }
    
    [Theory]
    [InlineData("    ")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Validate_ShouldBeInvalid_WhenDocumentNumberIsEmpty(string documentNumber)
    {
        var sample = _createDeliveryPersonCommandSample with { DocumentNumber = documentNumber };
        var expectedErrorCount = 1;
        var expectedErrorMessage = "Document Number is required";
        
        var result = _validator.Validate(sample);
        
        Assert.False(result.IsValid);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
        Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault()?.ErrorMessage);
    }
    
    [Theory]
    [InlineData("73100534000")] // CPF
    [InlineData("841005340001234")] // more than 14 digits
    [InlineData("11111111111111")] // 14 digits, equal numbers
    [InlineData("00000000000000")] // 14 digits, equal numbers
    [InlineData("20317078000103")] // 14 digits, but invalid CNPJ
    [InlineData("30317078000104")] // 14 digits, but invalid CNPJ
    [InlineData("A031707B00010C")] // 14 digits, not allowed symbols
    public void Validate_ShouldBeInvalid_WhenDocumentNumberIsNotValidBrazilianCompanyDocument(string documentNumber)
    {
        var sample = _createDeliveryPersonCommandSample with { DocumentNumber = documentNumber };
        var expectedErrorCount = 1;
        var expectedErrorMessage = "Document Number isn't valid brazilian company document number";
        
        var result = _validator.Validate(sample);
        
        Assert.False(result.IsValid);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
        Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault()?.ErrorMessage);
    }
    
    [Theory]
    [InlineData("    ")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Validate_ShouldBeInvalid_WhenDriverLicenseNumberIsEmpty(string driverLicenseNumber)
    {
        var sample = _createDeliveryPersonCommandSample with { DriverLicenseNumber = driverLicenseNumber };
        var expectedErrorCount = 1;
        var expectedErrorMessage = "Driver License Number is required";
        
        var result = _validator.Validate(sample);
        
        Assert.False(result.IsValid);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
        Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault()?.ErrorMessage);
    }
    
    [Theory]
    [InlineData("    ")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Validate_ShouldBeInvalid_WhenDriverLicenseTypeIsEmpty(string driverLicenseType)
    {
        var sample = _createDeliveryPersonCommandSample with { DriverLicenseType = driverLicenseType };
        var expectedErrorCount = 1;
        var expectedErrorMessage = "Driver License Type is required";
        
        var result = _validator.Validate(sample);
        
        Assert.False(result.IsValid);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
        Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault()?.ErrorMessage);
    }
    
    [Theory]
    [InlineData("ABC")]
    [InlineData("AA")]
    [InlineData("BB")]
    [InlineData("AD")]
    [InlineData("AC")]
    [InlineData("D")]
    [InlineData("E")]
    [InlineData("C")]
    public void Validate_ShouldBeInvalid_WhenDriverLicenseTypeIsNotAllowedValue(string driverLicenseType)
    {
        var sample = _createDeliveryPersonCommandSample with { DriverLicenseType = driverLicenseType };
        var expectedErrorCount = 1;
        var expectedErrorMessage = "Allowed driver license types: A, B or AB";
        
        var result = _validator.Validate(sample);
        
        Assert.False(result.IsValid);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
        Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault()?.ErrorMessage);
    }
    
    [Theory]
    [InlineData("    ")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Validate_ShouldBeInvalid_WhenDriverLicenseImageIsEmpty(string driverLicenseImage)
    {
        var sample = _createDeliveryPersonCommandSample with { Base64DriverLicenseImage = driverLicenseImage };
        var expectedErrorCount = 1;
        var expectedErrorMessage = "Driver License Image is required";
        
        var result = _validator.Validate(sample);
        
        Assert.False(result.IsValid);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
        Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault()?.ErrorMessage);
    }
    
    [Fact]
    public void Validate_ShouldBeInvalid_WhenDriverLicenseImageIsNotJpegOrBmp()
    {
        // Valid for JPEG
        var result = _validator.Validate(_createDeliveryPersonCommandSample with { Base64DriverLicenseImage = Consts.Base64JpegCnh});
        Assert.True(result.IsValid);
        
        // Valid for BMP
        result = _validator.Validate(_createDeliveryPersonCommandSample with { Base64DriverLicenseImage = Consts.Base64BmpCnh});
        Assert.True(result.IsValid);

        var expectedErrorCount = 1;
        var expectedErrorMessage = "Just JPEG or BPM image is allowed";
        
        // Invalid for GIF
        result = _validator.Validate(_createDeliveryPersonCommandSample with { Base64DriverLicenseImage = Consts.Base64GifCnh});
        Assert.False(result.IsValid);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
        Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault()?.ErrorMessage);
        
        // Invalid for PNG
        result = _validator.Validate(_createDeliveryPersonCommandSample with { Base64DriverLicenseImage = Consts.Base64GifCnh});
        Assert.False(result.IsValid);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
        Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault()?.ErrorMessage);
    }
    
    [Fact]
    public void Validate_ShouldBeValid_WheAllPropsAreOk()
    {
        var result = _validator.Validate(_createDeliveryPersonCommandSample);
        Assert.True(result.IsValid);
    }
}