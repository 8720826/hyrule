namespace Yes.Application.Admins.Pages
{
    public record PageAddedEvent(int PageId) : INotification;


    public class PageAddedEventHandler(BlogDbContext db, IMemoryCache cache) : INotificationHandler<PageAddedEvent>
    {
        private readonly BlogDbContext _db = db;
        private readonly IMemoryCache _cache = cache;
        public async Task Handle(PageAddedEvent notification, CancellationToken cancellationToken)
        {
            var pageId = notification.PageId;

            _cache.Remove($"SinglePages_10");

        }
    }


}
