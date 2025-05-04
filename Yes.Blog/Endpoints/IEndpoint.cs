namespace Yes.Blog.Endpoints
{
	public interface IEndpoint
	{
		int Priority { get; }


		string Prefix { get; }

		void Map(IEndpointRouteBuilder app);

        string[] Tags { get; }

    }

}
