namespace Yes.Application.Blogs
{
    public record CreateCommentCommand(
            int TargetId,
            string NickName,
            string Email,
            string Url,
            string Content,
            string Referer,
            string IP
     ) : IRequest<CreateCommentCommandResponse>;

    public record CreateCommentCommandResponse(int CommentId, string Referer);

    public class CreateCommentCommandHandler(
        BlogDbContext db,
        IIdentityContext identity) : IRequestHandler<CreateCommentCommand, CreateCommentCommandResponse>
    {
        private readonly BlogDbContext _db = db;
        private readonly IIdentityContext _identity = identity;

        public async Task<CreateCommentCommandResponse> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var article = await _db.Articles.FindAsync(request.TargetId);
            if (article == null)
            {
                throw new PageNotFoundException();
            }

            var comment = CommentEntity.Create(article.Id, 0, request.NickName, request.Content, request.Email, request.Url, request.IP, request.Referer);
            await _db.Comments.AddAsync(comment);
            await _db.SaveChangesAsync();


            return new CreateCommentCommandResponse(comment.Id, request.Referer);
        }
    }
}
