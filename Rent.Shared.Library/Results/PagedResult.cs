namespace Rent.Shared.Library.Results;

public class PagedResult<T> : Result
{
    public PagedResult(IEnumerable<T> items, int page, int perPage, int totalItemCount,Dictionary<string, object>? extraInfo = null) : base(isSuccess: true, [])
    {
        var enumerable = items as T[] ?? items.ToArray();
        Items = enumerable;
        Meta = new PagedResultMeta(page, perPage, totalItemCount, enumerable.Length, extraInfo);
    }

    public IEnumerable<T> Items { get; }
    public PagedResultMeta Meta { get; }
}

public record PagedResultMeta (int Page, int PerPage, int TotalItemCount, int PageItemCount, Dictionary<string, object>? ExtraInfo = null)
{
    public int PageCount => (int)Math.Ceiling((double)TotalItemCount / PerPage);
}