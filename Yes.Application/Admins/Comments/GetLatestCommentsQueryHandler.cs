namespace Yes.Application.Admins.Comments
{

    public record GetLatestCommentsQuery() : IRequest<List<GetLatestCommentsQueryResponse>>;

    public record GetLatestCommentsQueryResponse(
        int Id,
        int ArticleId,
		string Content, 
        DateTime CreateDate,
        bool HasVerified
    );

    public class GetLatestCommentsQueryHandler(BlogDbContext db) : IRequestHandler<GetLatestCommentsQuery, List<GetLatestCommentsQueryResponse>>
    {
        private readonly BlogDbContext _db = db;
        public async Task<List<GetLatestCommentsQueryResponse>> Handle(GetLatestCommentsQuery request, CancellationToken cancellationToken)
        {
            var query = _db.Comments.AsNoTracking().OrderByDescending(x => x.Id).Select(comment =>
                new GetLatestCommentsQueryResponse(
                    comment.Id,
                    comment.ArticleId,
                    comment.Content,
                    comment.CreateDate,
                    comment.Status== CommentStatusEnum.审核通过
                 )
            ).Take(10);

			var comments = await query.ToListAsync(cancellationToken);

            return comments;
        }
    }
}
