namespace Rent.Shared.Library.Consts;

public class FileMagicNumberConsts
{
    public static byte[] Jpeg { get; private set; } = [0xFF, 0xD8, 0xFF];
    public static byte[] Bmp { get; private set; } = [0x42,0x4D];
}