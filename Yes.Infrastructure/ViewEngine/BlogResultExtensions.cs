namespace Yes.Infrastructure.ViewEngine
{
    public static class BlogResultExtensions
    {
        public static IResult BlogView(this IResultExtensions result, string theme, string viewName)
        {
            return new BlogViewResult(theme, viewName);
        }

        public static IResult BlogView(this IResultExtensions result, string theme, string viewName, object model)
        {
            return new BlogViewResult(theme, viewName, model);
        }
    }
}
