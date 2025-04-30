namespace Yes.Application.Admins.Categories
{
    public record CreateCategoryCommand(
        string Name,
        string? Slug,
        string? CoverUrl,
        string? Description,
        int Sort
    ) : IRequest<CreateCategoryCommandResponse>;

    public record CreateCategoryCommandResponse(int Id);

    public class CreateCategoryCommandHandler(
        BlogDbContext db, 
        IMediator mediator, 
        ICategorieService categorieService) : IRequestHandler<CreateCategoryCommand, CreateCategoryCommandResponse>
    {
        private readonly BlogDbContext _db = db;
        private readonly IMediator _mediator = mediator;
        private readonly ICategorieService _categorieService = categorieService;
        public async Task<CreateCategoryCommandResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.Slug) && await _categorieService.IsSlugInUse(request.Slug))
            {
                throw new SlugInUseException(request.Slug);
            }

            var category = CategoryEntity.Create(request.Name, request.Slug, request.CoverUrl, request.Description, request.Sort);
            await _db.Categories.AddAsync(category);
            await _db.SaveChangesAsync();

            await _mediator.Publish(new CategoryUpdatedEvent(category.Id));

            return new CreateCategoryCommandResponse(category.Id);
        }
    }
}
