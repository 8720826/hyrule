namespace Yes.Infrastructure.ViewEngine
{
    public class BlogViewResult : IResult
    {
        private readonly string _theme;
        private readonly string _viewName;
        private readonly object _model;


        private readonly static ConcurrentDictionary<string, string> _viewLocationsCache = new();

        public BlogViewResult(string theme, string viewName)
        {
            _viewName = viewName;
            _model = new object();
            _theme = theme;
        }

        public BlogViewResult(string theme, string viewName, object model)
        {
            _viewName = viewName;
            _model = model;
            _theme = theme;
        }

        public string ContentType { get; set; } = "text/html";

        public async Task ExecuteAsync(HttpContext httpContext)
        {
            //try
            {
                var fluidViewRenderer = httpContext.RequestServices.GetService<IFluidViewRenderer>();
                if (fluidViewRenderer == null)
                {
                    httpContext.Response.StatusCode = 400;
                    return;
                }
                var options = httpContext.RequestServices.GetService<IOptions<FluidViewEngineOptions>>()?.Value;
                if (options == null)
                {
                    httpContext.Response.StatusCode = 400;
                    return;
                }
                var env = httpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();

                var path = Path.Combine("files", "themes", _theme);

                options.PartialsFileProvider = new FileProviderMapper(env.ContentRootFileProvider, path);
                options.ViewsFileProvider = new FileProviderMapper(env.ContentRootFileProvider, path);

                var viewPath = LocatePageFromViewLocations(_viewName, options);

                if (viewPath == null)
                {
                    httpContext.Response.StatusCode = 404;
                    return;
                }

                httpContext.Response.StatusCode = 200;
                httpContext.Response.ContentType = ContentType;

                var context = new TemplateContext(_model, options.TemplateOptions);
                context.Options.FileProvider = options.PartialsFileProvider;


                await using var sw = new StreamWriter(httpContext.Response.Body);
                await fluidViewRenderer.RenderViewAsync(sw, viewPath, context);
            }
            //catch (Exception ex)
            {
                //httpContext.Response.StatusCode = 400;
                //await httpContext.Response.WriteAsync(ex.ToString());
            }
        }

        private static string LocatePageFromViewLocations(string viewName, FluidViewEngineOptions options)
        {
            if (_viewLocationsCache.TryGetValue(viewName, out var cachedLocation) && cachedLocation != null)
            {
                return cachedLocation;
            }

            var fileProvider = options.ViewsFileProvider;

            foreach (var location in options.ViewsLocationFormats)
            {
                var viewFilename = Path.Combine(string.Format(location, viewName));

                var fileInfo = fileProvider.GetFileInfo(viewFilename);

                if (fileInfo.Exists)
                {
                    _viewLocationsCache[viewName] = viewFilename;
                    return viewFilename;
                }
            }

            return null;
        }
    }
}
