using NHibernate;
using Repositories.Interfaces;
using Repositories.Models;
using System.Collections.Generic;

namespace Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private IUnitOfWork unitOfWork;

        public EmployeeRepository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IList<EMPLOYEE> FindAllEmployees()
        {
            return unitOfWork.Session.QueryOver<EMPLOYEE>().List();
        }
        public IList<EMPLOYEE> FindEmployeesByVisas(List<string> visas)
        {
            return unitOfWork.Session.QueryOver<EMPLOYEE>()
                    .WhereRestrictionOn(e => e.VISA).IsIn(visas)
                    .List();
        }
    }
}
