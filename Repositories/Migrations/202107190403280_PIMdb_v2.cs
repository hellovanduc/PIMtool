namespace Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PIMdb_v2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GroupDetail",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        EstablishDate = c.DateTime(nullable: false),
                        NumberOfProjectHasDone = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GroupDetail");
        }
    }
}
