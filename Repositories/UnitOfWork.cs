using Repositories.Models;
using System;

namespace Repositories
{
    public class UnitOfWork : IDisposable
    {
        private ProjectManagementContainer _context = new ProjectManagementContainer();
        private bool isDisposed = false;
        private GenericRepository<Project> _projectRepository;
        private GenericRepository<Group> _groupRepository;
        private GenericRepository<Employee> _employeeRepository;

        private ProjectManagementContainer Context { 
            get {
                if (isDisposed)
                {
                    _context = new ProjectManagementContainer();
                }
                return _context; 
            } 
        }

        public GenericRepository<Project> ProjectRepository
        {
            get
            {
                if (_projectRepository == null)
                {
                    _projectRepository = new GenericRepository<Project>(Context);
                }
                return _projectRepository;
            }
        }

        public GenericRepository<Employee> EmployeeRepository
        {
            get
            {

                if (_employeeRepository == null)
                {
                    _employeeRepository = new GenericRepository<Employee>(Context);
                }
                return _employeeRepository;
            }
        }

        public GenericRepository<Group> GroupRepository
        {
            get
            {
                if (_groupRepository == null)
                {
                    _groupRepository = new GenericRepository<Group>(Context);
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
