namespace Yes.Application.Admins.Articles
{
    public record DeleteArticleCommand(int Id) : IRequest<DeleteArticleCommandResponse>;

    public record DeleteArticleCommandResponse(int Id);

    public class DeleteArticleCommandHandler(
        BlogDbContext db, 
        IIdentityContext identity) : IRequestHandler<DeleteArticleCommand, DeleteArticleCommandResponse>
    {
        private readonly BlogDbContext _db = db;
        private readonly IIdentityContext _identity = identity;

        public async Task<DeleteArticleCommandResponse> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
        {
            var article = await _db.Articles.FindAsync(request.Id);
			if (article != null)
            {
                var identityUser = await _db.Users.FindAsync(_identity.Id);
                if (identityUser == null)
                {
                    throw new UserNotExistsException(_identity.Id);
                }

                if (!identityUser.IsSystemUser())
                {
                    if (article.UserId != identityUser.Id)
                    {
                        throw new AccessDeniedException();
                    }
                }


                article.Delete();
				_db.Articles.Update(article);
				await _db.SaveChangesAsync();
			}

			return new DeleteArticleCommandResponse(request.Id);
        }
    }
}
