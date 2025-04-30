

namespace Yes.Application.Blogs
{

    public record GetSearchViewQuery(NameValueCollection NameValueCollection) : IRequest<SearchViewModel>;

    public record SearchViewModel(
        MetaModel Meta,
        BlogModel Blog,
        List<CategoryModel> Categories,
        List<TagModel> Tags,
        List<ArchiveModel> Archives,
        List<SinglePageModel> SinglePages,
        string Keyword,
        PagedList<ArticleModel> Articles  
    );

    public class GetSearchViewQueryHandler(
        IBlogService blogService,
        IOptionsMonitor<BlogSettings> options,
        IMapper mapper) : IRequestHandler<GetSearchViewQuery, SearchViewModel>
    {
        private readonly IBlogService _blogService = blogService;
        private readonly BlogSettings _settings = options.CurrentValue;
        private readonly IMapper _mapper = mapper;
        public async Task<SearchViewModel> Handle(GetSearchViewQuery request, CancellationToken cancellationToken)
        {
            var keyword = request.NameValueCollection.QueryString(BlogRouteConst.Keyword);
            var pageIndex = request.NameValueCollection.QueryInt(BlogRouteConst.PageIndex, 1);


            var blogModel = _mapper.Map<BlogModel>(_settings);

            var categories = await _blogService.GetAllCategories();

            var pages = await _blogService.GetSinglePages();

            var tags = await _blogService.GetTags();

            var archives = await _blogService.GetArchives();

            var articles = await _blogService.GetKeywordArticles(keyword, pageIndex, _settings.PageSizeOfListpage);

            var meta = GetMeta(_settings, keyword);

            return new SearchViewModel(meta, blogModel, categories, tags, archives, pages, keyword, articles);
        }

        private MetaModel GetMeta(BlogSettings blogSettings, string keyword)
        {
            return new MetaModel
            {
                Description = blogSettings.Description,
                Keywords = $"{keyword}",
                Title = $"包含关键字 {keyword} 的文章 - {blogSettings.Name}",
            };
        }
    }
}
