namespace Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PIMdb_v4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GroupDetail", "Group_ID", "dbo.Group");
            DropIndex("dbo.GroupDetail", new[] { "Group_ID" });
            DropColumn("dbo.GroupDetail", "Group_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GroupDetail", "Group_ID", c => c.Long());
            CreateIndex("dbo.GroupDetail", "Group_ID");
            AddForeignKey("dbo.GroupDetail", "Group_ID", "dbo.Group", "ID");
        }
    }
}
