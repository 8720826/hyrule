namespace Yes.Application.Admins.Statistics
{

    public record GetStatsQuery() : IRequest<GetStatsQueryResponse>;

    public record GetStatsQueryResponse(
        int ArticleCount, int CategoryCount, int CommentCount
    );

    public class GetStatsQueryHandler(BlogDbContext db) : IRequestHandler<GetStatsQuery, GetStatsQueryResponse>
    {
        private readonly BlogDbContext _db = db;
        public async Task<GetStatsQueryResponse> Handle(GetStatsQuery request, CancellationToken cancellationToken)
        {
            var articleCount = await _db.Articles.CountAsync(x => x.Type == ArticleTypeEnum.Article && x.Status != ArticleStatusEnum.Deleted, cancellationToken);

            var categoryCount = await _db.Categories.CountAsync(cancellationToken);

            var commentCount = await _db.Comments.CountAsync(cancellationToken);

            return new GetStatsQueryResponse(articleCount, categoryCount, commentCount);
        }
    }
}
