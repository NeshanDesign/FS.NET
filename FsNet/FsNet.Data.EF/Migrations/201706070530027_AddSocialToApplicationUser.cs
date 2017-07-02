namespace FsNet.Data.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSocialToApplicationUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "TelegramUrl", c => c.String(maxLength: 100));
            AddColumn("dbo.User", "InstagramId", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "InstagramId");
            DropColumn("dbo.User", "TelegramUrl");
        }
    }
}
