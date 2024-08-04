using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Rent.Renter.Core.Entities;

public class Motorbike
{
    public Guid Id { get; set; } 
    public int ManufactureYear { get; set; }
    public string ModelName { get; set; } = string.Empty;
    public string LicensePlate { get; set; } = string.Empty;
}