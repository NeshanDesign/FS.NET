namespace FsNet.Data.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddValidationAttrToApplicationRole : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Role", "CreatedBy", c => c.String(maxLength: 35));
            AlterColumn("dbo.Role", "ModifiedBy", c => c.String(maxLength: 35));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Role", "ModifiedBy", c => c.String());
            AlterColumn("dbo.Role", "CreatedBy", c => c.String());
        }
    }
}
