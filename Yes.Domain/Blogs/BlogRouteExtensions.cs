namespace Yes.Domain.Blogs
{
    public static class BlogRouteExtensions
    {
        public static string ToRoute(this string categoryRoute, CategoryModel categoryModel)
        {
            return categoryRoute.Replace(BlogRouteConst.CategoryId.ToPlaceholder(), categoryModel.Id.ToString())
                                                .Replace(BlogRouteConst.CategorySlug.ToPlaceholder(), categoryModel.Slug);
        }

        public static string ToRoute(this string articleRoute, CategoryModel categoryModel, ArticleModel articleModel)
        {
            return articleRoute.Replace(BlogRouteConst.ArticleId.ToPlaceholder(), articleModel.Id.ToString())
                                                .Replace(BlogRouteConst.ArticleSlug.ToPlaceholder(), articleModel.Slug)
                                                .Replace(BlogRouteConst.Year.ToPlaceholder(), articleModel.ModifyDate.ToString("yyyy"))
                                                .Replace(BlogRouteConst.Month.ToPlaceholder(), articleModel.ModifyDate.ToString("MM"))
                                                .Replace(BlogRouteConst.Day.ToPlaceholder(), articleModel.ModifyDate.ToString("dd"))
                                                .Replace(BlogRouteConst.CategoryId.ToPlaceholder(), categoryModel.Id.ToString())
                                                .Replace(BlogRouteConst.CategorySlug.ToPlaceholder(), categoryModel.Slug);
        }

        public static string ToRoute(this string pageRoute, SinglePageModel singlePageModel)
        {
            return pageRoute.Replace(BlogRouteConst.PageId.ToPlaceholder(), singlePageModel.Id.ToString())
                                                .Replace(BlogRouteConst.PageSlug.ToPlaceholder(), singlePageModel.Slug);
        }


        public static string ToRoute(this string tagRoute, TagModel tagModel)
        {
            return tagRoute.Replace(BlogRouteConst.TagSlug.ToPlaceholder(), tagModel.Slug.ToString())
                                                .Replace(BlogRouteConst.TagId.ToPlaceholder(), tagModel.Id.ToString());
        }

        public static string ToRoute(this string archiveRoute, ArchiveModel archiveModel)
        {
            return archiveRoute.Replace(BlogRouteConst.Year.ToPlaceholder(), archiveModel.Year.ToString())
                                                .Replace(BlogRouteConst.Month.ToPlaceholder(), archiveModel.Month.ToString());
        }


        public static string ToPlaceholder(this string routeKey)
        {
            return $"{{{routeKey}}}".ToLower();
        }

        public static string ToPageRoute(this string route, int page)
        {
            return $"{route}?page={page}";
        }

    }
}
