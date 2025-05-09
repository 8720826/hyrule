namespace Yes.Application.Admins.Themes
{

    public record GetThemeFilesQuery(string Name) :  IRequest<List<GetThemeFilesQueryResponse>>;

    public record GetThemeFilesQueryResponse(
        string FileName
    );

    public class GetThemeFilesQueryHandler(IOptionsMonitor<BlogSettings> options, IMapper mapper, IWebHostEnvironment env) : IRequestHandler<GetThemeFilesQuery, List<GetThemeFilesQueryResponse>>
    {
        private readonly BlogSettings _settings = options.CurrentValue;
        private readonly IMapper _mapper = mapper;
        private readonly IWebHostEnvironment _env = env;

        public async Task<List<GetThemeFilesQueryResponse>> Handle(GetThemeFilesQuery request, CancellationToken cancellationToken)
        {
            var themeName = request.Name;
            var themes = new List<GetThemeFilesQueryResponse>();
            var themePath = Path.Combine(_env.ContentRootPath, "files", "themes", themeName);
            var files = Directory.GetFiles(themePath).Where(x=>x.EndsWith(".liquid") || x.EndsWith(".css") || x.EndsWith(".js"));



            themes = files.Select(x=>new GetThemeFilesQueryResponse(Path.GetFileName(x))).ToList();

            return themes;
        }
    }
}
