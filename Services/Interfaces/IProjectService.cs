using Repositories.Models;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IProjectService
    {
        /// <summary>
        /// Insert the project into the database
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        PROJECT CreateProject(PROJECT project);

        /// <summary>
        /// 
        /// </summary>
        /// <returns>List all projects in the database</returns>
        IList<PROJECT> FindAllProjects();

        /// <summary>
        /// Find all the projects which match searchString and projectStatus condition, sort by sortOrder (default is sort by PROJECT_NUMBER)
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="projectStatus"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        IList<PROJECT> FindAllProjects(string searchString, string projectStatus, string sortOrder);

        /// <summary> 
        /// </summary>
        /// <returns>List all employees in the database</returns>
        IList<EMPLOYEE> FindAllEmployees();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="visas"></param>
        /// <returns>The employees corresponding with the visas in list</returns>
        IList<EMPLOYEE> FindEmployeesByVisas(List<string> visas);

        /// <summary>
        /// </summary>
        /// <returns>List all groups in the database</returns>
        IList<GROUPS> FindAllGroups();

        /// <summary>
        /// </summary>
        /// <param name="name">Name of the group need to be founded</param>
        /// <returns>The group if founded, else null</returns>
        GROUPS FindGroupByName(string name);

        /// <summary>
        /// </summary>
        /// <param name="projectNumber"></param>
        /// <returns>null if the project which has projectNumber is not found, else return the founded project</returns>
        PROJECT FindProjectByProjectNumber(int projectNumber);

        /// <summary>
        /// Update the project which corresponding project number by newProject
        /// </summary>
        /// <param name="projectNumber"></param>
        /// <param name="newProject"></param>
        /// <returns>
        /// The project after updating if the project is updated successfully.
        /// null if the project which has projectNumber is not found
        /// </returns>
        PROJECT UpdateProjectByProjectNumber(int projectNumber, PROJECT newProject);

        /// <summary>
        /// Delete one or more projects corresponding with PROJECT_NUMBER in the list
        /// </summary>
        /// <param name="projectNumber"></param>
        /// <returns>The number of rows are deleted</returns>
        int DeleteProjectsByProjectNumber(List<int> projectNumbers);
    }
}
