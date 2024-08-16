namespace Sheeva.Core;

public interface IPagedList<T>
{
    IList<T> Items { get; }

    long TotalCount { get; }

    int TotalPages { get; }

    int PageSize { get; }

    int PageIndex { get; }
}
