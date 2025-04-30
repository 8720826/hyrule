namespace Yes.Domain.Categories
{
    public interface ICategorieService : IScoped
    {
		Task<bool> IsSlugInUse(string slug);

		Task<string> EnsureSlug();

	}
}
