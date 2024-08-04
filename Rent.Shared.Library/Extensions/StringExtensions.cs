namespace Rent.Shared.Library.Extensions;

public static class StringExtensions
{
    public static bool IsDigitsOnly(this string str)
    {
        return str.All(c => c >= '0' && c <= '9');
    }
}