using NUnit.Framework;
using Repositories;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMToolTest
{
    [TestFixture]
    public class GenerateData
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        [Test]
        public void Initialize()
        {
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

            //  Save test data to database
            using (_unitOfWork)
            {
                foreach (var employee in employees)
                {
                    _unitOfWork.EmployeeRepository.Insert(employee);
                }
                foreach (var group in groups)
                {
                    _unitOfWork.GroupRepository.Insert(group);
                }

                _unitOfWork.Save();
            }
        }
        private Employee NewEmployee(string visa, string firstName, string lastName, DateTime birthDate)
        {
            return new Employee
            {
                Visa = visa,
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthDate,
            };
        }

        private Group NewGroup(string GroupName, Employee GroupLeader)
        {
            return new Group
            {
                Name = GroupName,
                GroupLeader = GroupLeader
            };
        }
    }
}
