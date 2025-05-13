namespace Yes.Application.Admins.Articles
{
    public record UpdateDraftCommand(
        int Id, 
        int CategoryId,
        string Title,
        string? Slug,
        string Content,
        string? CoverUrl,
        string? Summary,
        List<string> Tags
    ) : IRequest<UpdateDraftCommandResponse>;

    public record UpdateDraftCommandResponse(int Id);

    public class UpdateDraftCommandHandler(BlogDbContext db, IIdentityContext identity) : IRequestHandler<UpdateDraftCommand, UpdateDraftCommandResponse>
    {
        private readonly BlogDbContext _db = db;
		private readonly IIdentityContext _identity = identity;
		public async Task<UpdateDraftCommandResponse> Handle(UpdateDraftCommand request, CancellationToken cancellationToken)
        {
            var userId = _identity.Id;

            var author = await _db.Users.FindAsync(userId);

            var authorObject = ValueObject<UserEntity>.Create(userId, author);

            var category = await _db.Categories.FindAsync(request.CategoryId);

            var categoryObject = ValueObject<CategoryEntity>.Create(request.CategoryId, category);

            var article = await _db.Articles.FindAsync(request.Id);
            if (article == null || article.Status == ArticleStatusEnum.Deleted)
            {
                throw new ArticleNotExistsException(request.Id);
            }

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


            article.UpdateDraft(request.Title, request.Content, request.Summary, request.Tags, request.Slug, request.CoverUrl, authorObject, categoryObject);

            _db.Articles.Update(article);

            await _db.SaveChangesAsync();

            return new UpdateDraftCommandResponse(request.Id);
        }
    }
}
