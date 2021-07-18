using Repositories.Models;
using System;

namespace Repositories
{
    public class UnitOfWork : IDisposable
    {
        private PIMdbEntities _context = new PIMdbEntities();
        private bool isDisposed = false;
        private GenericRepository<PROJECT> _projectRepository;
        private GenericRepository<GROUP> _groupRepository;
        private GenericRepository<EMPLOYEE> _employeeRepository;

        private PIMdbEntities Context { 
            get {
                if (isDisposed)
                {
                    _context = new PIMdbEntities();
                }
                return _context; 
            } 
        }

        public GenericRepository<PROJECT> ProjectRepository
        {
            get
            {
                if (_projectRepository == null)
                {
                    _projectRepository = new GenericRepository<PROJECT>(Context);
                }
                return _projectRepository;
            }
        }

        public GenericRepository<EMPLOYEE> EmployeeRepository
        {
            get
            {

                if (_employeeRepository == null)
                {
                    _employeeRepository = new GenericRepository<EMPLOYEE>(Context);
                }
                return _employeeRepository;
            }
        }

        public GenericRepository<GROUP> GroupRepository
        {
            get
            {
                if (_groupRepository == null)
                {
                    _groupRepository = new GenericRepository<GROUP>(Context);
                }
                return _groupRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
            isDisposed = true;
        }
    }
}
