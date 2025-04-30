namespace Yes.Application.Admins.Categories
{
    public record CategoryUpdatedEvent(int CategoryId) : INotification;


    public class CategoryUpdatedEventHandler(BlogDbContext db, IMemoryCache cache) : INotificationHandler<CategoryUpdatedEvent>
    {
        private readonly BlogDbContext _db = db;
        private readonly IMemoryCache _cache = cache;
        public async Task Handle(CategoryUpdatedEvent notification, CancellationToken cancellationToken)
        {
            var categoryId = notification.CategoryId;

            _cache.Remove("AllCategories");
        }
    }


}
