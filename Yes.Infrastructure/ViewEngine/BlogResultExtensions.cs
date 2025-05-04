namespace Yes.Infrastructure.ViewEngine
{
    public static class BlogResultExtensions
    {
        public static Microsoft.AspNetCore.Http.IResult BlogView(this IResultExtensions result, string theme, string viewName)
        {
            return new BlogViewResult(theme, viewName);
        }

        public static Microsoft.AspNetCore.Http.IResult BlogView(this IResultExtensions result, string theme, string viewName, object model)
        {
            return new BlogViewResult(theme, viewName, model);
        }
    }
}
