namespace Yes.Application.Admins.Articles
{

    public record GetArticleQuery(int Id) :  IRequest<GetArticleQueryResponse>;

    public record GetArticleQueryResponse(
        int Id,
		string Title, 
		int CategoryId,
		string Content,
        string Slug,
        string CoverUrl,
        string? Summary,
        string[] Tags
    );

    public class GetArticleQueryHandler(BlogDbContext db,IMapper mapper) : IRequestHandler<GetArticleQuery, GetArticleQueryResponse>
    {
        private readonly BlogDbContext _db = db;
        private readonly IMapper _mapper = mapper;
        public async Task<GetArticleQueryResponse> Handle(GetArticleQuery request, CancellationToken cancellationToken)
        {
			var article = await _db.Articles.FindAsync(request.Id);
            if (article == null|| article.Status== ArticleStatusEnum.Deleted)
            {
                throw new ArticleNotExistsException(request.Id);
            }

            var response = _mapper.Map<GetArticleQueryResponse>(article);

            return response;
        }
    }

    internal class GetArticleMappingProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<ArticleEntity, GetArticleQueryResponse>()
                .Map(dest => dest.Tags, src => src.Tag.ToTags()); ;
        }
    }


}
