namespace Yes.Infrastructure.Data.DbContexts
{
    public class BlogDbContext : DbContext
    {

        private readonly string _connectionString;
        private readonly DatabaseTypeEnum _databaseType;
        private readonly string _databaseVersion;

        public BlogDbContext(IConnectionStringProvider connectionStringProvider)
        {
            _connectionString = connectionStringProvider.GetConnectionString();
            _databaseType = connectionStringProvider.GetDatabaseType();
            _databaseVersion = connectionStringProvider.GetDatabaseVersion();
        }


        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {

            _ = _databaseType switch
            {
                DatabaseTypeEnum.SqlServer => builder.UseSqlServer(_connectionString),
                DatabaseTypeEnum.MySql => builder.UseMySql(_connectionString, string.IsNullOrEmpty(_databaseVersion) ? MySqlServerVersion.AutoDetect(_connectionString) : new MySqlServerVersion(_databaseVersion), builder => builder.TranslateParameterizedCollectionsToConstants()),
                DatabaseTypeEnum.PostgreSQL => builder.UseNpgsql(_connectionString),

                _ => throw new DatabaseNotSupportedException()
            };
        }

        public DbSet<ArticleEntity> Articles => Set<ArticleEntity>();
        public DbSet<ArticleTagEntity> ArticleTags => Set<ArticleTagEntity>();
        public DbSet<CategoryEntity> Categories => Set<CategoryEntity>();
        public DbSet<TagEntity> Tags => Set<TagEntity>();
        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<CommentEntity> Comments => Set<CommentEntity>();


    }
}
