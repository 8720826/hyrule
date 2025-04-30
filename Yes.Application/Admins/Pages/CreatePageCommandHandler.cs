namespace Yes.Application.Admins.Pages
{
    public record CreatePageCommand(
        string Title,
        string? Slug,
        string Content,
        string? CoverUrl,
        string? Summary,
        int Sort
     ) : IRequest<CreatePageCommandResponse>;

    public record CreatePageCommandResponse(int Id);

    
    public class CreatePageCommandHandler(
        IArticleService articleService, 
        BlogDbContext db, 
        IIdentityContext identity, 
        IMediator mediator) : IRequestHandler<CreatePageCommand, CreatePageCommandResponse>
    {
        private readonly IArticleService _articleService = articleService;
        private readonly BlogDbContext _db = db;
        private readonly IIdentityContext _identity = identity;
        private readonly IMediator _mediator = mediator;
        public async Task<CreatePageCommandResponse> Handle(CreatePageCommand request, CancellationToken cancellationToken)
        {
            var userId = _identity.Id;

            if (!string.IsNullOrEmpty(request.Slug) && await _articleService.IsSlugInUse(request.Slug))
            {
                throw new SlugInUseException(request.Slug);
            }

            var author = await _db.Users.FindAsync(userId);

            var authorObject = ValueObject<UserEntity>.Create(userId, author);

            var page = ArticleEntity.CreatePage(request.Title, request.Content, request.Summary, request.Slug, request.CoverUrl, request.Sort, authorObject);

            await _db.Articles.AddAsync(page);
            await _db.SaveChangesAsync();

            await _mediator.Publish(new PageAddedEvent(page.Id));


            return new CreatePageCommandResponse(page.Id);
        }
    }
}
