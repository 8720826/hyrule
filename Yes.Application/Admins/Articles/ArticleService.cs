


namespace Yes.Application.Admins.Articles
{
    public class ArticleService(BlogDbContext db) : IArticleService
    {
        private readonly BlogDbContext _db = db;
        public async Task<bool> IsSlugInUse(string slug)
        {
            return await _db.Articles.AnyAsync(x => x.Slug == slug);
        }

        public async Task<string> CreateUniqueSlug()
        {
            var times = 5;
            var slug = UniqueShortIdGenerator.CreateUniqueShortString();
            while (await IsSlugInUse(slug))
            {
                times--;
                if (times <= 0)
                {
                    throw new GeneratorSlugException();
                }
				slug = UniqueShortIdGenerator.CreateUniqueShortString();
			}


            return slug;
		}

	}
}
