namespace Yes.Application.Admins.Tags
{
    public record UpdateTagCommand(
        int Id,
        string Name,
        string? Slug
    ) : IRequest<Unit>;


    public class UpdateTagCommandHandler(BlogDbContext db) : IRequestHandler<UpdateTagCommand, Unit>
    {
        private readonly BlogDbContext _db = db;

        public async Task<Unit> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            var slug = (request.Slug ?? "").ToLower();
            var tag = await _db.Tags.FindAsync(request.Id);
            if (tag != null)
            {
                if (await _db.Tags.AnyAsync(x => x.Slug == slug))
                {
                    throw new SlugInUseException(slug);
                }
                tag.Update(slug);

                _db.Tags.Update(tag);
                await _db.SaveChangesAsync();
            }


            return new Unit();
        }
    }
}
