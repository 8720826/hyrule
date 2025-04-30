
namespace Yes.Application.Admins.Comments
{

    public record GetCommentsQuery(int? PageIndex, int? PageSize) : IPagedRequest, IRequest<PagedList<GetCommentsQueryResponse>>;

    public record GetCommentsQueryResponse(
        int Id,
		string Content,
        string Email,
        string Url,
        DateTime CreateDate,
		int ArticleId,
        bool HasVerified,
        string Title,
		int UserId,
		string NickName
    );

    public class GetCommentsQueryHandler(BlogDbContext db) : IRequestHandler<GetCommentsQuery, PagedList<GetCommentsQueryResponse>>
    {
        private readonly BlogDbContext _db = db;
        public async Task<PagedList<GetCommentsQueryResponse>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
        {
			var query = from comment in _db.Comments.AsNoTracking()
						join articleTemp in _db.Articles.AsNoTracking() on comment.ArticleId equals articleTemp.Id into tempArticle
                        from article in tempArticle.DefaultIfEmpty()
						join userTemp in _db.Users.AsNoTracking() on article.UserId equals userTemp.Id into tempUser
						from user in tempUser.DefaultIfEmpty()
						orderby comment.Id descending
						select new GetCommentsQueryResponse(
                            comment.Id,
                            comment.Content,
                            comment.Email,
                            comment.Url,
                            comment.CreateDate,
                            comment.ArticleId,
                            comment.Status == CommentStatusEnum.审核通过,
                            article.Title,
							article.UserId,
                            comment.NickName
                        );

			var comments = await query.ToPagedListAsync(request, cancellationToken);

            return comments;
        }
    }
}
