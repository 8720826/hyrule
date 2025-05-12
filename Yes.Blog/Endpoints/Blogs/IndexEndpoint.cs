namespace Yes.Blog.Endpoints.Blogs
{
    public class IndexEndpoint : BlogEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapGet("/{**path}", Handle).AllowAnonymous();

		private async Task<Microsoft.AspNetCore.Http.IResult> Handle(
			IMediator mediator,
			IOptionsMonitor<BlogSettings> options,
			HttpContext context,
            ILogger<IndexEndpoint> logger,
            CancellationToken cancellationToken)
		{
            var settings = options.CurrentValue;
            if (string.IsNullOrEmpty(settings.ConnectionString) || string.IsNullOrEmpty(settings.DatabaseName) || string.IsNullOrEmpty(settings.DatabaseType))
            {
                return Results.Redirect("/Install/Index", false);
            }

            var theme = settings.Theme;
            if (string.IsNullOrEmpty(theme))
            {
                theme = "default";
            }

            try
			{
				var queryValues = context.Request.Query.ToNameValueCollection();
				var path = context.Request.Path.ToString().ToLower();
				var viewName = "";
				if (path == "/")
				{
					viewName = "Index";
				}
				else
				{
					var routes = GetRoutes(settings);
					var dic = GetAllRouteValues(path, routes);
					if (dic.Count == 0)
					{
						return Results.NotFound();
					}
					var route = dic.First();
					viewName = route.Key;
					queryValues.Add(route.Value);
				}

				var response = await mediator.Send(viewName switch
				{
					"Index" => new GetIndexViewQuery(queryValues),
					"Search" => new GetSearchViewQuery(queryValues),
					"Article" => new GetArticleViewQuery(queryValues),
					"Category" => new GetCategoryViewQuery(queryValues),
                    "Tag" => new GetTagViewQuery(queryValues),
                    "Page" => new GetPageViewQuery(queryValues),
                    "Archive"=> new GetArchiveViewQuery(queryValues),
                    _ => new GetIndexViewQuery(queryValues)
				}, cancellationToken);

                return Results.Extensions.BlogView(theme, viewName, new { Model = response });
            }
            catch (PageNotFoundException ex)
            {
                logger.LogError(ex, "PageNotFoundException");

                try
				{
					var response = await mediator.Send(new GetNotFoundViewQuery(), cancellationToken);
					return Results.Extensions.BlogView(theme, "NotFound", new { Model = response });
                }
                catch (Exception exception)
                {
                    logger.LogError(exception, "Exception1");
                    return Results.BadRequest(exception.Message);
                }
            }
            catch (Exception ex)
			{
                logger.LogError(ex, "Exception2");
                return Results.BadRequest(ex.Message);
            }
		}

		private Dictionary<string, string> GetRoutes(BlogSettings settings)
		{
			var routes = new Dictionary<string, string>
			{
                { "Article", settings.ArticleRoute },
                { "Category", settings.CategoryRoute },
                { "Page", settings.PageRoute },
                { "Search",settings.SearchRoute },
                { "Tag",settings.TagRoute  },
                { "Archive",settings.ArchiveRoute  }
            };
			return routes;
		}

		private NameValueCollection GetRouteValues(string template, string path)
		{
			var baseUrl = "http://www.localhost";
			var dic = new Dictionary<string, object>();
			var uriTemplate = new UriTemplate(template, true);
			var uri = new Uri($"{baseUrl}{path}");
			var baseUri = new Uri($"{baseUrl}");
			var results = uriTemplate.Match(baseUri, uri);
			return results?.BoundVariables ?? new NameValueCollection();
		}

		private Dictionary<string, NameValueCollection> GetAllRouteValues(string path, Dictionary<string, string> routes)
		{
			var dic = new Dictionary<string, NameValueCollection>();
			foreach (var item in routes)
			{
				var nameValues = GetRouteValues(item.Value, path);
				if (nameValues.Count > 0)
				{
					dic.Add(item.Key, []);
					foreach (var key in nameValues.AllKeys)
					{
						dic[item.Key].Add(key, nameValues[key]);
					}
				}
			}
			return dic;
		}
	}
}
