
namespace Yes.Application.Admins.Themes
{
    public class ThemeService(IWebHostEnvironment env) : IThemeService
    {
        private readonly IWebHostEnvironment _env = env;


        public void CheckThemeExists(string theme)
        {
            var themePath = Path.Combine(_env.ContentRootPath, "files", "themes");

            var directories = Directory.GetDirectories(themePath);

            var jsonPath = Path.Combine(themePath, theme, "config.json");

            if (!File.Exists(jsonPath))
            {
                throw new ThemeNotExistsException(theme);
            }
        }
    }
}
