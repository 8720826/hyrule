



namespace Yes.Blog.Areas.Install.Pages
{
    public class IndexModel(IOptionsMonitor<BlogSettings> options, IWebHostEnvironment env, IFileService fileService) : BasePageModel
    {
        public IActionResult OnGet()
        {
            var defaultTheme = Path.Combine(env.ContentRootPath, "files", "themes", "default");
            var dir = new DirectoryInfo(defaultTheme);
            if (!dir.Exists)
            {
                Directory.CreateDirectory(defaultTheme);
            }
            if (dir.GetFiles().Length == 0)
            {
                var sourcePath = Path.Combine(env.WebRootPath, "defaulttheme");
                fileService.CopyFolder(sourcePath, defaultTheme);
            }


            var settings = options.CurrentValue;
            if (!string.IsNullOrEmpty(settings.ConnectionString) && !string.IsNullOrEmpty(settings.DatabaseName) && !string.IsNullOrEmpty(settings.DatabaseType))
            {
                return Redirect("/");
            }

            return Page();
        }


    }
}
