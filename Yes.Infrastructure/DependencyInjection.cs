namespace Yes.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterDataModule(this IServiceCollection services)
        {
            services.AddBlogDbContext();

            return services;
        }

        public static IServiceCollection AddBlogDbContext(this IServiceCollection services)
        {

            services.AddScoped<DbContext, BlogDbContext>();
            services.AddDbContext<BlogDbContext>();

            return services;
        }
    }
}
