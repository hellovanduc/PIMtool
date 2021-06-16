using Repositories.Models;
using System.Collections.Generic;

namespace Repositories.Interfaces
{
    public interface IProjectRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectNumber"></param>
        /// <returns>if the project which has projectNumber is founded, return the project, else null</returns>
        PROJECT FindProjectByProjectNumber(int projectNumber);
        /// <summary>
        /// 
        /// </summary>
        /// <returns>All the projects in the database</returns>
        IList<PROJECT> FindAllProjects();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchString">PROJECT_NAME or CUSTOMER</param>
        /// <param name="projectStatus"></param>
        /// <param name="projectNumber">PROJECT_NUMBER</param>
        /// <returns>All the projects in the database which corresponding with the searchString, projectNumber and the projectStatus</returns>
        IList<PROJECT> FindAllProjects(string searchString, string projectStatus, int? projectNumber = null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectNumbers">List the project numbers of the projects which need to be deleted</param>
        /// <returns>The number of projects has been deleted</returns>
        int DeleteProjectsByProjectNumbers(List<int> projectNumbers);
    }
}
