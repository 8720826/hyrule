namespace Yes.Domain.Categories
{
	[Table("Category")]
    public class CategoryEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public string CoverUrl { get; set; } = string.Empty;

        public int Sort { get; set; }

        public string Description { get; set; } = string.Empty;


		public static CategoryEntity Create(string name, string slug, string coverUrl, string description, int sort)
		{

			return new CategoryEntity()
			{
				CoverUrl = coverUrl ?? "",
				Slug = slug ?? "",
				Sort = sort,
				Description = description ?? "",
				Name = name ?? "",
			};
		}

		public void Update(string name, string slug, string coverUrl, string description, int sort)
		{
			CoverUrl = coverUrl ?? "";
			Slug = slug ?? "";
			Sort = sort;
			Description = description ?? "";
			Name = name ?? "";
		}
	}
}
