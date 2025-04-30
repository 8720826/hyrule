namespace Yes.Infrastructure.Migrator.Migrations
{
    [Migration(0)]
    public class InitTables : Migration
    {
        public override void Up()
        {

            if (!Schema.Table("Article").Exists())
            {
                Create.Table("Article")
                 .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                 .WithColumn("UserId").AsInt32().NotNullable().Indexed()
                 .WithColumn("CategoryId").AsInt32().NotNullable().Indexed()
                 .WithColumn("Title").AsString(256).NotNullable()
                 .WithColumn("Content").AsString(int.MaxValue).NotNullable()
                 .WithColumn("CoverUrl").AsString(256).NotNullable()
                 .WithColumn("Summary").AsString(512).NotNullable()
                 .WithColumn("Tag").AsString(64).NotNullable()
                 .WithColumn("CanComment").AsBoolean().NotNullable()
                 .WithColumn("PublishDate").AsDateTime2().NotNullable()
                 .WithColumn("ModifyDate").AsDateTime2().NotNullable()
                 .WithColumn("CommentCount").AsInt32().NotNullable()
                 .WithColumn("Password").AsString(64).NotNullable()
                 .WithColumn("Sort").AsInt32().NotNullable()
                 .WithColumn("Type").AsInt32().NotNullable()
                 .WithColumn("Status").AsInt32().NotNullable()
                 .WithColumn("ReadCount").AsInt32().NotNullable()
                 .WithColumn("Slug").AsString(256).NotNullable().WithDefaultValue("")
                 .WithColumn("CreateDate").AsDateTime2().NotNullable()
                 .WithColumn("DeleteDate").AsDateTime2().NotNullable();

                Insert.IntoTable("Article").Row(new { UserId = 1, CategoryId = 1, Title = "欢迎使用 Hyrule", Content = "如果您看到这篇文章,表示您的 blog 已经安装成功.", CoverUrl = "", Summary = "如果您看到这篇文章,表示您的 blog 已经安装成功.", Tag = "", CanComment = true, PublishDate = DateTime.Now, ModifyDate = DateTime.Now, CommentCount = 0, Password = "", Sort = 1, Type = (int)ArticleTypeEnum.Article, Status = (int)ArticleStatusEnum.Normal, ReadCount = 0, Slug = "start", CreateDate = DateTime.Now, DeleteDate = DateTime.Now });

                Insert.IntoTable("Article").Row(new { UserId = 1, CategoryId = 0, Title = "关于", Content = "本页面由 Hyrule 创建, 这只是个测试页面.", CoverUrl = "", Summary = "", Tag = "", CanComment = true, PublishDate = DateTime.Now, ModifyDate = DateTime.Now, CommentCount = 0, Password = "", Sort = 1, Type = (int)ArticleTypeEnum.Page, Status = (int)ArticleStatusEnum.Normal, ReadCount = 0, Slug = "start", CreateDate = DateTime.Now, DeleteDate = DateTime.Now });
            }

            if (!Schema.Table("ArticleTag").Exists())
            {
                Create.Table("ArticleTag")
                 .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                 .WithColumn("ArticleId").AsInt32().NotNullable().WithDefaultValue(0).Indexed()
                 .WithColumn("TagId").AsInt32().NotNullable().WithDefaultValue(0).Indexed();
            }

            if (!Schema.Table("Category").Exists())
            {
                Create.Table("Category")
                 .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                 .WithColumn("Name").AsString(64).NotNullable()
                 .WithColumn("CoverUrl").AsString(64).NotNullable()
                 .WithColumn("Sort").AsInt32().NotNullable()
                 .WithColumn("Slug").AsString(256).NotNullable().WithDefaultValue("")
                 .WithColumn("Description").AsString(512).NotNullable();

                Insert.IntoTable("Category").Row(new { Name = "默认分类", CoverUrl = "", Sort = 1, Slug = "default", Description = "这是一个默认分类" });

            }



            if (!Schema.Table("Tag").Exists())
            {
                Create.Table("Tag")
                 .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                 .WithColumn("Name").AsString(64).NotNullable()
                 .WithColumn("CreateDate").AsDateTime2().NotNullable()
                 .WithColumn("Slug").AsString(64).NotNullable().WithDefaultValue("");
            }

            if (!Schema.Table("User").Exists())
            {
                Create.Table("User")
                 .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                 .WithColumn("Name").AsString(64).NotNullable()
                 .WithColumn("Email").AsString(256).NotNullable()
                 .WithColumn("Password").AsString(64).NotNullable()
                 .WithColumn("IsSystem").AsBoolean().NotNullable()
                 .WithColumn("RegDate").AsDateTime2().NotNullable()
                 .WithColumn("NickName").AsString(64).NotNullable()
                 .WithColumn("Url").AsString(256).NotNullable()
                 .WithColumn("Status").AsInt32().NotNullable();


                Insert.IntoTable("User").Row(new { Name = "admin", Password = "admin".ToMd5(), Email = "", IsSystem = true, RegDate = DateTime.Now, NickName = "", Url = "", Status = (int)UserEnum.Normal });

            }

            if (!Schema.Table("Log").Exists())
            {
                Create.Table("Log")
                 .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                 .WithColumn("UserId").AsInt32().NotNullable().WithDefaultValue(0)
                 .WithColumn("Type").AsInt32().NotNullable().WithDefaultValue(0)
                 .WithColumn("RelatedId").AsInt32().NotNullable().WithDefaultValue(0)
                 .WithColumn("RelatedType").AsInt32().NotNullable().WithDefaultValue(0)
                 .WithColumn("Content").AsString(512).NotNullable().WithDefaultValue("")
                 .WithColumn("Success").AsBoolean().NotNullable().WithDefaultValue(true)
                 .WithColumn("Ip").AsString(64).NotNullable().WithDefaultValue("")
                 .WithColumn("CreateDate").AsDateTime2().NotNullable().WithDefault(SystemMethods.CurrentDateTime);
            }

        }



        public override void Down()
        {


            if (Schema.Table("Article").Exists())
            {
                Delete.Table("Article");
            }

            if (Schema.Table("ArticleTag").Exists())
            {
                Delete.Table("ArticleTag");
            }


            if (Schema.Table("Tag").Exists())
            {
                Delete.Table("Tag");
            }


            if (Schema.Table("Category").Exists())
            {
                Delete.Table("Category");
            }

            if (Schema.Table("User").Exists())
            {
                Delete.Table("User");
            }


            if (Schema.Table("Log").Exists())
            {
                Delete.Table("Log");
            }
        }
    }
}
