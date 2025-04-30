namespace Yes.Application.Admins.Users
{

    public record GetUsersQuery() : IRequest<List<GetUsersQueryResponse>>;

    public record GetUsersQueryResponse(
        int Id,
        string Name,
        string Email,
        string NickName,
        DateTime? RegDate,
        bool IsSystem
    );

    public class GetUsersQueryHandler(BlogDbContext db) : IRequestHandler<GetUsersQuery, List<GetUsersQueryResponse>>
    {
        private readonly BlogDbContext _db = db;
        public async Task<List<GetUsersQueryResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {

            var articles = await _db.Users.Select(x =>
                new GetUsersQueryResponse
                (
                    x.Id,
                    x.Name,
                    x.Email,
                    x.NickName,
                    x.RegDate,
                    x.IsSystem
                )
            ).ToListAsync(cancellationToken);

            return articles;
        }
    }
}
