

namespace Yes.Domain.Articles
{
    public interface IArticleService : IScoped
    {
        Task<bool> IsSlugInUse(string slug);

		Task<string> CreateUniqueSlug();


	}
}
