namespace Yes.Application.Admins.Tags
{

    public record GetTagQuery(int Id) : IRequest<GetTagQueryResponse>;

    public record GetTagQueryResponse(
        int Id,
        string Name,
        string Slug,
        DateTime CreateDate
    );

    public class GetTagQueryHandler(BlogDbContext db, IMapper mapper) : IRequestHandler<GetTagQuery, GetTagQueryResponse>
    {
        private readonly BlogDbContext _db = db;
        private readonly IMapper _mapper = mapper;
        public async Task<GetTagQueryResponse> Handle(GetTagQuery request, CancellationToken cancellationToken)
        {

            var tag = await _db.Tags.FindAsync(request.Id);
            var response = _mapper.Map<GetTagQueryResponse>(tag);

            return response;
        }
    }

    internal class GetTagMappingProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<TagEntity, GetTagQueryResponse>();
        }
    }
}
