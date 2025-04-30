namespace Yes.Domain.Blogs
{
    public class SinglePageModel
    {
        public int Id { get; set; }


        /// <summary>
        /// 作者ID
        /// </summary>
        public int UserId { get; set; }


        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; } = string.Empty;


        public string Slug { get; set; } = string.Empty;

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 封面图
        /// </summary>
        [StringLength(200)]
        public string CoverUrl { get; set; } = string.Empty;

        /// <summary>
        /// 摘要
        /// </summary>
        [StringLength(200)]
        public string Summary { get; set; } = string.Empty;

        /// <summary>
        /// 标签
        /// </summary>
        [StringLength(50)]
        public string Tag { get; set; } = string.Empty;

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime PublishDate { get; set; }


        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// 是否允许评论
        /// </summary>
        public bool CanComment { get; set; }

        /// <summary>
        /// 评论数
        /// </summary>
        public int CommentCount { get; set; }


        /// <summary>
        /// 排序值
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 文章类型
        /// </summary>
        public ArticleTypeEnum Type { get; set; }

        /// <summary>
        /// 文章状态
        /// </summary>
        public ArticleStatusEnum Status { get; set; }

        /// <summary>
        /// 阅读数
        /// </summary>
        public int ReadCount { get; set; }



        public string Link { get; set; } = string.Empty;
    }
}
