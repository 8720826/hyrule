namespace Yes.Application
{
    public static class DependencyInjection
    {
        private readonly static Assembly[] Assemblies = AppDomain.CurrentDomain.GetAssemblies();

        public static IServiceCollection RegisterApplicationModule(this IServiceCollection services)
        {

            services.AddMediatR();
            services.AddAutoMapper();

            return services;
        }

        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
            typeAdapterConfig.Scan(Assemblies);

            services.AddSingleton(typeAdapterConfig);
            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }


        public static IServiceCollection AddMediatR(this IServiceCollection services)
        {

            var application = typeof(IAssembly);

            services.AddMediatR(configure =>
            {
                configure.RegisterServicesFromAssembly(application.Assembly);
            });

            return services;
        }


    }
}
