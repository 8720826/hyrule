namespace Yes.Application.Admins.Configurations
{
    public record UpdateConfigurationCommand(
        string Name,
        string Url,
        string Logo,
        string Keywords,
        string Description,
        string BeiAn,
        string Script,
        int PageSizeOfHomepage,
        int PageSizeOfListpage,
        string ArticleRoute,
        string PageRoute,
        string CategoryRoute,
        string SearchRoute,
        StorageSettings Storage
    ) : IRequest<Unit>;

    public record UpdateConfigurationCommandResponse(int Id);

    public class UpdateConfigurationCommandHandler(
        IOptionsMonitor<BlogSettings> options, 
        IMapper mapper, 
        IConfigurationService configurationService, 
        BlogDbContext db,
        IIdentityContext identity) : IRequestHandler<UpdateConfigurationCommand, Unit>
    {
        private readonly BlogSettings _settings = options.CurrentValue;
        private readonly IMapper _mapper = mapper;
        private readonly IConfigurationService  _configurationService = configurationService;
        private readonly BlogDbContext _db = db;
        private readonly IIdentityContext _identity = identity;

        public async Task<Unit> Handle(UpdateConfigurationCommand request, CancellationToken cancellationToken)
        {

            var identityUser = await _db.Users.FindAsync(_identity.Id);
            if (identityUser == null)
            {
                throw new UserNotExistsException(_identity.Id);
            }

            if (!identityUser.IsSystemUser())
            {
                throw new AccessDeniedException();
            }

            _mapper.Map(request, _settings);

			await _configurationService.SaveConfiguration(_settings);

            return new Unit();
        }
    }

    internal class UpdateConfigurationMappingProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<UpdateConfigurationCommand, BlogSettings>()
                .IgnoreIf((src, dest) => !string.IsNullOrEmpty(dest.Storage.Qiniu.AccessKey), dest => dest.Storage.Qiniu.AccessKey)
                .IgnoreIf((src, dest) => !string.IsNullOrEmpty(dest.Storage.Qiniu.SecretKey), dest => dest.Storage.Qiniu.SecretKey);
        }
    }
}
