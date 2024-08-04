using Rent.Shared.Library.Entities;

namespace Rent.Renter.Core.Entities;

public class DeliveryPerson : Entity
{
    public string Name { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public string DriverLicenseNumber { get; set; } = string.Empty;
    public string DriverLicenseType { get; set; } = string.Empty;
    public string DriverLicenseImageName { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }
}