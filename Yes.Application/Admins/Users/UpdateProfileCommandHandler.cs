namespace Yes.Application.Admins.Users
{
    public record UpdateProfileCommand(
        string Email,
        string NickName,
        string Url
    ) : IRequest<Unit>;


    public class UpdateProfileCommandHandler(BlogDbContext db, IIdentityContext identityContext) : IRequestHandler<UpdateProfileCommand, Unit>
    {
        private readonly BlogDbContext _db = db;
        private readonly IIdentityContext _identityContext = identityContext;

        public async Task<Unit> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var userId = _identityContext.Id;
            var user = await _db.Users.FindAsync(userId);
            if (user == null)
            {
                throw new UserNotExistsException();
            }
            user.UpdateProfile(request.Email, request.NickName, request.Url);
            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            return new Unit();
        }
    }
}
