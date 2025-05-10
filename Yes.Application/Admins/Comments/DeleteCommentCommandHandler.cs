namespace Yes.Application.Admins.Comments
{
    public record DeleteCommentCommand(int Id) : IRequest<DeleteCommentCommandResponse>;

    public record DeleteCommentCommandResponse(int Id);

    public class DeleteCommentCommandHandler(
        BlogDbContext db,
        IIdentityContext identity) : IRequestHandler<DeleteCommentCommand, DeleteCommentCommandResponse>
    {
        private readonly BlogDbContext _db = db;
        private readonly IIdentityContext _identity = identity;
        public async Task<DeleteCommentCommandResponse> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var identityUser = await _db.Users.FindAsync(_identity.Id);
            if (identityUser == null)
            {
                throw new UserNotExistsException(_identity.Id);
            }

            if (!identityUser.IsSystemUser())
            {
                throw new AccessDeniedException();
            }


            var comment = await _db.Comments.FindAsync(request.Id);
            if (comment != null)
            {
                _db.Comments.Remove(comment);
                await _db.SaveChangesAsync();
            }

            return new DeleteCommentCommandResponse(request.Id);
        }
    }
}
