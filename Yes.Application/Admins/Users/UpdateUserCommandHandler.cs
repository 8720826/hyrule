namespace Yes.Application.Admins.Users
{
    public record UpdateUserCommand(
        int Id,
        string Name,
        string Email,
        string NickName,
        string Password
    ) : IRequest<Unit>;


    public class UpdateUserCommandHandler(BlogDbContext db) : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly BlogDbContext _db = db;

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _db.Users.FindAsync(request.Id);
            if (user == null)
            {
                throw new UserNotExistsException(request.Id);
            }
            user.Update(request.Name, request.Email, request.NickName, request.Password);
            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            return new Unit();
        }
    }
}
