using Repositories.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Repositories
{
    public class ProjectManagementContext : DbContext
    {
        public ProjectManagementContext() : base("ProjectManagementContext")
        {

        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Project> Projects { get; set; }

        // Prevents table names from being pluralized (ex: Employees => Employee)
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
