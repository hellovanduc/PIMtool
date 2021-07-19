namespace Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PIMdb_addMaxNumOfEmployee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GroupDetail", "MaximumNumberOfEmployee", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GroupDetail", "MaximumNumberOfEmployee");
        }
    }
}
