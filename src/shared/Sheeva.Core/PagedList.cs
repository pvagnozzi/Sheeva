namespace Sheeva.Core;

public record PagedList<T> : IPagedList<T>
{
    public PagedList(IEnumerable<T>? items = null, long? count = null, int currentPage = 0, int? pageSize = null)
    {
        this.Items = items?.ToArray() ?? [];
        this.TotalCount = count ?? this.Items.Count;
        this.PageSize = pageSize ?? (int)this.TotalCount;
        this.PageIndex = currentPage;
        this.TotalPages = (int)Math.Ceiling((double)this.TotalCount / this.PageSize);
    }

    public IList<T> Items { get; }

    public int PageIndex { get; }

    public int TotalPages { get; }

    public int PageSize { get; }

    public long TotalCount { get; }
}

public static class PageListExtensions
{
    public static IPagedList<TDest> Map<TSource, TDest>(this IPagedList<TSource> source, Func<TSource, TDest> map) =>
        new PagedList<TDest>(source.Items.Select(map), source.TotalCount, source.PageIndex, source.PageSize);
}
