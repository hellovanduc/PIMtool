namespace Repositories.Migrations
{
    using Repositories.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Repositories.ProjectManagementContext>
    {
        public Configuration()
        {
            ContextKey = "Repositories.ProjectManagementContext";
        }

        protected override void Seed(Repositories.ProjectManagementContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            IList<Employee> employees = new List<Employee>();
            employees.Add(NewEmployee("AAA", "Nguyen", "Van Duc", new DateTime(2000, 3, 26)));
            employees.Add(NewEmployee("BBB", "Tran", "Van Duc", new DateTime(2000, 3, 26)));
            employees.Add(NewEmployee("CCC", "Le", "Van Duc", new DateTime(2000, 3, 26)));
            employees.Add(NewEmployee("DDD", "Dang", "Van Duc", new DateTime(2000, 3, 26)));
            employees.Add(NewEmployee("EEE", "Dong", "Van Duc", new DateTime(2000, 3, 26)));
            employees.Add(NewEmployee("FFF", "Phan", "Van Duc", new DateTime(2000, 3, 26)));
            employees.Add(NewEmployee("GGG", "Ly", "Van Duc", new DateTime(2000, 3, 26)));
            employees.Add(NewEmployee("HHH", "Truong", "Van Duc", new DateTime(2000, 3, 26)));
            employees.Add(NewEmployee("III", "Dinh", "Van Duc", new DateTime(2000, 3, 26)));
            employees.Add(NewEmployee("KKK", "Trieu", "Van Duc", new DateTime(2000, 3, 26)));

            IList<Group> groups = new List<Group>();
            groups.Add(NewGroup("DevOps", employees.ElementAt(0)));
            groups.Add(NewGroup("Flutter", employees.ElementAt(1)));
            groups.Add(NewGroup("React Native", employees.ElementAt(2)));

            foreach (Employee employee in employees)
            {
                context.Set<Employee>().AddOrUpdate(employee);
            }

            foreach (Group group in groups)
            {
                context.Set<Group>().AddOrUpdate(group);
            }

            context.SaveChanges();
        }

        private Employee NewEmployee(string visa, string firstName, string lastName, DateTime birthDate)
        {
            return new Employee
            {
                VISA = visa,
                FIRST_NAME = firstName,
                LAST_NAME = lastName,
                BIRTH_DATE = birthDate,
            };
        }

        private Group NewGroup(string GroupName, Employee GroupLeader)
        {
            return new Group
            {
                NAME = GroupName,
                GROUP_LEADER = GroupLeader
            };
        }
    }
}
