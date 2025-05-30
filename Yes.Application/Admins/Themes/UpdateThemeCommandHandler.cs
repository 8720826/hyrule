﻿namespace Yes.Application.Admins.Themes
{
    public record UpdateThemeCommand(
        string Theme
    ) : IRequest<Unit>;


    public class UpdateThemeCommandHandler(
        IOptionsMonitor<BlogSettings> options, 
        IMapper mapper, 
        IThemeService  themeService,
		IConfigurationService configurationService,
        BlogDbContext db,
        IIdentityContext identity
    ) : IRequestHandler<UpdateThemeCommand, Unit>
    {
        private readonly BlogSettings _settings = options.CurrentValue;
        private readonly IMapper _mapper = mapper;
        private readonly IThemeService _themeService = themeService;
        private readonly IConfigurationService _configurationService = configurationService;
        private readonly BlogDbContext _db = db;
        private readonly IIdentityContext _identity = identity;
        public async Task<Unit> Handle(UpdateThemeCommand request, CancellationToken cancellationToken)
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


            _themeService.CheckThemeExists(request.Theme);

            _mapper.Map(request, _settings);

            await _configurationService.SaveConfiguration(_settings);

            return new Unit();
        }
    }
}
