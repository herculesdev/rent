using Rent.Backoffice.Core.Entities;
using Rent.Backoffice.Core.Features.Motorbike.GetPaged;
using Rent.Shared.Library.Pagination;

namespace Rent.Backoffice.Core.Data;

public interface IMotorbikeRepository
{
    Task<Motorbike?> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<Motorbike?> GetByLicensePlate(string licensePlate, CancellationToken cancellationToken = default);
    Task<Paged<Motorbike>> GetPaged(GetMotorbikesPagedQuery filters, CancellationToken cancellationToken = default);
    Task<Motorbike> Add(Motorbike motorbike, CancellationToken cancellationToken = default);
    Task<Motorbike> Update(Motorbike motorbike, CancellationToken cancellationToken = default);
    Task Delete(Motorbike motorbike, CancellationToken cancellationToken = default);
}