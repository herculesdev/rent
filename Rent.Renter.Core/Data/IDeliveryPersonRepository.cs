using Rent.Renter.Core.Entities;

namespace Rent.Renter.Core.Data;

public interface IDeliveryPersonRepository
{
    Task<DeliveryPerson?> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<DeliveryPerson?> GetByDocumentNumber(string documentNumber, CancellationToken cancellationToken = default);
    Task<DeliveryPerson?> GetByDriverLicenseNumber(string driverLicenseNumber, CancellationToken cancellationToken = default);
    Task<DeliveryPerson> Add(DeliveryPerson deliveryPerson, CancellationToken cancellationToken = default);
    Task<DeliveryPerson> Update(DeliveryPerson deliveryPerson, CancellationToken cancellationToken = default);
    Task Delete(DeliveryPerson deliveryPerson, CancellationToken cancellationToken = default);
}