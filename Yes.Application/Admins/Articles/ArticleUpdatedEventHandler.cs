namespace Yes.Application.Admins.Articles
{
    public record ArticleUpdatedEvent(int ArticleId) : INotification;


    public class ArticleUpdatedEventHandler(BlogDbContext db, IMemoryCache cache) : INotificationHandler<ArticleUpdatedEvent>
    {
        private readonly BlogDbContext _db = db;
        private readonly IMemoryCache _cache = cache;
        public async Task Handle(ArticleUpdatedEvent notification, CancellationToken cancellationToken)
        {
            var articleId = notification.ArticleId;

            _cache.Remove("Archives");
        }
    }


}
