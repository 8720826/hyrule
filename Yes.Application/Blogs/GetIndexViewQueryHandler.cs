

namespace Yes.Application.Blogs
{

    public record GetIndexViewQuery(NameValueCollection NameValueCollection) : IRequest<IndexViewModel>;

    public record IndexViewModel(
        MetaModel Meta,
        BlogModel Blog,
        List<CategoryModel> Categories,
        List<TagModel> Tags,
        List<ArchiveModel> Archives,
        List<SinglePageModel> SinglePages,
        PagedList<ArticleModel> Articles
    );


    public class GetIndexViewQueryHandler(
        IBlogService blogService, 
        IOptionsMonitor<BlogSettings> options, 
        IMapper mapper
        ) : IRequestHandler<GetIndexViewQuery, IndexViewModel>
    {
        private readonly IBlogService _blogService = blogService;
        private readonly BlogSettings _settings = options.CurrentValue;
        private readonly IMapper _mapper = mapper;

        public async Task<IndexViewModel> Handle(GetIndexViewQuery request, CancellationToken cancellationToken)
        {
            var pageIndex = request.NameValueCollection.QueryInt(BlogRouteConst.PageIndex, 1);

            var blogModel = _mapper.Map<BlogModel>(_settings);

            var categories = await _blogService.GetAllCategories();

            var articles = await _blogService.GetIndexArticles(pageIndex, _settings.PageSizeOfHomepage);

            var pages = await _blogService.GetSinglePages();

            var tags = await _blogService.GetTags();

            var archives = await _blogService.GetArchives();

            var meta = GetMeta(_settings);

            return new IndexViewModel(meta, blogModel, categories, tags, archives, pages, articles);
        }

        private MetaModel GetMeta(BlogSettings blogSettings)
        {
            return new MetaModel
            {
                Description = blogSettings.Description,
                Keywords = blogSettings.Keywords,
                Title = blogSettings.Name,
            };
        }
    }
}
