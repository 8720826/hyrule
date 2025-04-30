namespace Yes.Application.Admins.Categories
{

    public record GetCategoryQuery(int Id) : IRequest<GetCategoryQueryResponse>;

    public record GetCategoryQueryResponse(
        int Id,
        string Name,
        int Sort,
        string Slug,
        string CoverUrl,
        string? Description
    );

    public class GetCategoryQueryHandler(BlogDbContext db, IMapper mapper) : IRequestHandler<GetCategoryQuery, GetCategoryQueryResponse>
    {
        private readonly BlogDbContext _db = db;
        private readonly IMapper _mapper = mapper;
        public async Task<GetCategoryQueryResponse> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {

            var category = await _db.Categories.FindAsync(request.Id);
            var response = _mapper.Map<GetCategoryQueryResponse>(category);

            return response;
        }
    }

    internal class GetCategoryMappingProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<CategoryEntity, GetCategoryQueryResponse>();
        }
    }
}
