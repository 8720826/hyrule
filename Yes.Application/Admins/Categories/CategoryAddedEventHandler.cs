namespace Yes.Application.Admins.Categories
{
    public record CategoryAddedEvent(int CategoryId) : INotification;


    public class CategoryAddedEventHandler(BlogDbContext db, IMemoryCache cache) : INotificationHandler<CategoryAddedEvent>
    {
        private readonly BlogDbContext _db = db;
        private readonly IMemoryCache _cache = cache;
        public async Task Handle(CategoryAddedEvent notification, CancellationToken cancellationToken)
        {
            var categoryId = notification.CategoryId;

            _cache.Remove("AllCategories");

        }
    }


}
