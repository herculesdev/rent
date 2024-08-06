using MongoDB.Driver;
using Rent.Renter.Core.Data;
using Rent.Renter.Core.Entities;

namespace Rent.Renter.Infra.Data.Repositories;

public class MotorbikeNoSqlRepository : IMotorbikeRepository
{
    private const string DatabaseName = "rent";
    private const int OffsetId = 1;
    private readonly IMongoDatabase _mongoDatabase;
    private readonly IMongoCollection<Motorbike> _motorbikes;
    private readonly IMongoCollection<Motorbike> _motorbikes2024;
    
    public MotorbikeNoSqlRepository(IMongoClient mongoClient)
    {
        _mongoDatabase = mongoClient.GetDatabase(DatabaseName);
        _motorbikes = _mongoDatabase.GetCollection<Motorbike>("motorbikes");
        _motorbikes2024 = _mongoDatabase.GetCollection<Motorbike>("motorbikes2024");
    }
    
    public async Task<Motorbike?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _motorbikes.FindAsync(x => x.Id == id, cancellationToken: cancellationToken);
        await result.MoveNextAsync(cancellationToken: cancellationToken);
        return result.Current.FirstOrDefault();
    }

    public async Task Add(Motorbike motorbike, CancellationToken cancellationToken = default)
    {
        await _motorbikes.InsertOneAsync(motorbike, options: null, cancellationToken);
    }
    
    public async Task AddInto2024Collection(Motorbike motorbike, CancellationToken cancellationToken = default)
    {
        await _motorbikes2024.InsertOneAsync(motorbike, options: null, cancellationToken);
    }

    public async Task Update(Motorbike motorbike, CancellationToken cancellationToken = default)
    {        
        await _motorbikes.ReplaceOneAsync(x => x.Id == motorbike.Id, motorbike, cancellationToken: cancellationToken);
    }

    public async Task Delete(Motorbike motorbike, CancellationToken cancellationToken = default)
    {
        await _motorbikes.DeleteOneAsync(x => x.Id == motorbike.Id, cancellationToken);
    }

    public void SaveOffset(long offset)
    {
        var offsets = _mongoDatabase.GetCollection<MotorbikeOffset>("motorbikelastoffset");
        var offsetObject = new MotorbikeOffset(OffsetId, offset);
        var offsetFromDb = offsets.Find(x => x.Id == OffsetId).FirstOrDefault();
        
        if(offsetFromDb is null)
            offsets.InsertOne(offsetObject);
        else
            offsets.ReplaceOne(x => x.Id == OffsetId, offsetObject);
    }

    public long? GetOffset()
    {
        var offsets = _mongoDatabase.GetCollection<MotorbikeOffset>("motorbikelastoffset");
        var result = offsets.Find(x => x.Id == OffsetId).FirstOrDefault();
        return result?.Offset;
    }
}

internal record MotorbikeOffset(int Id, long Offset);