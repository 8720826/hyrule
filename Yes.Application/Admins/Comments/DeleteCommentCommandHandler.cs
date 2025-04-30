namespace Yes.Application.Admins.Comments
{
    public record DeleteCommentCommand(int Id) : IRequest<DeleteCommentCommandResponse>;

    public record DeleteCommentCommandResponse(int Id);

    public class DeleteCommentCommandHandler(BlogDbContext db) : IRequestHandler<DeleteCommentCommand, DeleteCommentCommandResponse>
    {
        private readonly BlogDbContext _db = db;

        public async Task<DeleteCommentCommandResponse> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
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
