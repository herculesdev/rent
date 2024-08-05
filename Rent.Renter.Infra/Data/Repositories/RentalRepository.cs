using Microsoft.EntityFrameworkCore;
using Rent.Renter.Core.Data;
using Rent.Renter.Core.Entities;
using Rent.Renter.Infra.Data.Contexts;

namespace Rent.Renter.Infra.Data.Repositories;

public class RentalRepository(RenterContext db) : IRentalRepository
{
    public async Task<Rental?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return await db.Set<Rental>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<Rental>> GetByDeliveryPersonId(Guid deliveryPersonId, CancellationToken cancellationToken = default)
    {
        return await db.Set<Rental>().Where(x => x.DeliveryPersonId == deliveryPersonId).ToListAsync(cancellationToken);
    }

    public async Task<Rental> Add(Rental rental, CancellationToken cancellationToken = default)
    {
        await db.Set<Rental>().AddAsync(rental, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        return rental;
    }

    public async Task<Rental> Update(Rental rental, CancellationToken cancellationToken = default)
    {
        db.Set<Rental>().Update(rental);
        await db.SaveChangesAsync(cancellationToken);

        return rental;
    }

    public async Task Delete(Rental rental, CancellationToken cancellationToken = default)
    {
        db.Remove(rental);
        await db.SaveChangesAsync(cancellationToken);
    }
}