namespace Yes.Application.Admins.Articles
{
    public record UpdateArticleCommand(
        int Id, 
        int CategoryId,
        string Title,
        string? Slug,
        string Content,
        string? CoverUrl,
        string? Summary,
        List<string>? Tags
    ) : IRequest<UpdateArticleCommandResponse>;

    public record UpdateArticleCommandResponse(int Id);

    public class UpdateArticleCommandHandler(
        IArticleService articleService, 
        BlogDbContext db, 
        IIdentityContext identity, 
        IMediator mediator) : IRequestHandler<UpdateArticleCommand, UpdateArticleCommandResponse>
    {
        private readonly IArticleService _articleService = articleService;
        private readonly BlogDbContext _db = db;
		private readonly IIdentityContext _identity = identity;
        private readonly IMediator _mediator = mediator;
        public async Task<UpdateArticleCommandResponse> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
        {
            var userId = _identity.Id;

            var author = await _db.Users.FindAsync(userId);

            var authorObject = ValueObject<UserEntity>.Create(userId, author);

            var category = await _db.Categories.FindAsync(request.CategoryId);

            var categoryObject = ValueObject<CategoryEntity>.Create(request.CategoryId, category);

            var article = await _db.Articles.FindAsync(request.Id);
            if (article == null)
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



            if (!string.IsNullOrEmpty(request.Slug) && 
                article.IsSlugDifferent(request.Slug) &&
				await _articleService.IsSlugInUse(request.Slug))
			{
				throw new SlugInUseException(request.Slug);
			}

            var isTagUpdated = article.Tag != request.Tags?.ToTag();


            article.UpdateArticle(request.Title, request.Content, request.Summary, request.Tags, request.Slug, request.CoverUrl, authorObject, categoryObject);

			if (article.IsSlugEmpty())
			{
				var slug = await _articleService.CreateUniqueSlug();
				article.UpdateSlug(slug);
			}

			_db.Articles.Update(article);

            foreach (var tagWord in request.Tags)
            {
                if (!await _db.Tags.AnyAsync(x => x.Name == tagWord))
                {
                    var tag = TagEntity.Create(tagWord);
                    await _db.Tags.AddAsync(tag);
                }
            }

            await _db.SaveChangesAsync();

            await _mediator.Publish(new ArticleUpdatedEvent(article.Id));

            if (isTagUpdated)
            {
                await mediator.Publish(new ArticleTagUpdatedEvent(article.Id));
            }
           

            return new UpdateArticleCommandResponse(request.Id);
        }
    }
}
