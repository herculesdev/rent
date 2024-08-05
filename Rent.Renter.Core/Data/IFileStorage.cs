namespace Rent.Renter.Core.Data;

public interface IFileStorage
{
    public void Save(string filename, byte[] fileBytes);
    public byte[] Read(string filename);
}