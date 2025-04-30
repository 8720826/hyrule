

namespace Yes.Domain.Core.Extensions
{
    public static class NameValueCollectionExtensions
    {
        public static int QueryInt(this NameValueCollection collection, string key, int defaultValue = 0)
        {
            key = key.ToUpper();
            var value = collection.QueryString(key);
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            if (int.TryParse(value, out int result))
            {
                return result;
            }
            return defaultValue;
        }

        public static string QueryString(this NameValueCollection collection, string key)
        {
            key = key.ToUpper();
            if (collection.AllKeys.Contains(key))
            {
                try
                {
                    return collection[key]?.ToString() ?? "";
                }
                catch
                {
                    return "";
                }
            }
            return "";
        }
    }
}
