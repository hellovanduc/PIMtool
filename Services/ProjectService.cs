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
            if (FindProjectByProjectNumber(project.ProjectNumber) != null)
            {
                throw new ProjectNumberAlreadyExistsException(project.ProjectNumber);
            }

            List<string> invalidVisas = FindInvalidVisas(project.Employees.ToHashSet());
            if (invalidVisas != null)
            {
                throw new InvalidVisasException(invalidVisas);
            }

            if (FindGroupByName(project.Group.Name) == null)
            {
                throw new InvalidGroupNameException(project.Group.Name);
            }

            if (project.EndDate != null && DateTime.Compare(project.StartDate, (DateTime)project.EndDate) > 0)
            {
                throw new EndDateEarlierThanStartDateException(project.StartDate, (DateTime)project.EndDate);
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
        /// <returns>List of invalid Visas in set employees. Return null if the set employees is null or no invalid Visas is founded</returns>
        private List<string> FindInvalidVisas(ISet<Employee> employees)
        {
            if (employees == null)
            {
                return null;
            }

            //  Init list invalidVisas and list all Visas in set employees
            List<string> invalidVisas = null;
            List<string> Visas = new List<string>();
            foreach (var employee in employees)
            {
                Visas.Add(employee.Visa);
            }

            //  Find the employees corresponding with list Visas
            IList<Employee> foundedEmployees = FindEmployeesByVisas(Visas);

            //  If the number of employees is founded less than the number of employees in set employees, find the invalid Visas
            if (foundedEmployees.Count < employees.Count)
            {
                List<string> foundedVisas = new List<string>();
                foreach (var employee in foundedEmployees)
                {
                    foundedVisas.Add(employee.Visa);
                }

                invalidVisas = Visas.Except(foundedVisas).ToList();
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
                project.Employees = null;
                project.Group = null;
            }

            return projects;
        }

        public IList<Project> FindAllProjects(string searchString, string projectStatus, string sortOrder)
        {
            IList<Project> projects;
            using (_unitOfWork)
            {
                searchString = searchString.ToLower();
                try
                {
                    //  If searchString can convert to an integer, we can compare it with ProjectNumber to filter the result
                    int searchNumber = Convert.ToInt32(searchString);

                    projects = _unitOfWork.ProjectRepository
                        .Get(x => x.ProjectNumber == searchNumber
                                || x.Name == searchString
                                || x.Status == projectStatus)
                        .ToList();
                }
                catch (FormatException)
                {
                    //  If searchString cannot convert to an integer, we will not check ProjectNumber of the project
                    projects = _unitOfWork.ProjectRepository
                        .Get(x => x.Name == searchString
                                || x.Status == projectStatus)
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

        public Project FindProjectByProjectNumber(decimal projectNumber)
        {
            using (_unitOfWork)
            {
                var project = _unitOfWork.ProjectRepository
                    .Get(x => x.ProjectNumber == projectNumber)
                    .FirstOrDefault();
                return project;
            }
        }

        public Project UpdateProjectByProjectNumber(decimal projectNumber, Project newProject)
        {
            List<string> invalidVisas = FindInvalidVisas(newProject.Employees.ToHashSet());
            if (invalidVisas != null)
            {
                throw new InvalidVisasException(invalidVisas);
            }

            if (FindGroupByName(newProject.Group.Name) == null)
            {
                throw new InvalidGroupNameException(newProject.Group.Name);
            }

            if (newProject.EndDate != null && DateTime.Compare(newProject.StartDate, (DateTime)newProject.EndDate) > 0)
            {
                throw new EndDateEarlierThanStartDateException(newProject.StartDate, (DateTime)newProject.EndDate);
            }

            using (_unitOfWork)
            {
                var project = _unitOfWork.ProjectRepository.Get(x => x.ProjectNumber == projectNumber)
                                .FirstOrDefault();
                if (project == null || newProject == null) return null;

                AssignProject(project, newProject);

                if (project.Version != newProject.Version)
                {
                    throw new ObjectHasBeenModifiedException(Resources.Resources.Resources.Resources.StaleProjectError + project.ProjectNumber);
                }

                _unitOfWork.ProjectRepository.Update(project);
                _unitOfWork.Save();

                return project;
            }
        }

        /// <summary>
        /// Assign all properties of source project to target project (except ProjectNumber and ID)
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        private void AssignProject(Project target, Project source)
        {
            target.Name = source.Name;
            target.Customer = source.Customer;
            target.Status = source.Status;
            target.StartDate = source.StartDate;
            target.EndDate = source.EndDate;
            target.Employees = source.Employees;
            target.Group = source.Group;
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
                    Project project = _unitOfWork.ProjectRepository.Get(x => x.ProjectNumber == projectNumber)
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

        public IList<Employee> FindEmployeesByVisas(List<string> Visas)
        {
            if (Visas == null)
            {
                return null;
            }
            using (_unitOfWork)
            {
                IList<Employee> employees = new List<Employee>();

                foreach (string Visa in Visas)
                {
                    Employee employee = _unitOfWork.EmployeeRepository.Get(x => x.Visa == Visa)
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
                var group = _unitOfWork.GroupRepository.Get(x => x.Name == name).FirstOrDefault();
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
                    projects = projects.OrderByDescending(p => p.ProjectNumber).ToList();
                    break;
                case SortOrder.name:
                    projects = projects.OrderBy(p => p.Name).ToList();
                    break;
                case SortOrder.name_desc:
                    projects = projects.OrderByDescending(p => p.Name).ToList();
                    break;
                case SortOrder.status:
                    projects = projects.OrderBy(p => p.Status).ToList();
                    break;
                case SortOrder.status_desc:
                    projects = projects.OrderByDescending(p => p.Status).ToList();
                    break;
                case SortOrder.customer:
                    projects = projects.OrderBy(p => p.Customer).ToList();
                    break;
                case SortOrder.customer_desc:
                    projects = projects.OrderByDescending(p => p.Customer).ToList();
                    break;
                case SortOrder.date:
                    projects = projects.OrderBy(p => p.StartDate).ToList();
                    break;
                case SortOrder.date_desc:
                    projects = projects.OrderByDescending(p => p.StartDate).ToList();
                    break;
                default:
                    projects = projects.OrderBy(p => p.ProjectNumber).ToList();
                    break;
            }

            return projects;
        }
    }
}