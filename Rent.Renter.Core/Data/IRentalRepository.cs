using Rent.Renter.Core.Entities;

namespace Rent.Renter.Core.Data;

public interface IRentalRepository
{
    Task<Rental?> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<List<Rental>> GetByDeliveryPersonId(Guid deliveryPersonId, CancellationToken cancellationToken = default);
    Task<Rental> Add(Rental rental, CancellationToken cancellationToken = default);
    Task<Rental> Update(Rental rental, CancellationToken cancellationToken = default);
    Task Delete(Rental rental, CancellationToken cancellationToken = default);
}