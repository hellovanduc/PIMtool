using log4net;
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

            container.RegisterType<IProjectService, ProjectService>();

            ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            container.RegisterInstance(log);

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}