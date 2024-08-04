using Rent.Shared.Library.Entities;

namespace Rent.Backoffice.Core.Entities;

public class Motorbike : Entity
{
    public int ManufactureYear { get; set; }
    public string ModelName { get; set; } = string.Empty;
    public string LicensePlate { get; set; } = String.Empty;

    public Motorbike()
    {
        
    }
    
    public Motorbike(int manufactureYear, string modelName, string licensePlate)
    {
        ManufactureYear = manufactureYear;
        ModelName = modelName;
        LicensePlate = licensePlate;
    }
    
    public Motorbike(Guid id, int manufactureYear, string modelName, string licensePlate) : base(id)
    {
        ManufactureYear = manufactureYear;
        ModelName = modelName;
        LicensePlate = licensePlate;
    }
    
    public Motorbike(Guid id, int manufactureYear, string modelName, string licensePlate, DateTime createdAtUtc, DateTime updatedAtUtc, DateTime? deletedAtUtc, bool isDeleted) : base(id, createdAtUtc, updatedAtUtc, deletedAtUtc, isDeleted)
    {
        ManufactureYear = manufactureYear;
        ModelName = modelName;
        LicensePlate = licensePlate;
    }
}