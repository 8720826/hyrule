
namespace Yes.Application.Admins.Configurations
{
	public class ConfigurationService(IWebHostEnvironment env) : IConfigurationService
	{
		private readonly IWebHostEnvironment _env = env;

		public async Task SaveConfiguration(BlogSettings settings)
		{
            var filePath = Path.Combine(_env.ContentRootPath, "files", "config", "appsettings.blog.json");
            JObject jsonObject = JObject.FromObject(settings);

			using (var writer = new StreamWriter(filePath))
			using (JsonTextWriter jsonwriter = new JsonTextWriter(writer))
			{
				jsonwriter.Formatting = Formatting.Indented;
			  await	jsonObject.WriteToAsync(jsonwriter);
			}
		}
	}
}
