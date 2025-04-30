namespace Yes.Blog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.RegisterBlogModule(builder);
            builder.Services.RegisterApplicationModule();
            builder.Services.RegisterDataModule();

            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseBlogFiles(builder);

            app.UseRouting();

            app.UseGlobalExceptionHandler();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();
			app.MapEndpoints();

            app.Run();
        }
    }
}
