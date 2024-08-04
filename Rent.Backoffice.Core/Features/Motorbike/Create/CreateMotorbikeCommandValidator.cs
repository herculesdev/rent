using FluentValidation;
using Rent.Shared.Library.Extensions;

namespace Rent.Backoffice.Core.Features.Motorbike.Create;

public class CreateMotorbikeCommandValidator : AbstractValidator<CreateMotorbikeCommand>
{
    private const int FirstMotorcycleManufactureYear = 1885;
    
    public CreateMotorbikeCommandValidator()
    {
        RuleFor(x => x.ManufactureYear)
            .GreaterThanOrEqualTo(FirstMotorcycleManufactureYear)
            .WithMessage($"Minimum Manufacture Year is {FirstMotorcycleManufactureYear}");

        RuleFor(x => x.ModelName)
            .NotEmpty()
            .WithMessage( "Model is required");
        
        RuleFor(x => x.LicensePlate)
            .NotEmpty()
            .WithMessage( "License Plate is required");
        
        When(x => !string.IsNullOrWhiteSpace(x.LicensePlate), () =>
        {
            RuleFor(x => x.LicensePlate)
                .IsBrazilianValidLicensePlate()
                .WithMessage(x => "License Plate should be in the following format: AAA0A00 or AAA0000");
        });
            
    }
}