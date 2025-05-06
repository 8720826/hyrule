namespace Yes.Application.Admins.Articles
{
    public record CreateArticleCommand(
        int CategoryId,
        string Title,
        string? Slug,
        string Content,
        string? CoverUrl,
        string? Summary,1
        List<string>? Tags
     ) : IRequest<CreateArticleCommandResponse>;

    public record CreateArticleCommandResponse(int Id);

    public class CreateArticleCommandHandler(
        IArticleService articleService, 
        BlogDbContext db, 
        IIdentityContext identity, 
        IMediator mediator) : IRequestHandler<CreateArticleCommand, CreateArticleCommandResponse>
    {
        private readonly IArticleService _articleService = articleService;
        private readonly BlogDbContext _db = db;
		private readonly IIdentityContext _identity = identity;
        private readonly IMediator _mediator = mediator;

        public async Task<CreateArticleCommandResponse> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
        {
            var userId = _identity.Id;

            if (!string.IsNullOrEmpty(request.Slug) && await _articleService.IsSlugInUse(request.Slug))
            {
                throw new SlugInUseException(request.Slug);
            }

            var author = await _db.Users.FindAsync(userId);

            var authorObject = ValueObject<UserEntity>.Create(userId, author);

            var category = await _db.Categories.FindAsync(request.CategoryId);

            var categoryObject = ValueObject<CategoryEntity>.Create(request.CategoryId, category);

            var article = ArticleEntity.CreateArticle(request.Title, request.Content, request.Summary, request.Tags, request.Slug, request.CoverUrl, authorObject, categoryObject);

			if (article.IsSlugEmpty())
			{
				var slug = await _articleService.CreateUniqueSlug();
				article.UpdateSlug(slug);
			}

            var isHasTag = request.Tags?.Count > 0;


            await _db.Articles.AddAsync(article);

            foreach(var tagWord in request.Tags)
            {
                if(!await _db.Tags.AnyAsync(x => x.Name == tagWord))
                {
                    var tag = TagEntity.Create(tagWord);
                    await _db.Tags.AddAsync(tag);
                }
            }

            await _db.SaveChangesAsync();

            await _mediator.Publish(new ArticleUpdatedEvent(article.Id));

            if (isHasTag)
            {

                await mediator.Publish(new ArticleTagUpdatedEvent(article.Id));
            }

            return new CreateArticleCommandResponse(article.Id);
        }
    }
}
