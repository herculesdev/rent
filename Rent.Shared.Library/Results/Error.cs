namespace Rent.Shared.Library.Results;

public record Error(string? Key, string Message)
{
    public Error(string message) : this(null, message)
    {
        
    }
}