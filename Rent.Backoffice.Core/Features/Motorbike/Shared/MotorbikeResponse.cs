namespace Rent.Backoffice.Core.Features.Motorbike.Shared;

public record MotorbikeResponse(Guid Id, int ManufactureYear, string ModelName, string LicensePlate)
{
    public static MotorbikeResponse From(Entities.Motorbike src) => new(src.Id, src.ManufactureYear, src.ModelName, src.LicensePlate);
}