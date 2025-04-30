namespace Yes.Application.Admins.Categories
{

    public record GetCategoriesQuery() : IRequest<List<GetCategoriesQueryResponse>>;

    public record GetCategoriesQueryResponse(
        int Id,
        string Name,
        int Sort,
        string Slug
    );

    public class GetCategoriesQueryHandler(BlogDbContext db) : IRequestHandler<GetCategoriesQuery, List<GetCategoriesQueryResponse>>
    {
        private readonly BlogDbContext _db = db;
        public async Task<List<GetCategoriesQueryResponse>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            //throw new Exception("111");

            var articles = await _db.Categories.Select(x =>
                new GetCategoriesQueryResponse
                (
                    x.Id,
                    x.Name,
                    x.Sort,
                    x.Slug
                )
            ).ToListAsync(cancellationToken);

            return articles;
        }
    }
}
