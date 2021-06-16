using Repositories.Models;
using System.Collections.Generic;

namespace Repositories.Interfaces
{
    public interface IGroupRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>All the groups in the database</returns>
        IList<GROUPS> FindAllGroups();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The group is corresponding with name</returns>
        GROUPS FindGroupByName(string name);
    }
}
