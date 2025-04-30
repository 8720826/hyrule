



namespace Yes.Domain.Articles
{
    [Table("Article")]
    public class ArticleEntity
    {

        [Key]
        public int Id { get; private set; }


        /// <summary>
        /// 作者ID
        /// </summary>
        [ForeignKey(nameof(Author))]
        public int UserId { get; private set; }

        public UserEntity? Author { get; private set; }

		/// <summary>
		/// 分类ID
		/// </summary>
		[ForeignKey(nameof(Category))]
		public int CategoryId { get; private set; }


        public CategoryEntity?  Category { get; private set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; private set; } = string.Empty;


        public string Slug { get; private set; } = string.Empty;

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; private set; } = string.Empty;

        /// <summary>
        /// 封面图
        /// </summary>
        public string CoverUrl { get; private set; } = string.Empty;

        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { get; private set; } = string.Empty;

        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; private set; } = string.Empty;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; private set; }

        /// <summary>
        /// 删除时间
        /// </summary>
		public DateTime DeleteDate { get; private set; }

		/// <summary>
		/// 发布时间
		/// </summary>
		public DateTime PublishDate { get; private set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyDate { get; private set; }

        /// <summary>
        /// 是否允许评论
        /// </summary>
        public bool CanComment { get; private set; }

        /// <summary>
        /// 评论数
        /// </summary>
        public int CommentCount { get; private set; }

        /// <summary>
        /// 密码，不为空时需要输入密码才可查看
        /// </summary>
        public string Password { get; private set; } = string.Empty;

        /// <summary>
        /// 排序值
        /// </summary>
        public int Sort { get; private set; }

        /// <summary>
        /// 文章类型
        /// </summary>
        public ArticleTypeEnum Type { get; private set; }

        /// <summary>
        /// 文章状态
        /// </summary>
        public ArticleStatusEnum Status { get; private set; }

        /// <summary>
        /// 阅读数
        /// </summary>
        public int ReadCount { get; private set; }

        public static ArticleEntity CreatePage(string title, string content, string summary, string slug, string coverUrl,int sort, ValueObject<UserEntity> authorObject)
        {

            var author = authorObject.Value;
            var authorId = authorObject.Id;

            var page = new ArticleEntity()
            {
                Author = author ?? throw new UserNotExistsException(authorId),
                UserId = authorId,
                Content = content ?? "",
                Status = ArticleStatusEnum.Normal,
                Summary = summary ?? "",
                Title = title ?? "",
                CategoryId = 0,
                Tag = "",
                CoverUrl = coverUrl ?? "",
                Slug = slug ?? "",
                Type = ArticleTypeEnum.Page,
                CanComment = true,
                CommentCount = 0,
                ReadCount = 0,
                Sort = sort,
                Password = "",
                CreateDate = DateTime.Now,
                PublishDate = DateTime.Now, 
                ModifyDate= DateTime.Now
            };

            page.CheckSummary();

            return page;
        }


        public static ArticleEntity CreateDraft(string title, string content, string summary, List<string> tags, string slug, string coverUrl, ValueObject<UserEntity> authorObject, ValueObject<CategoryEntity> categoryObject)
        {
            var category = categoryObject.Value;
            var categoryId = categoryObject.Id;

            var author = authorObject.Value;
            var authorId = authorObject.Id;

            return new ArticleEntity()
            {
                Author = author ?? throw new UserNotExistsException(authorId),
                UserId = authorId,
                Content = content ?? "",
                Status = ArticleStatusEnum.Draft,
                Summary = summary ?? "",
                Title = title ?? "",
                Category = category,
                CategoryId = categoryId,
                Tag = string.Join(",", tags.Distinct()),
                CoverUrl = coverUrl ?? "",
                Slug = slug ?? "",
                Type = ArticleTypeEnum.Article,
                CanComment = true,
                CommentCount = 0,
                ReadCount = 0,
                Sort = 0,
                Password = "",
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now
            };
        }

        public static ArticleEntity CreateArticle(string title, string content, string summary, List<string> tags, string slug, string coverUrl, ValueObject<UserEntity> authorObject, ValueObject<CategoryEntity> categoryObject)
        {
            var article = CreateDraft(title, content, summary, tags, slug, coverUrl, authorObject, categoryObject);

            article.CheckCategory();

            article.Publish();

            article.CheckSummary();

            return article;
        }

        public void UpdateDraft(string title, string content, string summary, List<string> tags, string slug, string coverUrl, ValueObject<UserEntity> authorObject, ValueObject<CategoryEntity> categoryObject)
        {
            Update(title, content, summary, tags, slug, coverUrl, authorObject, categoryObject);
            Draft();
        }

        public void UpdateArticle(string title, string content, string summary, List<string> tags, string slug, string coverUrl, ValueObject<UserEntity> authorObject, ValueObject<CategoryEntity> categoryObject)
        {
            Update(title, content, summary, tags, slug, coverUrl, authorObject, categoryObject);

            CheckCategory();

            Publish();

            CheckSummary();
        }

        public void UpdatePage(string title, string content, string summary, string slug, string coverUrl,int sort, ValueObject<UserEntity> authorObject)
        {
            var author = authorObject.Value;
            var authorId = authorObject.Id;

            Title = title ?? "";
            Content = content ?? "";
            Summary = summary ?? "";
            Tag = "";
            Slug = slug ?? "";
            CoverUrl = coverUrl ?? "";
            Author = author ?? throw new UserNotExistsException(authorId);
            UserId = authorId;
            CategoryId = 0;
            Sort = sort;

            CheckSummary();
        }

        public bool IsPage()
        {
            return Type == ArticleTypeEnum.Page;
        }

        public bool IsArticle()
        {
            return Type == ArticleTypeEnum.Article;

        }

        public bool IsSlugEmpty()
        {
            return string.IsNullOrEmpty(Slug);
        }


        public bool IsSlugDifferent(string slug)
        {
            return !string.Equals(Slug, slug);

		}


		public void UpdateSlug(string slug)
        {
            if (string.IsNullOrEmpty(Slug))
            {
                Slug = slug;
            }
        }

        public void Delete()
        {
            Status = ArticleStatusEnum.Deleted;
            DeleteDate = DateTime.Now;
        }

        private void Update(string title, string content, string summary, List<string> tags, string slug, string coverUrl, ValueObject<UserEntity> authorObject, ValueObject<CategoryEntity> categoryObject)
        {
            var category = categoryObject.Value;
            var categoryId = categoryObject.Id;

            var author = authorObject.Value;
            var authorId = authorObject.Id;

            Title = title ?? "";
            Content = content ?? "";
            Summary = summary ?? "";
            Tag = string.Join(",", tags.Distinct());
            Slug = slug ?? "";
            CoverUrl = coverUrl ?? "";
            Author = author ?? throw new UserNotExistsException(authorId);
            UserId = authorId;
            Category = category;
            CategoryId = categoryId;
        }

        private void CheckCategory()
        {
            if (Category == null)
            {
                throw new CategoryNotExistsException();
            }
        }

        private void Publish()
        {
            Status = ArticleStatusEnum.Normal;
            PublishDate = DateTime.Now;
        }

        private void Draft()
        {
            Status = ArticleStatusEnum.Draft;
            PublishDate = DateTime.Now;
        }

        private void CheckSummary()
        {
            if (Status == ArticleStatusEnum.Normal && string.IsNullOrEmpty(Summary))
            {
                Summary = Content.ToSummary();
            }
        }
    }
}
