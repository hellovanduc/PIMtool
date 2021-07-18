using Repositories.Models;
using Services.Interfaces;
using System.Web.Mvc;

namespace PIMTool.Controllers
{
    public class APIController : BaseController
    {
        private IProjectService service;
        public APIController(IProjectService projectService)
        {
            this.service = projectService;
        }
        public JsonResult IsProjectNumberExist(int PROJECT_NUMBER, bool EditMode)
        {
            if (EditMode)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            PROJECT project = service.FindProjectByProjectNumber(PROJECT_NUMBER);
            if (project != null)
            {
                return Json(Resources.Resources.Resources.Resources.ProjectNumberAlreadyExist, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
