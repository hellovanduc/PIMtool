using System.Web.Mvc;
using System.Web.Routing;

namespace PIMTool
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{culture}/{controller}/{action}/{id}",
                defaults: new { culture = "en", controller = "Project", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
