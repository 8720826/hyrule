namespace Yes.Application.Blogs
{
    public class BlogService : IBlogService
    {

        private readonly IMemoryCache _cache;
        private readonly BlogDbContext _db;
        private readonly BlogSettings _settings;
        public BlogService(BlogDbContext db, IMemoryCache cache, IOptionsMonitor<BlogSettings> opt)
        {
            _db = db;
            _cache = cache;
            _settings = opt.CurrentValue;
        }



        public async Task<List<CategoryModel>> GetAllCategories()
        {
            var result = await _cache.GetOrCreateAsync("AllCategories", async x =>
            {
                x.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);

                var categories = await _db.Categories.OrderBy(x => x.Sort).Select(x => x.CategoryEntityToModel()).ToListAsync() ?? [];
                categories.ForEach(x =>
                {
                    x.Link = _settings.CategoryRoute.ToRoute(x);
                });

                return categories;
            });

            return result ?? [];
        }


        public async Task<List<TagModel>> GetTags()
        {
            var result = await _cache.GetOrCreateAsync("Tags", async x =>
            {
                x.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);

                var tags = await _db.Tags.Select(x => x.TagEntityToModel()).ToListAsync() ?? [];
                tags.ForEach(x =>
                {
                    x.Link = _settings.TagRoute.ToRoute(x);
                });

                return tags;
            });

            return result ?? [];
        }

        public async Task<List<ArchiveModel>> GetArchives()
        {
            var result = await _cache.GetOrCreateAsync("Archives", async x =>
            {
                x.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);

                var archives = await _db.Articles.GroupBy(a => new { a.PublishDate.Year, a.PublishDate.Month })
                    .Select(g => new ArchiveModel
                    {
                        Year = g.Key.Year,
                        Month = g.Key.Month
                    }).ToListAsync() ?? [];

                archives.ForEach(x =>
                {
                    x.Link = _settings.ArchiveRoute.ToRoute(x);
                });

                return archives;
            });

            return result ?? [];
        }
        

        public async Task<PagedList<ArticleModel>> GetCategoryArticles(int categoryId, int pageIndex = 1, int pageSize = 10)
        {
            var query = from article in _db.Articles.AsNoTracking().Where(x => x.Type == ArticleTypeEnum.Article && x.Status == ArticleStatusEnum.Normal)
                        join categoryTemp in _db.Categories.AsNoTracking() on article.CategoryId equals categoryTemp.Id into tempCategory
                        from category in tempCategory.DefaultIfEmpty()
                        join userTemp in _db.Users.AsNoTracking() on article.UserId equals userTemp.Id into tempUser
                        from user in tempUser.DefaultIfEmpty()
                        orderby article.Id descending
                        where article.CategoryId == categoryId
                        select new ArticleModel
                        {
                            Id = article.Id,
                            CanComment = article.CanComment,
                            CommentCount = article.CommentCount,
                            CategoryId = article.CategoryId,
                            ModifyDate = article.ModifyDate.ToSafeTime(),
                            PublishDate = article.PublishDate.ToSafeTime(),
                            CreateDate = article.CreateDate.ToSafeTime(),
                            Content = article.Content,
                            CoverUrl = article.CoverUrl,
                            Slug = article.Slug,
                            ReadCount = article.ReadCount,
                            Title = article.Title,
                            Summary = article.Summary,
                            UserId = article.UserId,
                            Category = new CategoryModel
                            {
                                CoverUrl = category.CoverUrl,
                                Description = category.Description,
                                Id = category.Id,
                                Name = category.Name,
                                Slug = category.Slug
                            }
                        };
            var pagedRequest = new BlogPagedRequest { PageIndex = pageIndex, PageSize = pageSize };

            var articles = await query.ToPagedListAsync(pagedRequest);

            var articleIds = articles.Items.Select(x => x.Id).ToList();

            var tags = await (from articleTag in _db.ArticleTags.AsNoTracking()
                              join tag in _db.Tags.AsNoTracking() on articleTag.TagId equals tag.Id
                              where articleIds.Contains(articleTag.ArticleId)
                              select new TagModel
                              {
                                  Id = tag.Id,
                                  CreateDate = tag.CreateDate,
                                  Name = tag.Name,
                                  Slug = tag.Slug,
                                  ArticleId = articleTag.ArticleId
                              }).ToListAsync();

            articles.Items.ForEach(x =>
            {
                x.Link = _settings.ArticleRoute.ToRoute(x.Category, x);
                x.Category.Link = _settings.CategoryRoute.ToRoute(x.Category);
                x.Tags = tags.Where(y => y.ArticleId == x.Id).ToList();
                x.Tags.ForEach(z =>
                {
                    z.Link = _settings.TagRoute.ToRoute(z);
                });
            });

            return articles;

        }


        public async Task<PagedList<ArticleModel>> GetTagArticles(int tagId, int pageIndex = 1, int pageSize = 10)
        {
            var query = from article in _db.Articles.AsNoTracking().Where(x => x.Type == ArticleTypeEnum.Article && x.Status == ArticleStatusEnum.Normal)
                        join articleTag in _db.ArticleTags.AsNoTracking() on article.Id equals articleTag.ArticleId
                        join categoryTemp in _db.Categories.AsNoTracking() on article.CategoryId equals categoryTemp.Id into tempCategory
                        from category in tempCategory.DefaultIfEmpty()
                        join userTemp in _db.Users.AsNoTracking() on article.UserId equals userTemp.Id into tempUser
                        from user in tempUser.DefaultIfEmpty()
                        orderby article.Id descending
                        where articleTag.TagId == tagId
                        select new ArticleModel
                        {
                            Id = article.Id,
                            CanComment = article.CanComment,
                            CommentCount = article.CommentCount,
                            CategoryId = article.CategoryId,
                            ModifyDate = article.ModifyDate.ToSafeTime(),
                            PublishDate = article.PublishDate.ToSafeTime(),
                            CreateDate = article.CreateDate.ToSafeTime(),
                            Content = article.Content,
                            CoverUrl = article.CoverUrl,
                            Slug = article.Slug,
                            ReadCount = article.ReadCount,
                            Title = article.Title,
                            Summary = article.Summary,
                            UserId = article.UserId,
                            Category = new CategoryModel
                            {
                                CoverUrl = category.CoverUrl,
                                Description = category.Description,
                                Id = category.Id,
                                Name = category.Name,
                                Slug = category.Slug
                            }
                        };
            var pagedRequest = new BlogPagedRequest { PageIndex = pageIndex, PageSize = pageSize };

            var articles = await query.ToPagedListAsync(pagedRequest);

            var articleIds = articles.Items.Select(x => x.Id).ToList();

            var tags = await (from articleTag in _db.ArticleTags.AsNoTracking()
                              join tag in _db.Tags.AsNoTracking() on articleTag.TagId equals tag.Id
                              where articleIds.Contains(articleTag.ArticleId)
                              select new TagModel
                              {
                                  Id = tag.Id,
                                  CreateDate = tag.CreateDate,
                                  Name = tag.Name,
                                  Slug = tag.Slug, ArticleId = articleTag.ArticleId
                              }).ToListAsync();

            articles.Items.ForEach(x =>
            {
                x.Link = _settings.ArticleRoute.ToRoute(x.Category, x);
                x.Category.Link = _settings.CategoryRoute.ToRoute(x.Category);
                x.Tags = tags.Where(y => y.ArticleId == x.Id).ToList();
                x.Tags.ForEach(z =>
                {
                    z.Link = _settings.TagRoute.ToRoute(z);
                });
            });


            return articles;
        }



        public async Task<PagedList<ArticleModel>> GetArchiveArticles(ArchiveModel archiveModel, int pageIndex = 1, int pageSize = 10)
        {
            var query = from article in _db.Articles.AsNoTracking().Where(x => x.Type == ArticleTypeEnum.Article && x.Status == ArticleStatusEnum.Normal)
                        join categoryTemp in _db.Categories.AsNoTracking() on article.CategoryId equals categoryTemp.Id into tempCategory
                        from category in tempCategory.DefaultIfEmpty()
                        join userTemp in _db.Users.AsNoTracking() on article.UserId equals userTemp.Id into tempUser
                        from user in tempUser.DefaultIfEmpty()
                        orderby article.Id descending
                        where article.PublishDate.Year == archiveModel.Year && article.PublishDate.Month == archiveModel.Month
                        select new ArticleModel
                        {
                            Id = article.Id,
                            CanComment = article.CanComment,
                            CommentCount = article.CommentCount,
                            CategoryId = article.CategoryId,
                            ModifyDate = article.ModifyDate.ToSafeTime(),
                            PublishDate = article.PublishDate.ToSafeTime(),
                            CreateDate = article.CreateDate.ToSafeTime(),
                            Content = article.Content,
                            CoverUrl = article.CoverUrl,
                            Slug = article.Slug,
                            ReadCount = article.ReadCount,
                            Title = article.Title,
                            Summary = article.Summary,
                            UserId = article.UserId,
                            Category = new CategoryModel
                            {
                                CoverUrl = category.CoverUrl,
                                Description = category.Description,
                                Id = category.Id,
                                Name = category.Name,
                                Slug = category.Slug
                            }
                        };
            var pagedRequest = new BlogPagedRequest { PageIndex = pageIndex, PageSize = pageSize };

            var articles = await query.ToPagedListAsync(pagedRequest);

            var articleIds = articles.Items.Select(x => x.Id).ToList();

            var tags = await (from articleTag in _db.ArticleTags.AsNoTracking()
                              join tag in _db.Tags.AsNoTracking() on articleTag.TagId equals tag.Id
                              where articleIds.Contains(articleTag.ArticleId)
                              select new TagModel
                              {
                                  Id = tag.Id,
                                  CreateDate = tag.CreateDate,
                                  Name = tag.Name,
                                  Slug = tag.Slug,
                                  ArticleId = articleTag.ArticleId
                              }).ToListAsync();

            articles.Items.ForEach(x =>
            {
                x.Link = _settings.ArticleRoute.ToRoute(x.Category, x);
                x.Category.Link = _settings.CategoryRoute.ToRoute(x.Category);
                x.Tags = tags.Where(y => y.ArticleId == x.Id).ToList();
                x.Tags.ForEach(z =>
                {
                    z.Link = _settings.TagRoute.ToRoute(z);
                });
            });


            return articles;
        }

        public async Task<PagedList<ArticleModel>> GetKeywordArticles(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var query = from article in _db.Articles.AsNoTracking().Where(x => x.Type == ArticleTypeEnum.Article && x.Status == ArticleStatusEnum.Normal)
                        .Where(entity => EF.Functions.Like(EF.Functions.Collate(entity.Title, "Latin1_General_CI_AI"), $"%{keyword}%"))
                        join categoryTemp in _db.Categories.AsNoTracking() on article.CategoryId equals categoryTemp.Id into tempCategory
                        from category in tempCategory.DefaultIfEmpty()
                        join userTemp in _db.Users.AsNoTracking() on article.UserId equals userTemp.Id into tempUser
                        from user in tempUser.DefaultIfEmpty()
                        orderby article.Id descending
                        select new ArticleModel
                        {
                            Id = article.Id,
                            CanComment = article.CanComment,
                            CommentCount = article.CommentCount,
                            CategoryId = article.CategoryId,
                            ModifyDate = article.ModifyDate.ToSafeTime(),
                            PublishDate = article.PublishDate.ToSafeTime(),
                            CreateDate = article.CreateDate.ToSafeTime(),
                            Content = article.Content,
                            CoverUrl = article.CoverUrl,
                            Slug = article.Slug,
                            ReadCount = article.ReadCount,
                            Title = article.Title,
                            Summary = article.Summary,
                            UserId = article.UserId,
                            Category = new CategoryModel
                            {
                                CoverUrl = category.CoverUrl,
                                Description = category.Description,
                                Id = category.Id,
                                Name = category.Name,
                                Slug = category.Slug
                            }
                        };

            var pagedRequest = new BlogPagedRequest { PageIndex = pageIndex, PageSize = pageSize };

            var articles = await query.ToPagedListAsync(pagedRequest);

            var articleIds = articles.Items.Select(x => x.Id).ToList();

            var tags = await (from articleTag in _db.ArticleTags.AsNoTracking()
                              join tag in _db.Tags.AsNoTracking() on articleTag.TagId equals tag.Id
                              where articleIds.Contains(articleTag.ArticleId)
                              select new TagModel
                              {
                                  Id = tag.Id,
                                  CreateDate = tag.CreateDate,
                                  Name = tag.Name,
                                  Slug = tag.Slug,
                                  ArticleId = articleTag.ArticleId
                              }).ToListAsync();

            articles.Items.ForEach(x =>
            {
                x.Link = _settings.ArticleRoute.ToRoute(x.Category, x);
                x.Category.Link = _settings.CategoryRoute.ToRoute(x.Category);
                x.Tags = tags.Where(y => y.ArticleId == x.Id).ToList();
                x.Tags.ForEach(z =>
                {
                    z.Link = _settings.TagRoute.ToRoute(z);
                });
            });

            return articles;

        }

        public async Task<PagedList<ArticleModel>> GetIndexArticles(int pageIndex = 1, int pageSize = 10)
        {
            var query = from article in _db.Articles.AsNoTracking().Where(x => x.Type == ArticleTypeEnum.Article && x.Status == ArticleStatusEnum.Normal)
                        join categoryTemp in _db.Categories.AsNoTracking() on article.CategoryId equals categoryTemp.Id into tempCategory
                        from category in tempCategory.DefaultIfEmpty()
                        join userTemp in _db.Users.AsNoTracking() on article.UserId equals userTemp.Id into tempUser
                        from user in tempUser.DefaultIfEmpty()
                        orderby article.Id descending
                        select new ArticleModel
                        {
                            Id = article.Id,
                            CanComment = article.CanComment,
                            CommentCount = article.CommentCount,
                            CategoryId = article.CategoryId,
                            ModifyDate = article.ModifyDate.ToSafeTime(),
                            PublishDate = article.PublishDate.ToSafeTime(),
                            CreateDate = article.CreateDate.ToSafeTime(),
                            Content = article.Content,
                            CoverUrl = article.CoverUrl,
                            Slug = article.Slug,
                            ReadCount = article.ReadCount,
                            Title = article.Title,
                            Summary = article.Summary,
                            UserId = article.UserId,
                            Category = new CategoryModel
                            {
                                CoverUrl = category.CoverUrl,
                                Description = category.Description,
                                Id = category.Id,
                                Name = category.Name,
                                Slug = category.Slug
                            }
                        };

            var pagedRequest = new BlogPagedRequest { PageIndex = pageIndex, PageSize = pageSize };

            var articles = await query.ToPagedListAsync(pagedRequest);

            var articleIds = articles.Items.Select(x => x.Id).ToList();

            var tags = await (from articleTag in _db.ArticleTags.AsNoTracking()
                              join tag in _db.Tags.AsNoTracking() on articleTag.TagId equals tag.Id
                              where articleIds.Contains(articleTag.ArticleId)
                              select new TagModel
                              {
                                  Id = tag.Id,
                                  CreateDate = tag.CreateDate,
                                  Name = tag.Name,
                                  Slug = tag.Slug,
                                  ArticleId = articleTag.ArticleId
                              }).ToListAsync();

            articles.Items.ForEach(x =>
            {
                x.Link = _settings.ArticleRoute.ToRoute(x.Category, x);
                x.Category.Link = _settings.CategoryRoute.ToRoute(x.Category);
                x.Tags = tags.Where(y => y.ArticleId == x.Id).ToList();
                x.Tags.ForEach(z =>
                {
                    z.Link = _settings.TagRoute.ToRoute(z);
                });
            });


            return articles;
        }

        public async Task<List<SinglePageModel>> GetSinglePages(int top = 10)
        {
            var result = await _cache.GetOrCreateAsync($"SinglePages_{top}", async x =>
            {
                x.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);

                var articles = _db.Articles.Where(x => x.Type == ArticleTypeEnum.Page).OrderByDescending(x => x.Sort).Take(top).Select(x => x.ArticleEntityToSinglePage()).ToList();

                articles.ForEach(x =>
                {
                    x.Link = _settings.PageRoute.ToRoute(x);
                });

                return articles;
            });
            return result ?? [];
        }

        public async Task<ArticleModel> GetArticle(int id, string slug)
        {
            var articleEntity = await _db.Articles.FindAsync(id);
            if (articleEntity == null)
            {
                articleEntity = await _db.Articles.FirstOrDefaultAsync(x => x.Slug == slug);
            }
            if (articleEntity == null)
            {
                throw new PageNotFoundException();
            }


            var categories = await GetAllCategories();
            var category = categories.FirstOrDefault(y => y.Id == articleEntity.CategoryId);

            var tags = await (from articleTag in _db.ArticleTags.AsNoTracking()
                              join tag in _db.Tags.AsNoTracking() on articleTag.TagId equals tag.Id
                              where articleTag.ArticleId == articleEntity.Id
                              select new TagModel
                              {
                                  Id = tag.Id,
                                  CreateDate = tag.CreateDate,
                                  Name = tag.Name,
                                  Slug = tag.Slug,
                                  ArticleId = articleTag.ArticleId
                              }).ToListAsync();

            var article = articleEntity?.ArticleEntityToModel(category, tags);
            article.Tags.ForEach(z =>
            {
                z.Link = _settings.TagRoute.ToRoute(z);
            });

            article.Link = _settings.ArticleRoute.ToRoute(category, article);

            return article.ToHtml();

        }


        public async Task<SinglePageModel> GetSinglePage(int id)
        {
            var article = (await _db.Articles.FindAsync(id))?.ArticleEntityToSinglePage();
            if (article == null)
            {
                throw new PageNotFoundException();
            }
            article.Link = _settings.PageRoute.ToRoute(article);

            return article.ToHtml();

        }

        public async Task<SinglePageModel> GetSinglePage(string slug)
        {
            var article = (await _db.Articles.FirstOrDefaultAsync(x => x.Slug == slug))?.ArticleEntityToSinglePage();
            if (article == null)
            {
                throw new PageNotFoundException();
            }
            article.Link = _settings.PageRoute.ToRoute(article);

            return article.ToHtml();

        }


        public async Task<TagModel> GetTag(int id, string slug)
        {
            var tagEntity = await _db.Tags.FindAsync(id);
            if (tagEntity == null)
            {
                tagEntity = await _db.Tags.FirstOrDefaultAsync(x => x.Slug == slug);
            }
            if (tagEntity == null)
            {
                tagEntity = await _db.Tags.FirstOrDefaultAsync(x => x.Name == slug);
            }
            if (tagEntity == null)
            {
                throw new PageNotFoundException();
            }

            var tag = tagEntity?.TagEntityToModel();

            tag.Link = _settings.TagRoute.ToRoute(tag);

            return tag;
        }


        public async Task<PagedList<CommentModel>> GetArticleComments(int articleId,string ip, int pageIndex = 1, int pageSize = 10)
        {
            var yestoday = DateTime.Now.AddDays(-1);
            var query = from comment in _db.Comments.AsNoTracking().Where(x => x.ArticleId == articleId && (x.Status == CommentStatusEnum.审核通过 || x.IP == ip && x.CreateDate > yestoday))
                        orderby comment.Id descending
                        select new CommentModel
                        {
                            Id = comment.Id,
                            CreateDate = comment.CreateDate.ToSafeTime(),
                            Content = comment.Content,
                            UserId = comment.UserId,
                            Email = comment.Email,
                            IP = comment.IP,
                            Url = comment.Url,
                            NickName = comment.NickName
                        };
            var pagedRequest = new BlogPagedRequest { PageIndex = pageIndex, PageSize = pageSize };

            var comments = await query.ToPagedListAsync(pagedRequest);

            return comments;
        }
    }
}
