namespace Yes.Application.Admins.Pages
{
    public record PageUpdatedEvent(int PageId) : INotification;


    public class PageUpdatedEventHandler(BlogDbContext db, IMemoryCache cache) : INotificationHandler<PageUpdatedEvent>
    {
        private readonly BlogDbContext _db = db;
        private readonly IMemoryCache _cache = cache;
        public async Task Handle(PageUpdatedEvent notification, CancellationToken cancellationToken)
        {
            var pageId = notification.PageId;

            _cache.Remove($"SinglePages_10");
        }
    }


}
