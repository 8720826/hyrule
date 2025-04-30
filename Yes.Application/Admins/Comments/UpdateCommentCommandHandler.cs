namespace Yes.Application.Admins.Comments
{
    public record UpdateCommentCommand(
        int Id
    ) : IRequest<UpdateCommentCommandResponse>;

    public record UpdateCommentCommandResponse(int Id);

    public class UpdateCommentCommandHandler(BlogDbContext db, IIdentityContext identity) : IRequestHandler<UpdateCommentCommand, UpdateCommentCommandResponse>
    {
        private readonly BlogDbContext _db = db;
        private readonly IIdentityContext _identity = identity;
        public async Task<UpdateCommentCommandResponse> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _db.Comments.FindAsync(request.Id);
            if (comment == null)
            {
                throw new CommentNotExistsException(request.Id);
            }

            if (comment.IsVerified())
            {
                comment.UnVerify();
            }
            else
            {
                comment.Verify();
            }

            _db.Comments.Update(comment);

            await _db.SaveChangesAsync();

            return new UpdateCommentCommandResponse(request.Id);
        }
    }
}
