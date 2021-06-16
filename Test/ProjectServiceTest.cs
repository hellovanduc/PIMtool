using log4net;
using NUnit.Framework;
using Repositories;
using Repositories.Enums;
using Repositories.Interfaces;
using Repositories.Models;
using Services;
using Services.Exceptions;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity;

namespace Test
{
    [TestFixture]
    public class ProjectServiceTest
    {
        private IUnitOfWork unitOfWork;
        private IProjectService projectService;

        private IGenericRepository genericRepository;
        private IProjectRepository projectRepository;

        private readonly ISet<EMPLOYEE> employees = new HashSet<EMPLOYEE>();
        private readonly ISet<GROUPS> groups = new HashSet<GROUPS>();
        private readonly ISet<PROJECT> projects = new HashSet<PROJECT>();

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            //  Register injector
            var container = new UnityContainer();

            container.RegisterSingleton<IUnitOfWork, UnitOfWork>();
            container.RegisterSingleton<IProjectService, ProjectService>();

            #region Register singleton for repositories
            container.RegisterSingleton<IGenericRepository, GenericRepository>();
            container.RegisterSingleton<IProjectRepository, ProjectRepository>();
            container.RegisterSingleton<IEmployeeRepository, EmployeeRepository>();
            container.RegisterSingleton<IGroupRepository, GroupRepository>();
            #endregion

            ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            container.RegisterInstance(log);

            unitOfWork = container.Resolve<IUnitOfWork>();
            projectService = container.Resolve<IProjectService>();
            genericRepository = container.Resolve<IGenericRepository>();
            projectRepository = container.Resolve<IProjectRepository>();

            //  Init data
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

            groups.Add(NewGroup("DevOps", employees.ElementAt(0)));
            groups.Add(NewGroup("Flutter", employees.ElementAt(1)));
            groups.Add(NewGroup("React Native", employees.ElementAt(2)));

            //  Save test data to database
            using (unitOfWork.Start())
            {
                foreach (var employee in employees)
                {
                    genericRepository.Save(employee);
                }
                foreach (var group in groups)
                {
                    genericRepository.Save(group);
                }

                unitOfWork.Commit();
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            using (unitOfWork.Start())
            {
                foreach (var group in groups)
                {
                    genericRepository.Delete(group);
                }

                foreach (var employee in employees)
                {
                    genericRepository.Delete(employee);
                }

                unitOfWork.Commit();
            }
        }

        [TearDown]
        public void TearDown()
        {
            using (unitOfWork.Start())
            {
                foreach (var project in projects)
                {
                    genericRepository.Delete(project);
                }

                unitOfWork.Commit();
            }

            projects.Clear();
        }


        [Test]
        public void CreateProject__InsertOneValidProject__ShouldInsertTheProjectSuccessfully()
        {
            //  Arrange
            var created_project = NewProject(3000, "Viacar", "ELCA", new DateTime(2021, 3, 15), groups.ElementAt(0),
                    new HashSet<EMPLOYEE> { employees.ElementAt(0), employees.ElementAt(1), employees.ElementAt(2) });
            projects.Add(created_project);

            //  Act
            projectService.CreateProject(created_project);

            //  Assert
            PROJECT result_project;
            using (unitOfWork.Start())
            {
                result_project = projectRepository.FindProjectByProjectNumber(created_project.PROJECT_NUMBER);
                unitOfWork.Commit();
            }
            Assert.AreNotEqual(null, result_project);
            AreProjectsEqual(created_project, result_project);
        }
        [Test]
        public void CreateProject__ProjectNumberAlreadyExists__ShouldThrowProjectNumberAlreadyExistsException()
        {
            //  Arrange
            PROJECT project1 = NewProject(3000, "Viacar", "ELCA", new DateTime(2021, 3, 26), groups.ElementAt(0), null);
            PROJECT project2 = NewProject(3000, "Viacar", "ELCA", new DateTime(2021, 3, 26), groups.ElementAt(0), null);
            projectService.CreateProject(project1);
            projects.Add(project1);

            //  Act
            void createProject() => projectService.CreateProject(project2);

            //  Assert
            Assert.Throws(typeof(ProjectNumberAlreadyExistsException), createProject);
        }

        [Test]
        public void CreateProject__MembersHaveInvalidVisa__ShouldThrowInvalidVisasException()
        {
            //  Arrange
            PROJECT project = NewProject(3000, "Viacar", "ELCA", new DateTime(2021, 3, 26), groups.ElementAt(0),
                new HashSet<EMPLOYEE> { new EMPLOYEE {
                    VISA = "LLL",
                    FIRST_NAME = "Nguyen",
                    LAST_NAME = "Van",
                    BIRTH_DATE = new DateTime(2000, 3,26)
                }});

            //  Act
            void createProject() => projectService.CreateProject(project);

            //  Assert
            Assert.Throws(typeof(InvalidVisasException), createProject);
        }

