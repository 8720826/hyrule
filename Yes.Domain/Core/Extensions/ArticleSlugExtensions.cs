namespace Yes.Domain.Core.Extensions
{
    public static class ArticleSlugExtensions
    {

        public static bool IsSlug(this string input)
        {
            return Regex.IsMatch(input, @"^[a-zA-Z0-9-_]+$");
        }
    }
}
