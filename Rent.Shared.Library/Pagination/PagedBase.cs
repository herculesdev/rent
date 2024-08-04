namespace Rent.Shared.Library.Pagination;

public abstract class PagedBase(int page, int perPage, int totalItemCount, int pageItemCount)
{
    public int Page { get; } = page;
    public int PerPage { get; } = perPage;
    public int TotalItemCount { get; } = totalItemCount;
    public int PageItemCount { get; } = pageItemCount;
    public int PageCount => (int)Math.Ceiling((double)TotalItemCount / PerPage);
    public Dictionary<string, object> ExtraInfo { get; set; } = new();
    public abstract IEnumerable<object> GetItems();
}