        [Test]
        public void CreateProject__InvalidGroupName__ShouldThrowInvalidGroupNamexception()
        {
            //  Arrange
            PROJECT project = NewProject(3000, "Viacar", "ELCA", new DateTime(2021, 3, 26),
                new GROUPS { NAME = "ABC", GROUP_LEADER = employees.ElementAt(0) }, null);

            //  Act
            void createProject() => projectService.CreateProject(project);

            //  Assert
            Assert.Throws(typeof(InvalidGroupNameException), createProject);
        }

        [Test]
        public void UpdateProjectByProjectNumber__FindOneProjectByProjectNumberAndUpdateIt__ShouldUpdateTheProjectSuccessully()
        {
            //  Arrange
            var created_project = NewProject(3000, "Viacar", "ELCA", new DateTime(2021, 3, 15), groups.ElementAt(0),
                    new HashSet<EMPLOYEE> { employees.ElementAt(0), employees.ElementAt(1), employees.ElementAt(2) });
            projectService.CreateProject(created_project);

            //  Act
            created_project.NAME = "new name";
            created_project.STATUS = Status.INP;
            created_project.EMPLOYEES.Add(employees.ElementAt(3));
            created_project.GROUP = groups.ElementAt(2);

            var updated_project = projectService.UpdateProjectByProjectNumber(created_project.PROJECT_NUMBER, created_project);
            projects.Add(updated_project);

            //  Assert
            AreProjectsEqual(updated_project, created_project);
        }

        [Test]
        public void FindAllProjects__InsertThreeTestProjectsToDbAndFindThem__AllThreeTestProjectsShouldBeFounded()
        {
            //  Arrange
            projects.Add(NewProject(3000, "Viacar", "ELCA", new DateTime(2021, 3, 15), groups.ElementAt(0),
                        new HashSet<EMPLOYEE> { employees.ElementAt(0), employees.ElementAt(1), employees.ElementAt(2) }));
            projects.Add(NewProject(3001, "Viacar", "ELCA", new DateTime(2021, 3, 15), groups.ElementAt(0),
                        new HashSet<EMPLOYEE> { employees.ElementAt(0), employees.ElementAt(1), employees.ElementAt(2) }));
            projects.Add(NewProject(3002, "Viacar", "ELCA", new DateTime(2021, 3, 15), groups.ElementAt(0),
                        new HashSet<EMPLOYEE> { employees.ElementAt(0), employees.ElementAt(1), employees.ElementAt(2) }));

            using (unitOfWork.Start())
            {
                genericRepository.Save(projects.ElementAt(0));
                genericRepository.Save(projects.ElementAt(1));
                genericRepository.Save(projects.ElementAt(2));

                unitOfWork.Commit();
            }

            //  Act
            var founded_projects = (List<PROJECT>)projectService.FindAllProjects();

            //  Assert
            foreach (var project in projects)
            {
                PROJECT founded_project = founded_projects.Find(p => p.ID == project.ID);

                Assert.AreNotEqual(null, founded_project);
                AreProjectsEqual(founded_project, project);
            }
        }

        [Test]
        public void FindAllEmployees__InsertTenEmployeesToDbAndFindThem__AllTenTestEmployeesShouldBeFounded()
        {
            //  Act
            var founded_employees = (List<EMPLOYEE>)projectService.FindAllEmployees();

            //  Assert
            foreach (var employee in employees)
            {
                EMPLOYEE founded_employee = founded_employees.Find(p => p.ID == employee.ID);

                Assert.AreNotEqual(null, founded_employee);
                AreEmployeesEqual(founded_employee, employee);
            }
        }

        [Test]
        public void FindAllGroups__InsertThreeGroupsToDbAndFindThem__AllThreeTestGroupsShouldBeFounded()
        {
            //  Act
            var founded_groups = (List<GROUPS>)projectService.FindAllGroups();

            //  Assert
            foreach (var group in groups)
            {
                GROUPS founded_group = founded_groups.Find(p => p.ID == group.ID);

                Assert.AreNotEqual(null, founded_group);
                AreGroupsEqual(founded_group, group);
            }
        }

        [Test]
        public void FindProjectByProjectNumber__InsertOneProjectToDbAndFindIt__ProjectWithRelevantEmployeeAndGroupShouldBeFounded()
        {
            //  Arrange
            var project_needed_tobe_founded = NewProject(3000, "Viacar", "ELCA", new DateTime(2021, 3, 15), groups.ElementAt(0),
                    new HashSet<EMPLOYEE> { employees.ElementAt(0), employees.ElementAt(1), employees.ElementAt(2) });
            projectService.CreateProject(project_needed_tobe_founded);
            projects.Add(project_needed_tobe_founded);

            // Act
            var founded_project = projectService.FindProjectByProjectNumber(project_needed_tobe_founded.PROJECT_NUMBER);

            // Assert
            AreProjectsEqual(project_needed_tobe_founded, founded_project);
            AreSetEmployeesEqual(project_needed_tobe_founded.EMPLOYEES, founded_project.EMPLOYEES);
            AreGroupsEqual(project_needed_tobe_founded.GROUP, founded_project.GROUP);
        }

