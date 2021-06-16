using NHibernate;
using System;

namespace Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Return the session which the unit of work control
        /// </summary>
        ISession Session { get; }
        /// <summary>
        /// Start UnitOfWork (Open a new session if CurrentSession is null or is closed, begin Transaction)
        /// </summary>
        /// <returns></returns>
        IUnitOfWork Start();
        /// <summary>
        /// Flush the session and commit the transaction
        /// </summary>
        void Commit();
    }
}
