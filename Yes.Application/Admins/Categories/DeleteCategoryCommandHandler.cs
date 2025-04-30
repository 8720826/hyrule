namespace Yes.Application.Admins.Categories
{
    public record DeleteCategoryCommand(int Id) : IRequest<Unit>;

    public class DeleteCategoryCommandHandler(BlogDbContext db) : IRequestHandler<DeleteCategoryCommand, Unit>
    {
        private readonly BlogDbContext _db = db;

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _db.Categories.FindAsync(request.Id);
            if (category != null)
            {
                _db.Categories.Remove(category);
                await _db.SaveChangesAsync();
            }

            return new Unit();
        }
    }
}
