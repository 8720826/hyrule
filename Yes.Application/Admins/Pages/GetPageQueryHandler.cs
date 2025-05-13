namespace Yes.Application.Admins.Pages
{

    public record GetPageQuery(int Id) :  IRequest<GetPageQueryResponse>;

    public record GetPageQueryResponse(
        int Id,
		string Title, 
		int CategoryId,
		string Content,
        string Slug,
        string CoverUrl,
        string? Summary,
        string[] Tags,
        int Sort
    );

    public class GetPageQueryHandler(BlogDbContext db,IMapper mapper) : IRequestHandler<GetPageQuery, GetPageQueryResponse>
    {
        private readonly BlogDbContext _db = db;
        private readonly IMapper _mapper = mapper;
        public async Task<GetPageQueryResponse> Handle(GetPageQuery request, CancellationToken cancellationToken)
        {
			var page = await _db.Articles.FindAsync(request.Id);
            if (page == null || page.Status == ArticleStatusEnum.Deleted)
            {
                throw new PageNotExistsException(request.Id);
            }

            var response = _mapper.Map<GetPageQueryResponse>(page);

            return response;
        }
    }

    internal class CreatePageMappingProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<ArticleEntity, GetPageQueryResponse>()
                .Map(dest => dest.Tags, src => src.Tag.ToTags()); ;
        }
    }


}
