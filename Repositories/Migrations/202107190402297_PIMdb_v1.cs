namespace Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PIMdb_v1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employee", "FULL_NAME", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employee", "FULL_NAME");
        }
    }
}
