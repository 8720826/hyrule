namespace Yes.Blog.Endpoints.Blogs
{
	public static class NameValueCollectionExtensions
	{
		public static NameValueCollection ToNameValueCollection(this IQueryCollection queryCollection)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			foreach (var (key, value) in queryCollection)
			{
				nameValueCollection.Add(key.ToUpper(), value);
			}

			return nameValueCollection;
		}
	}
}
