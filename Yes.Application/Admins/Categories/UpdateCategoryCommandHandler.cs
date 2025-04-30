namespace Yes.Application.Admins.Categories
{
    public record UpdateCategoryCommand(
        int Id,
        string Name,
        string? Slug,
        string? CoverUrl,
        string? Description,
        int Sort
    ) : IRequest<Unit>;


    public class UpdateCategoryCommandHandler(BlogDbContext db, IMediator mediator, ICategorieService categorieService) : IRequestHandler<UpdateCategoryCommand, Unit>
    {
        private readonly BlogDbContext _db = db;
        private readonly IMediator _mediator = mediator;
        private readonly ICategorieService _categorieService = categorieService;
        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _db.Categories.FindAsync(request.Id);
            if (category != null)
            {

                if (!string.IsNullOrEmpty(request.Slug) && request.Slug != category.Slug && await _categorieService.IsSlugInUse(request.Slug))
                {
                    throw new SlugInUseException(request.Slug);
                }

                category.Update(request.Name, request.Slug ?? "", request.CoverUrl ?? "", request.Description ?? "", request.Sort);
                _db.Categories.Update(category);
                await _db.SaveChangesAsync();


                await _mediator.Publish(new CategoryUpdatedEvent(category.Id));
            }

            return new Unit();
        }
    }
}
