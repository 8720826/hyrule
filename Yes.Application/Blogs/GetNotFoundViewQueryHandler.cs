namespace Yes.Application.Blogs
{

    public record GetNotFoundViewQuery() : IRequest<NotFoundViewModel>;

    public record NotFoundViewModel(
        MetaModel Meta,
        BlogModel Blog,
        List<CategoryModel> Categories,
        List<TagModel> Tags,
        List<ArchiveModel> Archives,
        List<SinglePageModel> SinglePages
    );

    public class GetNotFoundViewQueryHandler(
        IBlogService blogService,
        IOptionsMonitor<BlogSettings> options,
        IMapper mapper) : IRequestHandler<GetNotFoundViewQuery, NotFoundViewModel>
    {
        private readonly IBlogService _blogService = blogService;
        private readonly BlogSettings _settings = options.CurrentValue;
        private readonly IMapper _mapper = mapper;
        public async Task<NotFoundViewModel> Handle(GetNotFoundViewQuery request, CancellationToken cancellationToken)
        {
            var blogModel = _mapper.Map<BlogModel>(_settings);

            var categories = await _blogService.GetAllCategories();

            var pages = await _blogService.GetSinglePages();

            var tags = await _blogService.GetTags();

            var archives = await _blogService.GetArchives();

            var meta = GetMeta(_settings);

            return new NotFoundViewModel(meta, blogModel, categories, tags, archives, pages);
        }

        private MetaModel GetMeta(BlogSettings blogSettings)
        {
            return new MetaModel
            {
                Description = blogSettings.Description,
                Keywords = blogSettings.Keywords,
                Title = $"页面没找到 - {blogSettings.Name}",
            };
        }
    }
}
