namespace Yes.Blog.Endpoints.Migrators
{
	public class MigratorEndpointScheme
	{
		public int Priority { get; set; } = 99;
		public string SchemeName { get; set; } = "Migrator";
	}
}
