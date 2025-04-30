namespace Yes.Domain.Blogs
{
    public static class EntityExtensions
    {
        public static ArticleModel ArticleEntityToModel(this ArticleEntity x, CategoryModel category, List<TagModel> tags)
        {
            var article = new ArticleModel
            {
                CoverUrl = x.CoverUrl,
                Id = x.Id,
                Title = x.Title,
                CanComment = x.CanComment,
                CategoryId = x.CategoryId,
                CommentCount = x.CommentCount,
                Content = x.Content,
                ModifyDate = x.ModifyDate.ToSafeTime(),
                PublishDate = x.PublishDate.ToSafeTime(),
                CreateDate = x.CreateDate.ToSafeTime(),
                ReadCount = x.ReadCount,
                Slug = x.Slug,
                Summary = x.Summary,
                Tags = tags,
                Category = category ?? new CategoryModel
                {
                    Link = "/",
                    Name = "[未分类]",
                    Id = 0
                }
            };

            return article;
        }

        public static SinglePageModel ArticleEntityToSinglePage(this ArticleEntity x)
        {
            return new SinglePageModel
            {
                CoverUrl = x.CoverUrl,
                Id = x.Id,
                Title = x.Title,
                CanComment = x.CanComment,
                CommentCount = x.CommentCount,
                Content = x.Content,
                ModifyDate = x.ModifyDate.ToSafeTime(),
                PublishDate = x.PublishDate.ToSafeTime(),
                CreateDate = x.CreateDate.ToSafeTime(),
                ReadCount = x.ReadCount,
                Slug = x.Slug,
                Sort = x.Sort,
                Summary = x.Summary,
                Tag = x.Tag,
            };
        }

        public static CategoryModel CategoryEntityToModel(this CategoryEntity x)
        {
            return new CategoryModel
            {
                CoverUrl = x.CoverUrl,
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
            };
        }


        public static ArticleModel ToHtml(this ArticleModel model)
        {
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            model.Content = Markdown.ToHtml(model.Content, pipeline);

            return model;
        }

        public static SinglePageModel ToHtml(this SinglePageModel model)
        {
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            model.Content = Markdown.ToHtml(model.Content, pipeline);

            return model;
        }

        public static TagModel TagEntityToModel(this TagEntity x)
        {
            return new TagModel
            {
                CreateDate = x.CreateDate.ToSafeTime(),
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
            };
        }
    }
}
