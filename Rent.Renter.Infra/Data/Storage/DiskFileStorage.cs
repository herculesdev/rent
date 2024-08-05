using Rent.Renter.Core.Data;

namespace Rent.Renter.Infra.Data.Storage;

public class DiskFileStorage : IFileStorage
{
    private const string BaseDir = "storage";
    
    public DiskFileStorage()
    {
        
        if (!Directory.Exists(BaseDir))
            Directory.CreateDirectory(BaseDir);
    }
    public void Save(string filename, byte[] fileBytes)
    {
        using var fs = new FileStream($"{BaseDir}/{filename}", FileMode.Create, FileAccess.Write);
        fs.Write(fileBytes, 0, fileBytes.Length);
    }

    public byte[] Read(string filename)
    {
        return File.ReadAllBytes($"{BaseDir}/{filename}");
    }
}