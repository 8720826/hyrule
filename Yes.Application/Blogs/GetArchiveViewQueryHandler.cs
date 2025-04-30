

namespace Yes.Application.Blogs
{

    public record GetArchiveViewQuery(NameValueCollection NameValueCollection) : IRequest<ArchiveViewModel>;

    public record ArchiveViewModel(
        MetaModel Meta,
        BlogModel Blog,
        List<CategoryModel> Categories,
        List<TagModel> Tags,
        List<ArchiveModel> Archives,
        List<SinglePageModel> SinglePages,
        ArchiveModel Archive,
        PagedList<ArticleModel> Articles
    );

    public class GetArchiveViewQueryHandler(
        IBlogService blogService,
        IOptionsMonitor<BlogSettings> options,
        IMapper mapper) : IRequestHandler<GetArchiveViewQuery, ArchiveViewModel>
    {
        private readonly IBlogService _blogService = blogService;
        private readonly BlogSettings _settings = options.CurrentValue;
        private readonly IMapper _mapper = mapper;
        public async Task<ArchiveViewModel> Handle(GetArchiveViewQuery request, CancellationToken cancellationToken)
        {
            var year = request.NameValueCollection.QueryInt(BlogRouteConst.Year);
            var month = request.NameValueCollection.QueryInt(BlogRouteConst.Month);
            var pageIndex = request.NameValueCollection.QueryInt(BlogRouteConst.PageIndex, 1);

            var blogModel = _mapper.Map<BlogModel>(_settings);

            var categories = await _blogService.GetAllCategories();

            var pages = await _blogService.GetSinglePages();

            var tags = await _blogService.GetTags();

            var archives = await _blogService.GetArchives();

            var archive = new ArchiveModel
            {
                Year = year,
                Month = month
            };

            var articles = await _blogService.GetArchiveArticles(archive, pageIndex, _settings.PageSizeOfListpage);

            var meta = GetMeta(_settings, archive, pageIndex);

            return new ArchiveViewModel(meta, blogModel, categories, tags, archives, pages, archive, articles);
        }

        private MetaModel GetMeta(BlogSettings blogSettings, ArchiveModel archiveModel, int pageIndex)
        {
            var pageStr = pageIndex > 1 ? $" - 第{pageIndex}页" : "";
            return new MetaModel
            {
                Description = blogSettings.Description,
                Keywords = blogSettings.Keywords,
                Title = $"{archiveModel.Year}年{archiveModel.Month}月{pageStr} - {blogSettings.Name}",
            };
        }
    }
}
