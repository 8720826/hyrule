

namespace Yes.Application.Blogs
{

    public record GetArticleViewQuery(NameValueCollection NameValueCollection) : IRequest<ArticleViewModel>;
    public record ArticleViewModel(
        MetaModel Meta,
        BlogModel Blog,
        List<CategoryModel> Categories,
        List<TagModel> Tags,
        List<ArchiveModel> Archives,
        List<SinglePageModel> SinglePages,
        CategoryModel CategoryModel,
        ArticleModel Article,
        PagedList<CommentModel> Comments
    );

    public class GetArticleViewQueryHandler(
        IBlogService blogService,
        IOptionsMonitor<BlogSettings> options,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<GetArticleViewQuery, ArticleViewModel>
    {
        private readonly IBlogService _blogService = blogService;
        private readonly BlogSettings _settings = options.CurrentValue;
        private readonly IMapper _mapper = mapper;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        public async Task<ArticleViewModel> Handle(GetArticleViewQuery request, CancellationToken cancellationToken)
        {
            var articleId = request.NameValueCollection.QueryInt(BlogRouteConst.ArticleId);
            var slug = request.NameValueCollection.QueryString(BlogRouteConst.ArticleSlug);
            var pageIndex = request.NameValueCollection.QueryInt(BlogRouteConst.PageIndex, 1);
            var error = request.NameValueCollection.QueryString(BlogRouteConst.Error);

            var blogModel = _mapper.Map<BlogModel>(_settings);

            var categories = await _blogService.GetAllCategories();

            var pages = await _blogService.GetSinglePages();

            var tags = await _blogService.GetTags();

            var archives = await _blogService.GetArchives();

            var article = await _blogService.GetArticle(articleId, slug);
            if (article == null)
            {
                throw new PageNotFoundException();
            }
            var category = categories.FirstOrDefault(c => c.Id == article.Id);

            var ip = _httpContextAccessor?.HttpContext?.GetIpAddress() ?? "";

            var comments = await _blogService.GetArticleComments(article.Id, ip, pageIndex);

            var meta = GetMeta(_settings, article);

            return new ArticleViewModel(meta, blogModel, categories, tags, archives, pages, category, article, comments);
        }

        private MetaModel GetMeta(BlogSettings blogSettings, ArticleModel articleModel)
        {
            var keywords = articleModel.Tags.Select(x=>$"{x.Name},{x.Slug}").ToList();
            keywords.Add(articleModel.Slug);
            keywords = keywords.Where(x => !string.IsNullOrEmpty(x)).Distinct().ToList();
            return new MetaModel
            {
                Description = articleModel.Summary,
                Keywords = $"{string.Join(",", keywords)}",
                Title = $"{articleModel.Title} - {blogSettings.Name}",
            };
        }
    }
}
