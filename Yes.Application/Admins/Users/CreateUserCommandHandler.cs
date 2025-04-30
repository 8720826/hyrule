namespace Yes.Application.Admins.Users
{
    public record CreateUserCommand(
        string Name,
        string Email,
        string NickName,
        string Password
    ) : IRequest<CreateUserCommandResponse>;

    public record CreateUserCommandResponse(int Id);

    public class CreateUserCommandHandler(BlogDbContext db) : IRequestHandler<CreateUserCommand, CreateUserCommandResponse>
    {
        private readonly BlogDbContext _db = db;

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = UserEntity.Create(request.Name, request.Email, request.NickName, request.Password);
			await _db.Users.AddAsync(user);
			await _db.SaveChangesAsync();

			return new CreateUserCommandResponse(user.Id);
        }
    }
}
