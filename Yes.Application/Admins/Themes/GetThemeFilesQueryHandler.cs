namespace Yes.Application.Admins.Themes
{

    public record GetThemeFilesQuery(string Name) :  IRequest<List<GetThemeFilesQueryResponse>>;

    public record GetThemeFilesQueryResponse(
        string FileName
    );

    public class GetThemeFilesQueryHandler(IWebHostEnvironment env) : IRequestHandler<GetThemeFilesQuery, List<GetThemeFilesQueryResponse>>
    {
        private readonly IWebHostEnvironment _env = env;

        public async Task<List<GetThemeFilesQueryResponse>> Handle(GetThemeFilesQuery request, CancellationToken cancellationToken)
        {
            var themeName = request.Name;
            var themes = new List<GetThemeFilesQueryResponse>();
            var themePath = Path.Combine(_env.ContentRootPath, "files", "themes", themeName);

            var allowedExtensions = new[] { ".liquid", ".css", ".js" };
            var files = Directory.GetFiles(themePath, "*.*", SearchOption.AllDirectories).Where(file => allowedExtensions.Contains(Path.GetExtension(file).ToLowerInvariant()));

            themes = files.Select(x=>new GetThemeFilesQueryResponse(Path.GetRelativePath(themePath, x))).ToList();

            return themes;
        }
    }
}
