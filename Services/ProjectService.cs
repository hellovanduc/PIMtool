using Library.Resources.Resources;
using Repositories.Enums;
using Repositories.Interfaces;
using Repositories.Models;
using Services.Exceptions;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IGenericRepository genericRepository;
        private readonly IProjectRepository projectRepository;
        private readonly IEmployeeRepository employeeRepository;
        private readonly IGroupRepository groupRepository;
        public ProjectService(
            IUnitOfWork unitOfWork,
            IGenericRepository genericRepository,
            IProjectRepository projectRepository,
            IEmployeeRepository employeeRepository,
            IGroupRepository groupRepository)
        {
            this.unitOfWork = unitOfWork;
            this.genericRepository = genericRepository;
            this.projectRepository = projectRepository;
            this.groupRepository = groupRepository;
            this.employeeRepository = employeeRepository;
        }
        public PROJECT CreateProject(PROJECT project)
        {
            if (FindProjectByProjectNumber(project.PROJECT_NUMBER) != null)
            {
                throw new ProjectNumberAlreadyExistsException(project.PROJECT_NUMBER);
            }

            List<string> invalidVisas = FindInvalidVisas(project.EMPLOYEES);
            if (invalidVisas != null)
            {
                throw new InvalidVisasException(invalidVisas);
            }

            if (FindGroupByName(project.GROUP.NAME) == null)
            {
                throw new InvalidGroupNameException(project.GROUP.NAME);
            }

            if (project.END_DATE != null && DateTime.Compare(project.START_DATE, (DateTime)project.END_DATE) > 0)
            {
                throw new EndDateEarlierThanStartDateException(project.START_DATE, (DateTime)project.END_DATE);
            }

            using (unitOfWork.Start())
            {
                genericRepository.Save(project);
                unitOfWork.Commit();
            }

            return project;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employees"></param>
        /// <returns>List of invalid visas in set employees. Return null if the set employees is null or no invalid visas is founded</returns>
        private List<string> FindInvalidVisas(ISet<EMPLOYEE> employees)
        {
            if (employees == null)
            {
                return null;
            }

            //  Init list invalidVisas and list all visas in set employees
            List<string> invalidVisas = null;
            List<string> visas = new List<string>();
            foreach (var employee in employees)
            {
                visas.Add(employee.VISA);
            }

            //  Find the employees corresponding with list visas
            IList<EMPLOYEE> foundedEmployees = FindEmployeesByVisas(visas);

            //  If the number of employees is founded less than the number of employees in set employees, find the invalid visas
            if (foundedEmployees.Count < employees.Count)
            {
                List<string> foundedVisas = new List<string>();
                foreach (var employee in foundedEmployees)
                {
                    foundedVisas.Add(employee.VISA);
                }

                invalidVisas = visas.Except(foundedVisas).ToList();
            }

            return invalidVisas;
        }

        public IList<PROJECT> FindAllProjects()
        {
            IList<PROJECT> projects;

            using (unitOfWork.Start())
            {
                projects = projectRepository.FindAllProjects();
            }

            foreach (var project in projects)
            {
                project.EMPLOYEES = null;
                project.GROUP = null;
            }

            return projects;
        }

        public IList<PROJECT> FindAllProjects(string searchString, string projectStatus, string sortOrder)
        {
            IList<PROJECT> projects;
            using (unitOfWork.Start())
            {
                searchString = searchString.ToLower();
                try
                {
                    //  If searchString can convert to an integer, we can compare it with PROJECT_NUMBER to filter the result
                    int searchNumber = Convert.ToInt32(searchString);

                    projects = projectRepository.FindAllProjects(searchString, projectStatus, searchNumber);
                }
                catch (FormatException)
                {
                    //  If searchString cannot convert to an integer, we will not check PROJECT_NUMBER of the project
                    projects = projectRepository.FindAllProjects(searchString, projectStatus);
                }
            }

            projects = SortProjects(projects, sortOrder);

            return projects;
        }

        public IList<EMPLOYEE> FindAllEmployees()
        {
            using (unitOfWork.Start())
            {
                var employees = employeeRepository.FindAllEmployees();
                unitOfWork.Commit();

                return employees;
            }
        }

        public IList<GROUPS> FindAllGroups()
        {
            IList<GROUPS> groups;

            using (unitOfWork.Start())
            {
                groups = groupRepository.FindAllGroups();
            }

            return groups;
        }

        public PROJECT FindProjectByProjectNumber(int projectNumber)
        {
            using (unitOfWork.Start())
            {
                var project = projectRepository.FindProjectByProjectNumber(projectNumber);
                return project;
            }
        }

        public PROJECT UpdateProjectByProjectNumber(int projectNumber, PROJECT newProject)
        {
            List<string> invalidVisas = FindInvalidVisas(newProject.EMPLOYEES);
            if (invalidVisas != null)
            {
                throw new InvalidVisasException(invalidVisas);
            }

            if (FindGroupByName(newProject.GROUP.NAME) == null)
            {
                throw new InvalidGroupNameException(newProject.GROUP.NAME);
            }

            if (newProject.END_DATE != null && DateTime.Compare(newProject.START_DATE, (DateTime)newProject.END_DATE) > 0)
            {
                throw new EndDateEarlierThanStartDateException(newProject.START_DATE, (DateTime)newProject.END_DATE);
            }

            using (unitOfWork.Start())
            {
                var project = projectRepository.FindProjectByProjectNumber(projectNumber);
                if (project == null || newProject == null) return null;

                AssignProject(project, newProject);

                if (project.VERSION != newProject.VERSION)
                {
                    throw new ObjectHasBeenModifiedException(Resources.StaleProjectError + project.PROJECT_NUMBER);
                }

                genericRepository.Update(project);
                unitOfWork.Commit();

                return project;
            }
        }

        /// <summary>
        /// Assign all properties of source project to target project (except PROJECT_NUMBER and ID)
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        private void AssignProject(PROJECT target, PROJECT source)
        {
            target.NAME = source.NAME;
            target.CUSTOMER = source.CUSTOMER;
            target.STATUS = source.STATUS;
            target.START_DATE = source.START_DATE;
            target.END_DATE = source.END_DATE;
            target.EMPLOYEES = source.EMPLOYEES;
            target.GROUP = source.GROUP;
        }

        public int DeleteProjectsByProjectNumber(List<int> projectNumbers)
        {
            if (projectNumbers == null)
            {
                return 0;
            }
            using (unitOfWork.Start())
            {
                int count = projectRepository.DeleteProjectsByProjectNumbers(projectNumbers);
                unitOfWork.Commit();
                return count;
            }
        }

        public IList<EMPLOYEE> FindEmployeesByVisas(List<string> visas)
        {
            if (visas == null)
            {
                return null;
            }
            using (unitOfWork.Start())
            {
                var employees = employeeRepository.FindEmployeesByVisas(visas);

                return employees;
            }
        }

        public GROUPS FindGroupByName(string name)
        {
            using (unitOfWork.Start())
            {
                var group = groupRepository.FindGroupByName(name);

                return group;
            }
        }

        /// <summary>
        /// Sort the projects list by the sortOrder
        /// </summary>
        /// <param name="projects"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        private IList<PROJECT> SortProjects(IList<PROJECT> projects, string sortOrder)
        {
            var order = SortOrderHelper.StringToSortOrder(sortOrder);

            switch (order)
            {
                case SortOrder.number_desc:
                    projects = projects.OrderByDescending(p => p.PROJECT_NUMBER).ToList();
                    break;
                case SortOrder.name:
                    projects = projects.OrderBy(p => p.NAME).ToList();
                    break;
                case SortOrder.name_desc:
                    projects = projects.OrderByDescending(p => p.NAME).ToList();
                    break;
                case SortOrder.status:
                    projects = projects.OrderBy(p => p.STATUS).ToList();
                    break;
                case SortOrder.status_desc:
                    projects = projects.OrderByDescending(p => p.STATUS).ToList();
                    break;
                case SortOrder.customer:
                    projects = projects.OrderBy(p => p.CUSTOMER).ToList();
                    break;
                case SortOrder.customer_desc:
                    projects = projects.OrderByDescending(p => p.CUSTOMER).ToList();
                    break;
                case SortOrder.date:
                    projects = projects.OrderBy(p => p.START_DATE).ToList();
                    break;
                case SortOrder.date_desc:
                    projects = projects.OrderByDescending(p => p.START_DATE).ToList();
                    break;
                default:
                    projects = projects.OrderBy(p => p.PROJECT_NUMBER).ToList();
                    break;
            }

            return projects;
        }
    }
}