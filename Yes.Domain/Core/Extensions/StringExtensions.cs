namespace Yes.Domain.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsUrlValid(this string url)
        {
            var urlRegex = new Regex(@"^(https?|http)://[^\s/$.?#].[^\s]*$", RegexOptions.IgnoreCase);
            return !string.IsNullOrEmpty(url) && urlRegex.IsMatch(url);
        }


    }
}
