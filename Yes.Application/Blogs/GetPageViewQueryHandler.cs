

namespace Yes.Application.Blogs
{

    public record GetPageViewQuery(NameValueCollection NameValueCollection) : IRequest<PageViewModel>;

    public record PageViewModel(
        MetaModel Meta,
        BlogModel Blog,
        List<CategoryModel> Categories,
        List<TagModel> Tags,
        List<ArchiveModel> Archives,
        List<SinglePageModel> SinglePages,
        SinglePageModel SinglePage,
        PagedList<CommentModel> Comments
    );

    public class GetPageViewQueryHandler(
        IBlogService blogService,
        IOptionsMonitor<BlogSettings> options,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetPageViewQuery, PageViewModel>
    {
        private readonly IBlogService _blogService = blogService;
        private readonly BlogSettings _settings = options.CurrentValue;
        private readonly IMapper _mapper = mapper;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<PageViewModel> Handle(GetPageViewQuery request, CancellationToken cancellationToken)
        {
            var articleId = request.NameValueCollection.QueryInt(BlogRouteConst.PageId);
            var slug = request.NameValueCollection.QueryString(BlogRouteConst.PageSlug);
            var pageIndex = request.NameValueCollection.QueryInt(BlogRouteConst.PageIndex, 1);


            var blogModel = _mapper.Map<BlogModel>(_settings);

            var categories = await _blogService.GetAllCategories();

            var pages = await _blogService.GetSinglePages();

            var tags = await _blogService.GetTags();

            var archives = await _blogService.GetArchives();

            var page = pages.FirstOrDefault(x => x.Id == articleId || x.Slug == slug);
            if (page == null)
            {
                throw new PageNotFoundException();
            }

            var ip = _httpContextAccessor?.HttpContext?.GetIpAddress() ?? "";

            var comments = await _blogService.GetArticleComments(page.Id, ip, pageIndex);

            var meta = GetMeta(_settings, page);

            return new PageViewModel(meta , blogModel, categories, tags, archives, pages, page, comments);
        }

        private MetaModel GetMeta(BlogSettings blogSettings, SinglePageModel singlePageModel)
        {
            return new MetaModel
            {
                Description = singlePageModel.Summary,
                Keywords = $"{singlePageModel.Tag},{singlePageModel.Slug}",
                Title = $"{singlePageModel.Title} - {blogSettings.Name}",
            };
        }
    }
}
