namespace Yes.Domain.Core.Models
{
    public interface IPagedRequest
    {
        public const int MaxPageSize = 100;
        int? PageIndex { get; }
        int? PageSize { get; }

    }



    public record PagedList<T>(List<T> Items, int PageIndex, int PageSize, int TotalItems)
    {
        public bool HasNextPage => PageIndex * PageSize < TotalItems;
        public int NextPage => PageIndex + 1;
        public int PreviousPage => PageIndex - 1;

        public bool HasPreviousPage => PageIndex > 1;

        public int TotalPage => (int)Math.Ceiling((double)TotalItems / PageSize);
    }

    public static class PaginationDatabaseExtensions
    {
        public static async Task<PagedList<TResponse>> ToPagedListAsync<TRequest, TResponse>(this IQueryable<TResponse> query, TRequest request, CancellationToken cancellationToken = default) where TRequest : IPagedRequest
        {
            var pageIndex = request.PageIndex ?? 1;
            var pageSize = request.PageSize ?? 10;

            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(pageIndex, 0);
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(pageSize, 0);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(pageSize, IPagedRequest.MaxPageSize);

            var totalItems = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedList<TResponse>(items, pageIndex, pageSize, totalItems);
        }

    }
}
