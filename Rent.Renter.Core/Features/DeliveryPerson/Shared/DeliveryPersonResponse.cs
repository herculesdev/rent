namespace Rent.Renter.Core.Features.DeliveryPerson.Shared;

public record DeliveryPersonResponse(
    Guid Id, 
    string Name, 
    string DocumentNumber, 
    string DriverLicenseNumber, 
    string DriverLicenseType,
    DateOnly BirthDate)
{
    public static DeliveryPersonResponse From(Entities.DeliveryPerson deliveryPerson)
    {
        return new DeliveryPersonResponse(deliveryPerson.Id, deliveryPerson.Name, deliveryPerson.DocumentNumber, deliveryPerson.DriverLicenseNumber, deliveryPerson.DriverLicenseType, deliveryPerson.BirthDate);
    }
}