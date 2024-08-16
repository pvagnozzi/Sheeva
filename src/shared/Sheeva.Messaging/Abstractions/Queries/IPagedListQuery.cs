namespace Sheeva.Messaging.Abstractions.Queries;

public interface IPagedListQuery<TResult> : IQuery<IPagedListQuery<TResult>>
{
    int PageIndex { get; }

    int PageSize { get; }
}
