using System.Collections;

namespace Rent.Shared.Library.Pagination;

public class Paged<T> : PagedBase, IEnumerable<T>
{
    private readonly IEnumerable<T> _items;

    // ReSharper disable once PossibleMultipleEnumeration
    public Paged(IEnumerable<T> items, int page, int perPage, int totalItemCount) : base(page, perPage, totalItemCount, items.Count())
    {
        // ReSharper disable once PossibleMultipleEnumeration
        _items = items;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public override IEnumerable<object> GetItems()
    {
        return _items.Cast<object>();
    }
}