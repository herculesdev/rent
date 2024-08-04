using Microsoft.EntityFrameworkCore;
using Rent.Backoffice.Core.Data;
using Rent.Backoffice.Core.Entities;
using Rent.Backoffice.Core.Features.Motorbike.GetPaged;
using Rent.Backoffice.Infra.Data.Contexts;
using Rent.Shared.Library.Pagination;

namespace Rent.Backoffice.Infra.Data.Repositories;

public class MotorbikeRepository(BackofficeContext db) : IMotorbikeRepository
{
    public async Task<Motorbike?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return await db.Set<Motorbike>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
    
    public async Task<Motorbike?> GetByLicensePlate(string licensePlate, CancellationToken cancellationToken = default)
    {
        return await db.Set<Motorbike>().FirstOrDefaultAsync(x => x.LicensePlate == licensePlate, cancellationToken);
    }

    public async Task<Paged<Motorbike>> GetPaged(GetMotorbikesPagedQuery filters, CancellationToken cancellationToken = default)
    {
        var query = db.Set<Motorbike>()
            .Where(x => string.IsNullOrEmpty(filters.SearchString) ||
                        EF.Functions.Like(x.LicensePlate, $"{filters.SearchString}%") ||
                        EF.Functions.Like(x.ModelName, $"{filters.SearchString}%"))
            .AsNoTracking();

        var totalCount = await query.CountAsync(cancellationToken);
        var motorbikes = await query.Skip(filters.GetSkip())
            .Take(filters.PageSize)
            .ToListAsync(cancellationToken);

        return new Paged<Motorbike>(motorbikes, filters.Page, filters.PageSize, totalCount);
    }

    public async Task<Motorbike> Add(Motorbike motorbike, CancellationToken cancellationToken = default)
    {
        await db.Set<Motorbike>().AddAsync(motorbike, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        return motorbike;
    }

    public async Task<Motorbike> Update(Motorbike motorbike, CancellationToken cancellationToken = default)
    {
        db.Set<Motorbike>().Update(motorbike);
        await db.SaveChangesAsync(cancellationToken);

        return motorbike;
    }

    public async Task Delete(Motorbike motorbike, CancellationToken cancellationToken = default)
    {
        db.Remove(motorbike);
        await db.SaveChangesAsync(cancellationToken);
    }
}