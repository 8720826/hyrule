namespace Yes.Application.Installs
{
    public record InstallCommand(
            string DatabaseType,
            string DatabaseServer,
            string DatabaseVersion,
            string DatabaseUser,
            string DatabasePassword,
            string DatabaseName,
            bool UseDefaultAdmin
    ) : IRequest<InstallCommandResponse>;

    public record InstallCommandResponse(bool Success, string ErrorMessage = "");

    public class InstallCommandHandler(
        IConnectionStringProvider connectionStringProvider,
        IMigratorProvider migratorProvider,
        IWebHostEnvironment env,
        IConfigurationService configurationService,
        IFileService fileService,
        IOptionsMonitor<BlogSettings> options) : IRequestHandler<InstallCommand, InstallCommandResponse>
    {
        private readonly IConnectionStringProvider _connectionStringProvider = connectionStringProvider;
        private readonly IMigratorProvider _migratorProvider = migratorProvider;
        private readonly BlogSettings _settings = options.CurrentValue;
        private readonly IConfigurationService _configurationService = configurationService;
        private readonly IWebHostEnvironment _env = env;
        private readonly IFileService _fileService = fileService;
        public async Task<InstallCommandResponse> Handle(InstallCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(_settings.ConnectionString) && !string.IsNullOrEmpty(_settings.DatabaseType) && !string.IsNullOrEmpty(_settings.DatabaseName))
            {
                return new InstallCommandResponse(true, $"数据库已安装！");
            }



            try
            {
                var defaultTheme = Path.Combine(_env.ContentRootPath, "files", "themes", "default");
                var dir = new DirectoryInfo(defaultTheme);
                if (dir.GetFiles().Length == 0)
                {
                    var sourcePath = Path.Combine(_env.WebRootPath, "themes", "default");
                    _fileService.CopyFolder(sourcePath, defaultTheme);
                }

                Enum.TryParse(request.DatabaseType, out DatabaseTypeEnum databaseType);
                var databaseServer = request.DatabaseServer;
                var databaseName = request.DatabaseName;
                var databaseVersion = request.DatabaseVersion;

                var settings = new BlogSettings();

                settings.DatabaseVersion = databaseVersion;
                settings.DatabaseName = databaseName;
                settings.DatabaseType = databaseType.ToString();
                settings.ConnectionString = _connectionStringProvider.GetConnectionString(databaseType, databaseServer, databaseName, request.DatabaseUser, request.DatabasePassword);

                _migratorProvider.MigrateUp(databaseType, settings.ConnectionString);

                await _configurationService.SaveConfiguration(settings);

            }
            catch (SqlException ex)
            {
                return new InstallCommandResponse(false, $"数据库连接异常！{ex.Message}");
            }
            catch (OperationCanceledException ex)
            {
                return new InstallCommandResponse(false, $"操作被取消，原因可能是数据库连接失败。{ex.Message}");
            }
            catch (Exception ex)
            {
                return new InstallCommandResponse(false, ex.Message);
            }

            return new InstallCommandResponse(true);
        }


    }
}
