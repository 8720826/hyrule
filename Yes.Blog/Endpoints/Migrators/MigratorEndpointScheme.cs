namespace Yes.Blog.Endpoints.Migrators
{
	public class MigratorEndpointScheme
	{
		public int Priority { get; set; } = 99;

		public string Prefix { get; set; } = "migrator";

        public string[] Tags { get; set; } = new[] { "Migrators" };
    }
}
