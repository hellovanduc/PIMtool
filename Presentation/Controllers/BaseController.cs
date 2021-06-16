using log4net;
using PIMTool.Helpers;
using Services.Exceptions;
using System;
using System.Threading;
using System.Web.Mvc;
using Unity;

namespace PIMTool.Controllers
{
    public class BaseController : Controller
    {
        [Dependency]
        public ILog Log { get; set; }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = RouteData.Values["culture"] as string;

            // Obtain culture from HTTP header AcceptLanguages
            if (cultureName == null)
            {
                cultureName = (Request.UserLanguages != null && Request.UserLanguages.Length > 0) ?
                        Request.UserLanguages[0] : null;
            }

            // Validate culture name
            cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return base.BeginExecuteCore(callback, state);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Exception e = filterContext.Exception;

            Log.Error(e.Message);

            ViewData["ErrorMsg"] = (e is CustomBaseException) ? e.Message : "";

            filterContext.Result = new ViewResult
            {
                ViewName = "Error",
                ViewData = this.ViewData
            };

            filterContext.ExceptionHandled = true;
            base.OnException(filterContext);
        }
    }
}