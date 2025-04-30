

namespace Yes.Application.Blogs
{

    public record GetCategoryViewQuery(NameValueCollection NameValueCollection) : IRequest<CategoryViewModel>;

    public record CategoryViewModel(
        MetaModel Meta,
        BlogModel Blog,
        List<CategoryModel> Categories,
        List<TagModel> Tags,
        List<ArchiveModel> Archives,
        List<SinglePageModel> SinglePages,
        CategoryModel Category,
        PagedList<ArticleModel> Articles
    );

    public class GetCategoryViewQueryHandler(
        IBlogService blogService,
        IOptionsMonitor<BlogSettings> options,
        IMapper mapper) : IRequestHandler<GetCategoryViewQuery, CategoryViewModel>
    {
        private readonly IBlogService _blogService = blogService;
        private readonly BlogSettings _settings = options.CurrentValue;
        private readonly IMapper _mapper = mapper;
        public async Task<CategoryViewModel> Handle(GetCategoryViewQuery request, CancellationToken cancellationToken)
        {
            var categoryId = request.NameValueCollection.QueryInt(BlogRouteConst.CategoryId);
            var slug = request.NameValueCollection.QueryString(BlogRouteConst.CategorySlug);
            var pageIndex = request.NameValueCollection.QueryInt(BlogRouteConst.PageIndex, 1);

            var blogModel = _mapper.Map<BlogModel>(_settings);

            var categories = await _blogService.GetAllCategories();

            var pages = await _blogService.GetSinglePages();

            var tags = await _blogService.GetTags();

            var archives = await _blogService.GetArchives();

            var category = categories.FirstOrDefault(x => x.Id == categoryId || x.Slug == slug);
            if (category == null)
            {
                throw new PageNotFoundException();
            }
            categoryId = category.Id;

            var articles = await _blogService.GetCategoryArticles(categoryId, pageIndex, _settings.PageSizeOfListpage);

            var meta = GetMeta(_settings, category, pageIndex);

            return new CategoryViewModel(meta, blogModel, categories, tags, archives, pages, category, articles);
        }

        private MetaModel GetMeta(BlogSettings blogSettings, CategoryModel categoryModel, int pageIndex)
        {
            var pageStr = pageIndex > 1 ? $" - 第{pageIndex}页" : "";
            return new MetaModel
            {
                Description = categoryModel.Description,
                Keywords = $"{categoryModel.Name},{categoryModel.Slug}",
                Title = $"分类 {categoryModel.Name} 下的文章{pageStr} - {blogSettings.Name}",
            };
        }
    }
}
