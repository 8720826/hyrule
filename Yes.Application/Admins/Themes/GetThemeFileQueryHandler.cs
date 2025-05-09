namespace Yes.Application.Admins.Themes
{

    public record GetThemeFileQuery(string ThemeName, string FileName) :  IRequest<GetThemeFileQueryResponse>;

    public record GetThemeFileQueryResponse(
        string Content
    );

    public class GetThemeFileQueryHandler(IOptionsMonitor<BlogSettings> options, IMapper mapper, IWebHostEnvironment env) : IRequestHandler<GetThemeFileQuery, GetThemeFileQueryResponse>
    {
        private readonly BlogSettings _settings = options.CurrentValue;
        private readonly IMapper _mapper = mapper;
        private readonly IWebHostEnvironment _env = env;

        public async Task<GetThemeFileQueryResponse> Handle(GetThemeFileQuery request, CancellationToken cancellationToken)
        {
            var themeName = request.ThemeName; 
            var fileName = request.FileName;

            var themes = new List<GetThemeFileQueryResponse>();
            var filePath = Path.Combine(_env.ContentRootPath, "files", "themes", themeName, fileName);
            var content = "";
            if (File.Exists(filePath))
            {
                content = File.ReadAllText(filePath);
            }

            return new GetThemeFileQueryResponse(content);
        }
    }
}
