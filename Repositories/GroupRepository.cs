using NHibernate;
using Repositories.Interfaces;
using Repositories.Models;
using System.Collections.Generic;


namespace Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private IUnitOfWork unitOfWork;

        public GroupRepository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IList<GROUPS> FindAllGroups()
        {
            var groups = unitOfWork.Session.QueryOver<GROUPS>().List();

            foreach (var group in groups)
            {
                group.GROUP_LEADER = null;
            }

            return groups;
        }
        public GROUPS FindGroupByName(string name)
        {
            return unitOfWork.Session.QueryOver<GROUPS>()
                    .Where(g => g.NAME == name)
                    .SingleOrDefault();
        }
    }
}
