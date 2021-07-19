namespace Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PIMdb_addSlogan : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GroupDetail", "Slogan", c => c.String());
            DropColumn("dbo.GroupDetail", "MaximumNumberOfEmployee");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GroupDetail", "MaximumNumberOfEmployee", c => c.Int(nullable: false));
            DropColumn("dbo.GroupDetail", "Slogan");
        }
    }
}
