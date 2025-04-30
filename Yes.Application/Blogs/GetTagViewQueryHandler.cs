

namespace Yes.Application.Blogs
{

    public record GetTagViewQuery(NameValueCollection NameValueCollection) : IRequest<TagViewModel>;

    public record TagViewModel(
        MetaModel Meta,
        BlogModel Blog,
        List<CategoryModel> Categories,
        List<TagModel> Tags,
        List<ArchiveModel> Archives,
        List<SinglePageModel> SinglePages,
        TagModel Tag,
        PagedList<ArticleModel> Articles
    );

    public class GetTagViewQueryHandler(
        IBlogService blogService,
        IOptionsMonitor<BlogSettings> options,
        IMapper mapper) : IRequestHandler<GetTagViewQuery, TagViewModel>
    {
        private readonly IBlogService _blogService = blogService;
        private readonly BlogSettings _settings = options.CurrentValue;
        private readonly IMapper _mapper = mapper;
        public async Task<TagViewModel> Handle(GetTagViewQuery request, CancellationToken cancellationToken)
        {
            var tagId = request.NameValueCollection.QueryInt(BlogRouteConst.TagId);
            var slug = request.NameValueCollection.QueryString(BlogRouteConst.TagSlug);
            var pageIndex = request.NameValueCollection.QueryInt(BlogRouteConst.PageIndex, 1);

            var blogModel = _mapper.Map<BlogModel>(_settings);

            var categories = await _blogService.GetAllCategories();

            var pages = await _blogService.GetSinglePages();

            var tags = await _blogService.GetTags();

            var archives = await _blogService.GetArchives();

            var tag = await _blogService.GetTag(tagId ,slug); 
            if (tag == null)
            {
                throw new PageNotFoundException();
            }

            var articles = await _blogService.GetTagArticles(tag.Id, pageIndex, _settings.PageSizeOfListpage);

            var meta = GetMeta(_settings, tag, pageIndex);

            return new TagViewModel(meta, blogModel, categories, tags, archives, pages, tag, articles);
        }

        private MetaModel GetMeta(BlogSettings blogSettings, TagModel tagModel, int pageIndex)
        {
            var pageStr = pageIndex > 1 ? $" - 第{pageIndex}页" : "";
            return new MetaModel
            {
                Description = blogSettings.Description,
                Keywords = $"{tagModel.Name},{tagModel.Slug}",
                Title = $"标签 {tagModel.Name} 下的文章{pageStr} - {blogSettings.Name}",
            };
        }
    }
}
