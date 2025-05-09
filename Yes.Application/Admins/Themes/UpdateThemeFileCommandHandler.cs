namespace Yes.Application.Admins.Themes
{
    public record UpdateThemeFileCommand(
           string ThemeName,
           string FileName,
           string Content
    ) : IRequest<Unit>;


    public class UpdateThemeFileCommandHandler(
        IThemeService  themeService, 
        IWebHostEnvironment env
    ) : IRequestHandler<UpdateThemeFileCommand, Unit>
    {
        private readonly IThemeService _themeService = themeService;
        private readonly IWebHostEnvironment _env = env;
        public async Task<Unit> Handle(UpdateThemeFileCommand request, CancellationToken cancellationToken)
        {
            var themeName = request.ThemeName;
            var fileName = request.FileName;
            var content = request.Content;
            _themeService.CheckThemeExists(themeName);
            var filePath = Path.Combine(_env.ContentRootPath, "files", "themes", themeName, fileName);
            if (File.Exists(filePath))
            {
                File.WriteAllText(filePath, content);
            }
            return new Unit();
        }
    }
}
