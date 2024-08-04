using Rent.Renter.Core.Data;

namespace Rent.Renter.Infra.Data.Storage;

public class DiskFileStorage : IFileStorage
{
    public void Save(string filename, byte[] fileBytes)
    {
        var baseDir = "storage";
        if (!Directory.Exists(baseDir))
            Directory.CreateDirectory(baseDir);

        using var fs = new FileStream($"{baseDir}/{filename}", FileMode.Create, FileAccess.Write);
        fs.Write(fileBytes, 0, fileBytes.Length);
    }
}