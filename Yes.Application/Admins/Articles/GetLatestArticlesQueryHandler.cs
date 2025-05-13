namespace Yes.Application.Admins.Articles
{

    public record GetLatestArticlesQuery() : IRequest<List<GetLatestArticlesQueryResponse>>;

    public record GetLatestArticlesQueryResponse(
        int Id,
		string Title, 
        DateTime CreateDate,
        bool IsDraft
    );

    public class GetLatestArticlesQueryHandler(BlogDbContext db) : IRequestHandler<GetLatestArticlesQuery, List<GetLatestArticlesQueryResponse>>
    {
        private readonly BlogDbContext _db = db;
        public async Task<List<GetLatestArticlesQueryResponse>> Handle(GetLatestArticlesQuery request, CancellationToken cancellationToken)
        {
            var query = _db.Articles.AsNoTracking().Where(x=>x.Type==ArticleTypeEnum.Article && x.Status != ArticleStatusEnum.Deleted).OrderByDescending(x => x.Id).Select(article =>
                new GetLatestArticlesQueryResponse(
                    article.Id,
                    article.Title,
                    article.CreateDate,
                    article.Status == ArticleStatusEnum.Draft
                 )
            ).Take(10);

			var articles = await query.ToListAsync(cancellationToken);

            return articles;
        }
    }
}
