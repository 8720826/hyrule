namespace Yes.Application.Admins.Users
{
    public record UpdatePasswordCommand(
        string OriginalPassword,
        string NewPassword
     ) : IRequest<Unit>;


    public class UpdatePasswordCommandHandler(BlogDbContext db, IIdentityContext identityContext) : IRequestHandler<UpdatePasswordCommand, Unit>
    {
        private readonly BlogDbContext _db = db;
        private readonly IIdentityContext _identityContext = identityContext;

        public async Task<Unit> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            var userId = _identityContext.Id;
            var user = await _db.Users.FindAsync(userId);
            if (user == null)
            {
                throw new InvalidPasswordException();
            }

            user.CheckPassword(request.OriginalPassword);
            user.UpdatePassword(request.NewPassword);

            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            return new Unit();
        }
    }
}
