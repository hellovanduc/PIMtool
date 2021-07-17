using Repositories;
using Repositories.Enums;
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
        private readonly UnitOfWork _unitOfWork;
        
        public ProjectService()
        {
            _unitOfWork = new UnitOfWork();
        }

        public Project CreateProject(Project project)
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

            using (_unitOfWork)
            {
                _unitOfWork.ProjectRepository.Insert(project);
                _unitOfWork.Save();
            }

            return project;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employees"></param>
        /// <returns>List of invalid visas in set employees. Return null if the set employees is null or no invalid visas is founded</returns>
        private List<string> FindInvalidVisas(ISet<Employee> employees)
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
            IList<Employee> foundedEmployees = FindEmployeesByVisas(visas);

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

        public IList<Project> FindAllProjects()
        {
            IList<Project> projects;

            using (_unitOfWork)
            {
                projects = _unitOfWork.ProjectRepository.Get().ToList();
            }

            foreach (var project in projects)
            {
                project.EMPLOYEES = null;
                project.GROUP = null;
            }

            return projects;
        }

        public IList<Project> FindAllProjects(string searchString, string projectStatus, string sortOrder)
        {
            IList<Project> projects;
            using (_unitOfWork)
            {
                searchString = searchString.ToLower();
                Status searchStatus = StatusHelper.StringToStatus(projectStatus);
                try
                {
                    //  If searchString can convert to an integer, we can compare it with PROJECT_NUMBER to filter the result
                    int searchNumber = Convert.ToInt32(searchString);

                    projects = _unitOfWork.ProjectRepository
                        .Get(x => x.PROJECT_NUMBER == searchNumber
                                || x.NAME == searchString
                                || x.STATUS == searchStatus)
                        .ToList();
                }
                catch (FormatException)
                {
                    //  If searchString cannot convert to an integer, we will not check PROJECT_NUMBER of the project
                    projects = _unitOfWork.ProjectRepository
                        .Get(x => x.NAME == searchString
                                || x.STATUS == searchStatus)
                        .ToList();
                }
            }

            projects = SortProjects(projects, sortOrder);

            return projects;
        }

        public IList<Employee> FindAllEmployees()
        {
            using (_unitOfWork)
            {
                var employees = _unitOfWork.EmployeeRepository.Get().ToList();
                _unitOfWork.Save();

                return employees;
            }
        }

        public IList<Group> FindAllGroups()
        {
            IList<Group> groups;

            using (_unitOfWork)
            {
                groups = _unitOfWork.GroupRepository.Get().ToList();
            }

            return groups;
        }

        public Project FindProjectByProjectNumber(int projectNumber)
        {
            using (_unitOfWork)
            {
                var project = _unitOfWork.ProjectRepository
                    .Get(x => x.PROJECT_NUMBER == projectNumber)
                    .FirstOrDefault();
                return project;
            }
        }

        public Project UpdateProjectByProjectNumber(int projectNumber, Project newProject)
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

            using (_unitOfWork)
            {
                var project = _unitOfWork.ProjectRepository.Get(x => x.PROJECT_NUMBER == projectNumber)
                                .FirstOrDefault();
                if (project == null || newProject == null) return null;

                AssignProject(project, newProject);

                if (project.VERSION != newProject.VERSION)
                {
                    throw new ObjectHasBeenModifiedException(Resources.Resources.Resources.Resources.StaleProjectError + project.PROJECT_NUMBER);
                }

                _unitOfWork.ProjectRepository.Update(project);
                _unitOfWork.Save();

                return project;
            }
        }

        /// <summary>
        /// Assign all properties of source project to target project (except PROJECT_NUMBER and ID)
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        private void AssignProject(Project target, Project source)
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
            using (_unitOfWork)
            {
                int count = 0;
                foreach (int projectNumber in projectNumbers)
                {
                    Project project = _unitOfWork.ProjectRepository.Get(x => x.PROJECT_NUMBER == projectNumber)
                        .FirstOrDefault();
                    if (project != null)
                    {
                        _unitOfWork.ProjectRepository.Delete(project);
                        count++;
                    }
                }
                _unitOfWork.Save();
                return count;
            }
        }

        public IList<Employee> FindEmployeesByVisas(List<string> visas)
        {
            if (visas == null)
            {
                return null;
            }
            using (_unitOfWork)
            {
                IList<Employee> employees = new List<Employee>();

                foreach (string visa in visas)
                {
                    Employee employee = _unitOfWork.EmployeeRepository.Get(x => x.VISA == visa)
                        .FirstOrDefault();
                    if (employee != null)
                    {
                        employees.Add(employee);
                    }
                }

                return employees;
            }
        }

        public Group FindGroupByName(string name)
        {
            using (_unitOfWork)
            {
                var group = _unitOfWork.GroupRepository.Get(x => x.NAME == name).FirstOrDefault();
                return group;
            }
        }

        /// <summary>
        /// Sort the projects list by the sortOrder
        /// </summary>
        /// <param name="projects"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        private IList<Project> SortProjects(IList<Project> projects, string sortOrder)
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