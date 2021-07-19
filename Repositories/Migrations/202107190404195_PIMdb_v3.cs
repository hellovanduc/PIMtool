namespace Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PIMdb_v3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GroupDetail", "Group_ID", c => c.Long());
            CreateIndex("dbo.GroupDetail", "Group_ID");
            AddForeignKey("dbo.GroupDetail", "Group_ID", "dbo.Group", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GroupDetail", "Group_ID", "dbo.Group");
            DropIndex("dbo.GroupDetail", new[] { "Group_ID" });
            DropColumn("dbo.GroupDetail", "Group_ID");
        }
    }
}
