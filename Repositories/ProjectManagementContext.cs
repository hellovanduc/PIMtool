using Repositories.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Repositories
{
    public class ProjectManagementContext : DbContext
    {
        public ProjectManagementContext() : base("ProjectManagementContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ProjectManagementContext,
                Migrations.Configuration>());
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Project> Projects { get; set; }
         
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Prevents table names from being pluralized (ex: Employees => Employee)
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Group>()
                .HasRequired(x => x.GROUP_LEADER);
        }
    }
}
