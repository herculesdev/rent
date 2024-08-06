using Rent.Renter.Core.Entities;

namespace Rent.Renter.Core.Data;

public interface IMotorbikeRepository
{
    Task<Motorbike?> GetById(Guid id, CancellationToken cancellationToken = default);
    Task Add(Motorbike motorbike, CancellationToken cancellationToken = default);
    Task AddInto2024Collection(Motorbike motorbike, CancellationToken cancellationToken = default);
    Task Update(Motorbike motorbike, CancellationToken cancellationToken = default);
    Task Delete(Motorbike motorbike, CancellationToken cancellationToken = default);
    void SaveOffset(long offset);
    long? GetOffset();
}