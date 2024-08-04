namespace Rent.Shared.Library.Extensions;

public static class ByteArrayExtensions
{
    public static bool StartsWith(this byte[] array, byte[] prefix)
    {
        if (prefix.Length > array.Length)
            return false;
        
        for (var i = 0; i < prefix.Length; i++)
        {
            if (array[i] != prefix[i])
                return false;
        }

        return true;
    }
}