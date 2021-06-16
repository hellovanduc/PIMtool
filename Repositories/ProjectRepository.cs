using NHibernate;
using NHibernate.Criterion;
using Repositories.Enums;
using Repositories.Interfaces;
using Repositories.Models;
using System.Collections.Generic;

namespace Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private IUnitOfWork unitOfWork;

        public ProjectRepository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public int DeleteProjectsByProjectNumbers(List<int> projectNumbers)
        {
            var projects = unitOfWork.Session.QueryOver<PROJECT>()
                    .WhereRestrictionOn(p => p.PROJECT_NUMBER).IsIn(projectNumbers)
                    .List();

            foreach (var project in projects)
            {
                unitOfWork.Session.Delete(project);
            }

            return projects.Count;
        }

        public IList<PROJECT> FindAllProjects()
        {
            return unitOfWork.Session.QueryOver<PROJECT>().List();
        }

        public IList<PROJECT> FindAllProjects(string searchString, string projectStatus, int? projectNumber)
        {
            IList<PROJECT> projects;

            if (projectNumber != null)
            {
                projects = unitOfWork.Session.QueryOver<PROJECT>()
                .Where(p =>
                            (p.NAME.IsLike($"%{searchString}%") ||
                                p.CUSTOMER.IsLike($"%{searchString}%") ||
                                p.PROJECT_NUMBER == projectNumber)
                                &&
                                (projectStatus == "" ||
                                p.STATUS == StatusHelper.StringToStatus(projectStatus))
                    ).List();
            }
            else
            {
                projects = unitOfWork.Session.QueryOver<PROJECT>()
                .Where(p =>
                            (p.NAME.IsLike($"%{searchString}%") ||
                                p.CUSTOMER.IsLike($"%{searchString}%"))
                                &&
                                (projectStatus == "" ||
                                p.STATUS == StatusHelper.StringToStatus(projectStatus))
                    ).List();
            }

            foreach (var project in projects)
            {
                project.EMPLOYEES = null;
                project.GROUP = null;
            }

            return projects;
        }

        public PROJECT FindProjectByProjectNumber(int projectNumber)
        {
            var project = unitOfWork.Session.QueryOver<PROJECT>()
                .Where(p => p.PROJECT_NUMBER == projectNumber)
                .SingleOrDefault();

            //  Load the EMPLOYEES and GROUP because they are set to lazy loading,
            //      so we need to load them inside this session to avoid LazyInitalizationException
            //      (because we need to use them outside of this session)
            if (project != null)
            {
                NHibernateUtil.Initialize(project.EMPLOYEES);
                NHibernateUtil.Initialize(project.GROUP);
            }

            return project;
        }
    }
}