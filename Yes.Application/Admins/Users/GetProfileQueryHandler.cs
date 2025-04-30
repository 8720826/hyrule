namespace Yes.Application.Admins.Users
{

    public record GetProfileQuery() : IRequest<GetProfileQueryResponse>;

    public record GetProfileQueryResponse(
            int Id,
            string Name,
            string Email,
            string NickName,
            string Url
    );

    public class GetProfileQueryHandler(BlogDbContext db, IMapper mapper, IIdentityContext identityContext) : IRequestHandler<GetProfileQuery, GetProfileQueryResponse>
    {
        private readonly BlogDbContext _db = db;
        private readonly IMapper _mapper = mapper;
        private readonly IIdentityContext _identityContext = identityContext;
        public async Task<GetProfileQueryResponse> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = _identityContext.Id;
            var user = await _db.Users.FindAsync(userId);
            var response = _mapper.Map<GetProfileQueryResponse>(user);

            return response;
        }
    }

    internal class GetMyProfileMappingProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<UserEntity, GetProfileQueryResponse>();
        }
    }
}
