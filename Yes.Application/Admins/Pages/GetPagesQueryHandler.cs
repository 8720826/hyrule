namespace Yes.Application.Admins.Pages
{

    public record GetPagesQuery() : IRequest<List<GetPagesQueryResponse>>;

    public record GetPagesQueryResponse(
        int Id,
		string Title, 
        DateTime ModifyDate,
        string Slug
	);

    public class GetPagesQueryHandler(BlogDbContext db) : IRequestHandler<GetPagesQuery, List<GetPagesQueryResponse>>
    {
        private readonly BlogDbContext _db = db;
        public async Task<List<GetPagesQueryResponse>> Handle(GetPagesQuery request, CancellationToken cancellationToken)
        {
            //throw new Exception("111");

            var Pages = await _db.Articles.Where(x=>x.Type== ArticleTypeEnum.Page && x.Status != ArticleStatusEnum.Deleted).Select(x => 
                new GetPagesQueryResponse
                (
                    x.Id,
				    x.Title, 
                    x.ModifyDate,
                    x.Slug
				)
            ).ToListAsync(cancellationToken);

            return Pages;
        }
    }
}
