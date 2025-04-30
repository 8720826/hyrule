namespace Yes.Domain.Core.Extensions
{
    public static class ArticleTagExtensions
    {


        public static string ToTag(this string[] tags)
        {
            return string.Join(",", tags);
        }

        public static string ToTag(this List<string> tags)
        {
            return string.Join(",", tags);
        }

        public static List<string> ToTags(this string tags)
        {
            if (string.IsNullOrEmpty(tags))
                return new List<string>();

            return tags.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}
