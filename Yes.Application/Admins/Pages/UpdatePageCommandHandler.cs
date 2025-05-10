namespace Yes.Application.Admins.Pages
{
    public record UpdatePageCommand(
        int Id, 
        string Title,
        string? Slug,
        string Content,
        string? CoverUrl,
        string? Summary,
        int Sort
    ) : IRequest<UpdatePageCommandResponse>;

    public record UpdatePageCommandResponse(int Id);

    public class UpdatePageCommandHandler(
        IArticleService articleService, 
        BlogDbContext db, 
        IIdentityContext identity, 
        IMediator mediator) : IRequestHandler<UpdatePageCommand, UpdatePageCommandResponse>
    {
        private readonly IArticleService _articleService = articleService;
        private readonly BlogDbContext _db = db;
        private readonly IIdentityContext _identity = identity;
        private readonly IMediator _mediator = mediator;

        public async Task<UpdatePageCommandResponse> Handle(UpdatePageCommand request, CancellationToken cancellationToken)
        {
            var userId = _identity.Id;

            var author = await _db.Users.FindAsync(userId);

            var authorObject = ValueObject<UserEntity>.Create(userId, author);

            var page = await _db.Articles.FindAsync(request.Id);
            if (page == null)
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
                if (page.UserId != identityUser.Id)
                {
                    throw new AccessDeniedException();
                }
            }


            if (!string.IsNullOrEmpty(request.Slug) && 
                request.Slug != page.Id.ToString() &&
                request.Slug != page.Slug.ToString() &&
                await _articleService.IsSlugInUse(request.Slug))
            {
                throw new SlugInUseException(request.Slug);
            }

            page.UpdatePage(request.Title, request.Content, request.Summary, request.Slug, request.CoverUrl, request.Sort, authorObject);

            if (page.IsSlugEmpty())
            {
				var slug = await _articleService.CreateUniqueSlug();
                page.UpdateSlug(slug);
			}


			_db.Articles.Update(page);

            await _db.SaveChangesAsync();

            await _mediator.Publish(new PageUpdatedEvent(page.Id));

            return new UpdatePageCommandResponse(request.Id);
        }
    }
}
