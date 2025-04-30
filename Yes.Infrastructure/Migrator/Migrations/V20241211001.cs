namespace Yes.Infrastructure.Migrator.Migrations
{
    [Migration(20241211001)]
    public class V20241211001 : Migration
    {
        public override void Up()
        {

            if (!Schema.Table("Comment").Exists())
            {
                Create.Table("Comment")
                 .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                 .WithColumn("ArticleId").AsInt32().NotNullable().WithDefaultValue(0)
                 .WithColumn("Content").AsString(512).NotNullable().WithDefaultValue("")
                 .WithColumn("CreateDate").AsDateTime2().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
                 .WithColumn("Email").AsString(256).NotNullable().WithDefaultValue("")
                 .WithColumn("Url").AsString(256).NotNullable().WithDefaultValue("")
                 .WithColumn("IP").AsString(64).NotNullable().WithDefaultValue("")
                 .WithColumn("Referer").AsString(256).NotNullable().WithDefaultValue("")
                 .WithColumn("UserId").AsInt32().NotNullable().WithDefaultValue(0)
                 .WithColumn("NickName").AsString(64).NotNullable().WithDefaultValue("")
                 .WithColumn("Status").AsInt32().NotNullable().WithDefaultValue(0);
            }

        }



        public override void Down()
        {


            if (Schema.Table("Comment").Exists())
            {
                Delete.Table("Comment");
            }


        }
    }
}
