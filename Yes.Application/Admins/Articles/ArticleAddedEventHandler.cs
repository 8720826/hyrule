namespace Yes.Application.Admins.Articles
{
    public record ArticleAddedEvent(int ArticleId) : INotification;


    public class ArticleAddedEventHandler(BlogDbContext db, IMemoryCache cache) : INotificationHandler<ArticleAddedEvent>
    {
        private readonly BlogDbContext _db = db;
        private readonly IMemoryCache _cache = cache;
        public async Task Handle(ArticleAddedEvent notification, CancellationToken cancellationToken)
        {
            var articleId = notification.ArticleId;

            _cache.Remove("Archives");

        }
    }


}
