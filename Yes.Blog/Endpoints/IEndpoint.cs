namespace Yes.Blog.Endpoints
{
	public interface IEndpoint
	{
		int Priority { get; }


		string SchemeName { get; }

		void Map(IEndpointRouteBuilder app);
	}

}
