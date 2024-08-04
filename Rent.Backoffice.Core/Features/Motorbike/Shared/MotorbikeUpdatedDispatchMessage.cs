namespace Rent.Backoffice.Core.Features.Motorbike.Shared;

public record MotorbikeUpdatedDispatchMessage(Guid Id, int ManufactureYear, string ModelName, string LicensePlate, int UpdateType)
{
    public static MotorbikeUpdatedDispatchMessage From(MotorbikeResponse src, int updateType) => new(src.Id, src.ManufactureYear, src.ModelName, src.LicensePlate, updateType);
    public static MotorbikeUpdatedDispatchMessage FromCreated(MotorbikeResponse src) => From(src, 1);
    public static MotorbikeUpdatedDispatchMessage FromUpdated(MotorbikeResponse src) => From(src, 2);
    public static MotorbikeUpdatedDispatchMessage FromDeleted(MotorbikeResponse src) => From(src, 3);
}