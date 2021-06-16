using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PIMTool
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            InjectorConfig.RegisterComponents();

            //  Localizing default error messages in ASP.NET MVC
            ClientDataTypeModelValidatorProvider.ResourceClassKey = "Resources.Resources";
            DefaultModelBinder.ResourceClassKey = "Resources.Resources";

        }
    }
}
