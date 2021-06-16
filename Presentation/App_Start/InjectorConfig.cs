using log4net;
using Repositories;
using Repositories.Interfaces;
using Services;
using Services.Interfaces;
using System.Web.Mvc;
using Unity;
using Unity.AspNet.Mvc;

namespace PIMTool
{
    public class InjectorConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterSingleton<IUnitOfWork, UnitOfWork>();
            container.RegisterSingleton<IProjectService, ProjectService>();

            #region Register singleton for repositories
            container.RegisterSingleton<IGenericRepository, GenericRepository>();
            container.RegisterSingleton<IProjectRepository, ProjectRepository>();
            container.RegisterSingleton<IEmployeeRepository, EmployeeRepository>();
            container.RegisterSingleton<IGroupRepository, GroupRepository>();
            #endregion

            ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            container.RegisterInstance(log);

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}