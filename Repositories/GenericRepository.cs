using NHibernate;
using Repositories.Interfaces;

namespace Repositories
{
    public class GenericRepository : IGenericRepository
    {
        private IUnitOfWork unitOfWork;

        public GenericRepository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void Delete(object o)
        {
            unitOfWork.Session.Delete(o);
        }

        public object Save(object o)
        {
            return unitOfWork.Session.Save(o);
        }

        public void Update(object o)
        {
            unitOfWork.Session.Lock(o, LockMode.UpgradeNoWait);
            unitOfWork.Session.Update(o);
        }
    }
}
