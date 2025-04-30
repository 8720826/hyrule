
namespace Yes.Application.Admins.Articles
{

    public record GetArticlesQuery(int? PageIndex, int? PageSize) : IPagedRequest, IRequest<PagedList<GetArticlesQueryResponse>>;

    public record GetArticlesQueryResponse(
        int Id,
		string Title, 
        DateTime CreateDate,
		int CategoryId,
        string CategoryName,
		int UserId,
		string AuthorName,
		string CoverUrl,
        bool IsDraft
    );

    public class GetArticlesQueryHandler(BlogDbContext db) : IRequestHandler<GetArticlesQuery, PagedList<GetArticlesQueryResponse>>
    {
        private readonly BlogDbContext _db = db;
        public async Task<PagedList<GetArticlesQueryResponse>> Handle(GetArticlesQuery request, CancellationToken cancellationToken)
        {
			var query = from article in _db.Articles.AsNoTracking().Where(x=> x.Type == ArticleTypeEnum.Article && x.Status!= ArticleStatusEnum.Deleted)
						join categoryTemp in _db.Categories.AsNoTracking() on article.CategoryId equals categoryTemp.Id into tempCategory
						from category in tempCategory.DefaultIfEmpty()
						join userTemp in _db.Users.AsNoTracking() on article.UserId equals userTemp.Id into tempUser
						from user in tempUser.DefaultIfEmpty()
						orderby article.Id descending
						select new GetArticlesQueryResponse(
							article.Id,
							article.Title,
							article.CreateDate,
							article.CategoryId,
							category.Name,
							article.UserId,
							user.Name,
                            article.CoverUrl,
                            article.Status == ArticleStatusEnum.Draft
                        );

			var articles = await query.ToPagedListAsync(request, cancellationToken);

            return articles;
        }
    }
}