        [Test]
        public void DeleteProjectsByProjectNumber__DeleteTwoProjectsByProjectNumber__TwoProjectCorrespondingToProjectNumbersShouldBeDeleted()
        {
            //  Arrange
            PROJECT project1 = NewProject(3000, "Viacar", "ELCA", new DateTime(2021, 3, 15), groups.ElementAt(0),
                    new HashSet<EMPLOYEE> { employees.ElementAt(0), employees.ElementAt(1), employees.ElementAt(2) });
            PROJECT project2 = NewProject(3001, "Viacar", "ELCA", new DateTime(2021, 3, 15), groups.ElementAt(0),
                    new HashSet<EMPLOYEE> { employees.ElementAt(0), employees.ElementAt(1), employees.ElementAt(2) });

            using (unitOfWork.Start())
            {
                genericRepository.Save(project1);
                genericRepository.Save(project2);

                unitOfWork.Commit();
            }

            //  Act
            var projectNumbers = new List<int>
            {
                project1.PROJECT_NUMBER,
                project2.PROJECT_NUMBER
            };

            int numOfDeleted = projectService.DeleteProjectsByProjectNumber(projectNumbers);

            //  Assert
            Assert.AreEqual(2, numOfDeleted);
            foreach (var projectNumber in projectNumbers)
            {
                Assert.AreEqual(null, projectService.FindProjectByProjectNumber(projectNumber));
            }
        }

        /// <summary>
        /// Check if all properties of two project are the same, except EMPLOYEES and GROUPS
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        private void AreProjectsEqual(PROJECT p1, PROJECT p2)
        {
            Assert.AreEqual(p1.ID, p2.ID);
            Assert.AreEqual(p1.PROJECT_NUMBER, p2.PROJECT_NUMBER);
            Assert.AreEqual(p1.NAME, p2.NAME);
            Assert.AreEqual(p1.CUSTOMER, p2.CUSTOMER);
            Assert.AreEqual(p1.STATUS, p2.STATUS);
            Assert.AreEqual(p1.END_DATE, p2.END_DATE);
        }
        private void AreGroupsEqual(GROUPS g1, GROUPS g2)
        {
            Assert.AreEqual(g1.ID, g2.ID);
            Assert.AreEqual(g1.NAME, g2.NAME);
        }
        private void AreEmployeesEqual(EMPLOYEE e1, EMPLOYEE e2)
        {
            Assert.AreEqual(e1.ID, e2.ID);
            Assert.AreEqual(e1.VISA, e2.VISA);
            Assert.AreEqual(e1.FIRST_NAME, e2.FIRST_NAME);
            Assert.AreEqual(e1.LAST_NAME, e2.LAST_NAME);
            Assert.AreEqual(e1.BIRTH_DATE, e1.BIRTH_DATE);
        }
        private void AreSetEmployeesEqual(ISet<EMPLOYEE> set1, ISet<EMPLOYEE> set2)
        {
            Assert.AreEqual(set1.Count, set2.Count);

            var list1 = set1.ToList();
            var list2 = set2.ToList();

            foreach (var employee1 in list1)
            {
                var employee2 = list2.Find(e => e.ID == employee1.ID);
                Assert.AreNotEqual(null, employee2);
                AreEmployeesEqual(employee1, employee2);
            }
        }

        private PROJECT NewProject(int projectNumber, string name, string customer, DateTime startDate, GROUPS group, ISet<EMPLOYEE> employees)
        {
            return new PROJECT
            {
                PROJECT_NUMBER = projectNumber,
                NAME = name,
                CUSTOMER = customer,
                START_DATE = startDate,
                EMPLOYEES = employees,
                GROUP = group
            };
        }

        /// <summary>
        /// Create new employee with the parameters in parameters list.
        /// </summary>
        /// <param name="visa"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="birthDate"></param>
        /// <returns></returns>
        private EMPLOYEE NewEmployee(string visa, string firstName, string lastName, DateTime birthDate)
        {
            return new EMPLOYEE
            {
                VISA = visa,
                FIRST_NAME = firstName,
                LAST_NAME = lastName,
                BIRTH_DATE = birthDate,
            };
        }

        private GROUPS NewGroup(string GroupName, EMPLOYEE GroupLeader)
        {
            return new GROUPS
            {
                NAME = GroupName,
                GROUP_LEADER = GroupLeader
            };
        }
    }
}