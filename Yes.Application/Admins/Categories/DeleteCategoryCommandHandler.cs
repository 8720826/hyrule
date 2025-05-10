namespace Yes.Application.Admins.Categories
{
    public record DeleteCategoryCommand(int Id) : IRequest<Unit>;

    public class DeleteCategoryCommandHandler(
        BlogDbContext db,
        IIdentityContext identity) : IRequestHandler<DeleteCategoryCommand, Unit>
    {
        private readonly BlogDbContext _db = db;
        private readonly IIdentityContext _identity = identity;

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var identityUser = await _db.Users.FindAsync(_identity.Id);
            if (identityUser == null)
            {
                throw new UserNotExistsException(_identity.Id);
            }

            if (!identityUser.IsSystemUser())
            {
                throw new AccessDeniedException();
            }

            var hasArticlesInCategory = await _db.Articles.AnyAsync(x => x.CategoryId == request.Id, cancellationToken);
            if (hasArticlesInCategory)
            {
                throw new DeleteCategoryException("分类下还有文章，无法删除！");
            }

            var categoryCount = await _db.Categories.CountAsync(cancellationToken);
            if (categoryCount <= 1)
            {
                throw new DeleteCategoryException("至少要保留一个分类！");
            }

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
