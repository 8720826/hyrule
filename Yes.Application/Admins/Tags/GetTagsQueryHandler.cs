

namespace Yes.Application.Admins.Tags
{

    public record GetTagsQuery(int? PageIndex, int? PageSize) : IPagedRequest, IRequest<PagedList<GetTagsQueryResponse>>;

    public record GetTagsQueryResponse(
        int Id,
		string Name,
        string Slug,
        DateTime CreateDate
    );

    public class GetTagsQueryHandler(BlogDbContext db) : IRequestHandler<GetTagsQuery, PagedList<GetTagsQueryResponse>>
    {
        private readonly BlogDbContext _db = db;
        public async Task<PagedList<GetTagsQueryResponse>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {
			var query = from tag in _db.Tags.AsNoTracking()
						orderby tag.Id descending
						select new GetTagsQueryResponse(
                            tag.Id,
                            tag.Name,
                            tag.Slug,
                            tag.CreateDate
                        );

			var Tags = await query.ToPagedListAsync(request, cancellationToken);

            return Tags;
        }
    }
}
