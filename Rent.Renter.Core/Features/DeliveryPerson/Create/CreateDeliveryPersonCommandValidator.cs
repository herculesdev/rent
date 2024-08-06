using FluentValidation;
using Rent.Shared.Library.Extensions;

namespace Rent.Renter.Core.Features.DeliveryPerson.Create;

public class CreateDeliveryPersonCommandValidator : AbstractValidator<CreateDeliveryPersonCommand>
{
    public CreateDeliveryPersonCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required");

        RuleFor(x => x.DocumentNumber)
            .NotEmpty()
            .WithMessage("Document Number is required");

        When(x => !string.IsNullOrWhiteSpace(x.DocumentNumber), () =>
        {
            RuleFor(x => x.DocumentNumber)
                .IsCnpj()
                .WithMessage("Document Number isn't valid brazilian company document number");
        });
        
        RuleFor(x => x.DriverLicenseNumber)
            .NotEmpty()
            .WithMessage("Driver License Number is required");
        
        When(x => !string.IsNullOrWhiteSpace(x.DriverLicenseNumber), () =>
        {
            RuleFor(x => x.DriverLicenseNumber)
                .IsBrazilianValidDriverLicenseNumber()
                .WithMessage("Driver License Number isn't valid");
        });
        
        RuleFor(x => x.DriverLicenseType)
            .NotEmpty()
            .WithMessage("Driver License Type is required");

        When(x => !string.IsNullOrWhiteSpace(x.DriverLicenseType), () =>
        {
            var allowedLicenseTypes = new[] { "A", "B", "AB" };
            RuleFor(x => x.DriverLicenseType)
                .Must(x => allowedLicenseTypes.Contains(x))
                .WithMessage("Allowed driver license types: A, B or AB");
        });
        
        RuleFor(x => x.Base64DriverLicenseImage)
            .NotEmpty()
            .WithMessage("Driver License Image is required");

        When(x => !string.IsNullOrWhiteSpace(x.Base64DriverLicenseImage), () =>
        {
            RuleFor(x => x.Base64DriverLicenseImage)
                .IsBase64ValidJpegOrBmp()
                .WithMessage("Just JPEG or BPM image is allowed");
        });
    }
}