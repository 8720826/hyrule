using Microsoft.AspNetCore.Http;

namespace Yes.Blog
{
    public static class DependencyInjection
    {
        private readonly static Assembly[] Assemblies = AppDomain.CurrentDomain.GetAssemblies();


        public static IServiceCollection RegisterBlogModule(this IServiceCollection services, WebApplicationBuilder builder)
        {
            var env = builder.Environment;

            services.AddRazorPages();
            services.AddEndpoints();
            services.AddViewEngine(env);
            services.ConfigureValidator();
            services.AddAutoMapper();
            services.AutoRegister();
            services.AddMemoryCache();
            services.AddBackgroundService();
            services.AddMediatR();
            services.AddHttpClient();

            services.AddConfiguration(builder);

            services.AddHttpContextAccessor();

            services.AddUserAuthentication();

            services.AddAuthorization();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Info = new()
                    {
                        Title = "API",
                        Version = "V1",
                        Description = ""
                    };


                    return Task.CompletedTask;
                });

            });

            return services;
        }


        public static IServiceCollection AddConfiguration(this IServiceCollection services, WebApplicationBuilder builder)
        {
            var basePath = Path.Combine(builder.Environment.ContentRootPath, "files", "config");

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            IConfiguration configuration = new ConfigurationBuilder()
              .SetBasePath(basePath)
              .AddJsonFile("appsettings.json", true, true)
              .AddJsonFile("appsettings.blog.json", true, true)
              .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
              .Build();

            services.Configure<BlogSettings>(configuration);

            return services;
        }

        public static IServiceCollection AddBackgroundService(this IServiceCollection services)
        {
            var allAssemblies = Assembly.GetEntryAssembly()?.GetReferencedAssemblies().OrderBy(x => x.Name).Select(Assembly.Load).ToList() ?? new List<Assembly>();
            foreach (var serviceAsm in allAssemblies)
            {
                var serviceList = serviceAsm.GetTypes().Where(t => t.IsClass && !t.IsAbstract && !t.IsInterface);
                foreach (Type serviceType in serviceList.Where(t => t.IsSubclassOf(typeof(BackgroundService))))
                {
                    services.AddTransient(typeof(IHostedService), serviceType);
                }
            }
            return services;
        }



        public static IServiceCollection AddRazorPages(this IServiceCollection services)
        {
            services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizeAreaFolder("Admin", "/");
                options.Conventions.AllowAnonymousToAreaPage("Admin", "/Login");
            });

            services.Configure<RouteOptions>(option =>
            {
                option.LowercaseUrls = true;
                option.LowercaseQueryStrings = true;
            });

            services.AddHttpClient();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }


        public static IServiceCollection AutoRegister(this IServiceCollection services)
        {
            var allAssemblies = Assembly.GetEntryAssembly()?.GetReferencedAssemblies().OrderBy(x => x.Name).Select(Assembly.Load).ToList() ?? new List<Assembly>();
            foreach (var serviceAsm in allAssemblies)
            {
                var serviceList = serviceAsm.GetTypes().Where(t => t.IsClass && !t.IsAbstract && !t.IsInterface);

                foreach (Type serviceType in serviceList.Where(t => typeof(IScoped).IsAssignableFrom(t)))
                {
                    var interfaceTypes = serviceType.GetInterfaces();

                    foreach (var interfaceType in interfaceTypes)
                    {
                        services.AddScoped(interfaceType, serviceType);
                    }
                }

                foreach (Type serviceType in serviceList.Where(t => typeof(ISingleton).IsAssignableFrom(t)))
                {
                    var interfaceTypes = serviceType.GetInterfaces();

                    foreach (var interfaceType in interfaceTypes)
                    {
                        services.AddSingleton(interfaceType, serviceType);
                    }
                }

                foreach (Type serviceType in serviceList.Where(t => typeof(ITransient).IsAssignableFrom(t)))
                {
                    var interfaceTypes = serviceType.GetInterfaces();

                    foreach (var interfaceType in interfaceTypes)
                    {
                        services.AddTransient(interfaceType, serviceType);
                    }
                }

            }

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


        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
            typeAdapterConfig.Scan(Assemblies);

            services.AddSingleton(typeAdapterConfig);
            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }

        public static IServiceCollection ConfigureValidator(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblies(Assemblies);

            return services;
        }


        public static IServiceCollection AddViewEngine(this IServiceCollection services, IWebHostEnvironment env, Action<FluidViewEngineOptions> setupAction = null)
        {
            services.AddFluid();

            return services;
        }

        public static IServiceCollection AddEndpoints(this IServiceCollection services)
        {
            var assembly = typeof(IAssembly).Assembly;

            ServiceDescriptor[] serviceDescriptors = assembly
                .DefinedTypes
                .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                               type.IsAssignableTo(typeof(IEndpoint)))
                .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
                .ToArray();

            services.TryAddEnumerable(serviceDescriptors);

            return services;
        }

        public static IApplicationBuilder UseGlobalExceptionHandler(this WebApplication app)
        {
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

            return app;
        }

        public static IApplicationBuilder MapEndpoints(this WebApplication app)
        {
            var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>().OrderByDescending(x => x.Priority);
            foreach (var route in endpoints)
            {
                var routeGroup = app.MapGroup($"/{route.Prefix}").WithTags(route.Tags);
                route.Map(routeGroup);

            }



            return app;
        }

        public static IApplicationBuilder UseBlogFiles(this WebApplication app, WebApplicationBuilder builder)
        {
            var uploadsPath = Path.Combine(builder.Environment.ContentRootPath, "files", "uploads");
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "files", "uploads")),
                RequestPath = "/uploads",
            });

            var themesPath = Path.Combine(builder.Environment.ContentRootPath, "files", "themes");
            if (!Directory.Exists(themesPath))
            {
                Directory.CreateDirectory(themesPath);
            }
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "files", "themes")),
                RequestPath = "/themes",
            });

            return app;
        }

    }
}
