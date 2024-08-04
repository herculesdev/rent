using Microsoft.EntityFrameworkCore;
using Rent.Renter.Core.Data;
using Rent.Renter.Core.Entities;
using Rent.Renter.Infra.Data.Contexts;

namespace Rent.Renter.Infra.Data.Repositories;

public class DeliveryPersonRepository(RenterContext db) : IDeliveryPersonRepository
{
    public async Task<DeliveryPerson?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return await db.Set<DeliveryPerson>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<DeliveryPerson?> GetByDocumentNumber(string documentNumber, CancellationToken cancellationToken = default)
    {
        return await db.Set<DeliveryPerson>().FirstOrDefaultAsync(x => x.DocumentNumber == documentNumber, cancellationToken);
    }
    public async Task<DeliveryPerson?> GetByDriverLicenseNumber(string driverLicenseNumber, CancellationToken cancellationToken = default)
    {
        return await db.Set<DeliveryPerson>().FirstOrDefaultAsync(x => x.DriverLicenseNumber == driverLicenseNumber, cancellationToken);
    }
    public async Task<DeliveryPerson> Add(DeliveryPerson deliveryPerson, CancellationToken cancellationToken = default)
    {
        await db.Set<DeliveryPerson>().AddAsync(deliveryPerson, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        return deliveryPerson;
    }

    public async Task<DeliveryPerson> Update(DeliveryPerson deliveryPerson, CancellationToken cancellationToken = default)
    {
        db.Set<DeliveryPerson>().Update(deliveryPerson);
        await db.SaveChangesAsync(cancellationToken);

        return deliveryPerson;
    }

    public async Task Delete(DeliveryPerson deliveryPerson, CancellationToken cancellationToken = default)
    {
        db.Remove(deliveryPerson);
        await db.SaveChangesAsync(cancellationToken);
    }
}