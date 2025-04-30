




namespace Yes.Domain.Blogs
{
    public interface IBlogService : IScoped
    {
        Task<List<CategoryModel>> GetAllCategories();

        Task<List<TagModel>> GetTags();

        Task<List<ArchiveModel>> GetArchives();

        Task<PagedList<ArticleModel>> GetCategoryArticles(int categoryId, int pageIndex = 1, int pageSize = 10);

        Task<PagedList<ArticleModel>> GetTagArticles(int tagId, int pageIndex = 1, int pageSize = 10);

        Task<PagedList<ArticleModel>> GetArchiveArticles(ArchiveModel archiveModel, int pageIndex = 1, int pageSize = 10);

        Task<PagedList<ArticleModel>> GetKeywordArticles(string keyword, int pageIndex = 1, int pageSize = 10);

        Task<PagedList<ArticleModel>> GetIndexArticles(int pageIndex = 1, int pageSize = 10);

        Task<List<SinglePageModel>> GetSinglePages(int top = 10);

        Task<ArticleModel> GetArticle(int id, string slug);

        Task<SinglePageModel> GetSinglePage(int id);


        Task<SinglePageModel> GetSinglePage(string slug);

        Task<TagModel> GetTag(int id, string slug);

        Task<PagedList<CommentModel>> GetArticleComments(int articleId,string ip, int pageIndex = 1, int pageSize = 10);
    }
}
