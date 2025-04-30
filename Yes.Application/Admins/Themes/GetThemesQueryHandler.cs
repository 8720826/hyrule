namespace Yes.Application.Admins.Themes
{

    public record GetThemesQuery() :  IRequest<List<GetThemesQueryResponse>>;

    public record GetThemesQueryResponse(
        string Name,
        string Version,
        string Author,
        string Description,
        string Homepage,
        string DirName,
        bool IsInUse
    );

    public class GetThemesQueryHandler(IOptionsMonitor<BlogSettings> options, IMapper mapper, IWebHostEnvironment env) : IRequestHandler<GetThemesQuery, List<GetThemesQueryResponse>>
    {
        private readonly BlogSettings _settings = options.CurrentValue;
        private readonly IMapper _mapper = mapper;
        private readonly IWebHostEnvironment _env = env;

        public async Task<List<GetThemesQueryResponse>> Handle(GetThemesQuery request, CancellationToken cancellationToken)
        {
            var themes = new List<GetThemesQueryResponse>();
            var themePath = Path.Combine(_env.ContentRootPath, "files", "themes");
            var directories = Directory.GetDirectories(themePath);

            foreach (var directory in directories)
            {
                DirectoryInfo dir = new DirectoryInfo(directory);

                var jsonPath = Path.Combine(directory, "config.json");
                var json = File.ReadAllText(jsonPath);

                var theme = JsonConvert.DeserializeObject<ThemeModel>(json);

                theme.DirName = dir.Name;
                if (theme.DirName == _settings.Theme)
                {
                    theme.IsInUse = true;
                }

                themes.Add(_mapper.Map<GetThemesQueryResponse>(theme));
            }

            themes = themes.OrderByDescending(x=>x.IsInUse).ToList();

            return themes;
        }
    }
}
