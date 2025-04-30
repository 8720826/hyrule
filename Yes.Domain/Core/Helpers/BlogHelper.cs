

namespace Yes.Domain.Core.Helpers
{
    public class BlogHelper
    {
        public static string GetVersion()
        {
            var version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "";
            if (string.IsNullOrEmpty(version))
            {
                return "";
            }

            if (version.Contains("."))
            {
                version = string.Join(".", version.Split('.').Take(3));
            }

            return version;
        }
    }
}
