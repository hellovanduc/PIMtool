namespace Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PIMdb_v0 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employee",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        VISA = c.String(),
                        FIRST_NAME = c.String(),
                        LAST_NAME = c.String(),
                        BIRTH_DATE = c.DateTime(nullable: false),
                        VERSION = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Project",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        PROJECT_NUMBER = c.Int(nullable: false),
                        NAME = c.String(),
                        CUSTOMER = c.String(),
                        STATUS = c.Int(nullable: false),
                        START_DATE = c.DateTime(nullable: false),
                        END_DATE = c.DateTime(),
                        VERSION = c.Int(nullable: false),
                        GROUP_ID = c.Long(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Group", t => t.GROUP_ID)
                .Index(t => t.GROUP_ID);
            
            CreateTable(
                "dbo.Group",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        NAME = c.String(),
                        VERSION = c.Int(nullable: false),
                        GROUP_LEADER_ID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Employee", t => t.GROUP_LEADER_ID, cascadeDelete: true)
                .Index(t => t.GROUP_LEADER_ID);
            
            CreateTable(
                "dbo.ProjectEmployee",
                c => new
                    {
                        Project_ID = c.Long(nullable: false),
                        Employee_ID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_ID, t.Employee_ID })
                .ForeignKey("dbo.Project", t => t.Project_ID, cascadeDelete: true)
                .ForeignKey("dbo.Employee", t => t.Employee_ID, cascadeDelete: true)
                .Index(t => t.Project_ID)
                .Index(t => t.Employee_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Project", "GROUP_ID", "dbo.Group");
            DropForeignKey("dbo.Group", "GROUP_LEADER_ID", "dbo.Employee");
            DropForeignKey("dbo.ProjectEmployee", "Employee_ID", "dbo.Employee");
            DropForeignKey("dbo.ProjectEmployee", "Project_ID", "dbo.Project");
            DropIndex("dbo.ProjectEmployee", new[] { "Employee_ID" });
            DropIndex("dbo.ProjectEmployee", new[] { "Project_ID" });
            DropIndex("dbo.Group", new[] { "GROUP_LEADER_ID" });
            DropIndex("dbo.Project", new[] { "GROUP_ID" });
            DropTable("dbo.ProjectEmployee");
            DropTable("dbo.Group");
            DropTable("dbo.Project");
            DropTable("dbo.Employee");
        }
    }
}
