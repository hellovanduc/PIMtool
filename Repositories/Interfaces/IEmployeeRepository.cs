using Repositories.Models;
using System.Collections.Generic;

namespace Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>All the employees in the database</returns>
        IList<EMPLOYEE> FindAllEmployees();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="visas">List the visas of the employees which need to be founded</param>
        /// <returns>All the employees are corresponding with the visa in the visas list</returns>
        IList<EMPLOYEE> FindEmployeesByVisas(List<string> visas);
    }
}
