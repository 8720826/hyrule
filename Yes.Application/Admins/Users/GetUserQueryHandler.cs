namespace Yes.Application.Admins.Users
{

    public record GetUserQuery(int Id) : IRequest<GetUserQueryResponse>;

    public record GetUserQueryResponse(
            int Id,
            string Name,
            string Email,
            string NickName,
            bool IsSystem
    );

    public class GetUserQueryHandler(BlogDbContext db, IMapper mapper) : IRequestHandler<GetUserQuery, GetUserQueryResponse>
    {
        private readonly BlogDbContext _db = db;
        private readonly IMapper _mapper = mapper;
        public async Task<GetUserQueryResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {

            var user = await _db.Users.FindAsync(request.Id);
            var response = _mapper.Map<GetUserQueryResponse>(user);

            return response;
        }
    }

    internal class GetUserMappingProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<UserEntity, GetUserQueryResponse>();
        }
    }
}
