namespace Yes.Application.Admins.Articles
{
    public record class ArticleTagUpdatedEvent(int ArticleId) : INotification;


    public class ArticleTagUpdatedEventHandler(BlogDbContext db, IMemoryCache cache) : INotificationHandler<ArticleTagUpdatedEvent>
    {
        private readonly BlogDbContext _db = db;
        private readonly IMemoryCache _cache = cache;

        public async Task Handle(ArticleTagUpdatedEvent notification, CancellationToken cancellationToken)
        {
            var articleId = notification.ArticleId;
            var article = await _db.Articles.FindAsync(articleId, cancellationToken);
            if (article == null)
            {
                return;
            }

            var articleTags = _db.ArticleTags.Where(x => x.ArticleId == articleId);

            var tagWords = article.Tag.ToTags();

            var tags = _db.Tags.Where(x => tagWords.Contains(x.Name)).ToList();

            foreach (var articleTag in articleTags)
            {
                if (!tags.Any(x => x.Id == articleTag.TagId))
                {
                    _db.ArticleTags.Remove(articleTag);
                }
            }

            foreach (var tagWord in tagWords)
            {
                var tag = await _db.Tags.FirstOrDefaultAsync(x => x.Name == tagWord);
                if (tag != null)
                {
                    if (!await _db.ArticleTags.AnyAsync(x => x.ArticleId == articleId && x.TagId == tag.Id))
                    {
                        await _db.ArticleTags.AddAsync(new ArticleTagEntity
                        {
                            ArticleId = articleId,
                            TagId = tag.Id
                        });
                    }
                }
            }


            await _db.SaveChangesAsync();


            _cache.Remove("Tags");
        }
    }
}
