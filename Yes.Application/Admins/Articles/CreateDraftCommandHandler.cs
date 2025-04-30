namespace Yes.Application.Admins.Articles
{
    public record CreateDraftCommand(
        int CategoryId,
        string Title,
        string? Slug,
        string Content,
        string? CoverUrl,
        string? Summary,
        List<string> Tags
     ) : IRequest<CreateDraftCommandResponse>;

    public record CreateDraftCommandResponse(int Id);

    public class CreateDraftCommandHandler(BlogDbContext db, IIdentityContext identity) : IRequestHandler<CreateDraftCommand, CreateDraftCommandResponse>
    {
        private readonly BlogDbContext _db = db;
		private readonly IIdentityContext _identity = identity;
		public async Task<CreateDraftCommandResponse> Handle(CreateDraftCommand request, CancellationToken cancellationToken)
        {
            var userId = _identity.Id;

            var author = await _db.Users.FindAsync(userId);

            var authorObject = ValueObject<UserEntity>.Create(userId, author);

            var category = await _db.Categories.FindAsync(request.CategoryId);

            var categoryObject = ValueObject<CategoryEntity>.Create(request.CategoryId, category);

            var article = ArticleEntity.CreateDraft(request.Title, request.Content, request.Summary, request.Tags, request.Slug, request.CoverUrl, authorObject, categoryObject);

            await _db.Articles.AddAsync(article);

            await _db.SaveChangesAsync();

            return new CreateDraftCommandResponse(article.Id);
        }
    }
}
