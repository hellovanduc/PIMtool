using Library.Constants;
using Library.Resources.Resources;
using PagedList;
using PIMTool.ViewModels;
using Repositories.Enums;
using Repositories.Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace PIMTool.Controllers
{
    public class ProjectController : BaseController
    {
        private readonly IProjectService service;
        public ProjectController(IProjectService projectService)
        {
            this.service = projectService;
        }

        public ActionResult Index(string sortOrder, string searchString, string projectStatus, int? page)
        {
            searchString = searchString ?? "";
            projectStatus = projectStatus ?? "";

            var projects = service.FindAllProjects(searchString, projectStatus, sortOrder);

            int pageSize = 5;
            int pageNumber = page ?? 1;
            SelectList statusFilter = CreateSelectListStatus();
            if (projectStatus != "")
            {
                statusFilter.First(s => s.Value == projectStatus).Selected = true;
            }

            ViewBag.Title = $"{Resources.TitleListProject} | PIM tool";
            ViewBag.statusFilter = statusFilter;
            Session["sortOrder"] = sortOrder;
            Session["projectStatus"] = projectStatus;
            Session["searchString"] = searchString;
            Session.Timeout = 1;

            return View(projects.ToPagedList(pageNumber, pageSize));
        }
        // GET
        public ActionResult Create()
        {
            ViewBag.Title = $"{Resources.TitleCreateProject} | PIM tool";

            CreateProjectViewModel project = new CreateProjectViewModel
            {
                AllEmployees = CreateMultiSelectListEmployees(),
                AllGroups = CreateSelectListGroup(),
                AllStatus = CreateSelectListStatus(),
                STATUS = Status.NEW.ToString(),
                START_DATE = DateTime.Today,
                EditMode = false
            };

            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PROJECT_NUMBER,NAME,CUSTOMER,GROUP,STATUS,MEMBERS,START_DATE,END_DATE")] CreateProjectViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                PROJECT project = new PROJECT
                {
                    PROJECT_NUMBER = viewModel.PROJECT_NUMBER,
                    CUSTOMER = viewModel.CUSTOMER,
                    NAME = viewModel.NAME,
                    START_DATE = viewModel.START_DATE,
                    END_DATE = viewModel.END_DATE,
                    GROUP = service.FindGroupByName(viewModel.GROUP),
                    EMPLOYEES = (viewModel.MEMBERS == null) ? null : service.FindEmployeesByVisas(viewModel.MEMBERS).ToHashSet(),
                    STATUS = StatusHelper.StringToStatus(viewModel.STATUS)
                };
                service.CreateProject(project);

                var routes = new RouteValueDictionary();
                routes.Add("searchString", Session["searchString"]);
                routes.Add("projectStatus", Session["projectStatus"]);
                routes.Add("sortOrder", Session["sortOrder"]);

                return RedirectToAction("index", routes);
            }
            else
            {
                viewModel.Error = FindErrorInModel(ModelState);  
                viewModel.AllGroups = CreateSelectListGroup();
                viewModel.AllEmployees = CreateMultiSelectListEmployees();
                viewModel.AllStatus = CreateSelectListStatus();

                return View("Create", viewModel);
            }
        }

        public ActionResult Edit(int id)
        {
            ViewBag.Title = $"{Resources.TitleEditProject} | PIM tool";
            ViewBag.Groups = service.FindAllGroups();
            ViewBag.Employees = service.FindAllEmployees();

            PROJECT project = service.FindProjectByProjectNumber(id);
            CreateProjectViewModel viewModel = new CreateProjectViewModel
            {
                PROJECT_NUMBER = project.PROJECT_NUMBER,
                CUSTOMER = project.CUSTOMER,
                NAME = project.NAME,
                START_DATE = project.START_DATE,
                END_DATE = project.END_DATE,
                STATUS = project.STATUS.ToString(),
                GROUP = project.GROUP.NAME,
                MEMBERS = project.EMPLOYEES.Select(m => m.VISA).ToList(),
                AllEmployees = CreateMultiSelectListEmployees(),
                AllGroups = CreateSelectListGroup(),
                AllStatus = CreateSelectListStatus(),
                EditMode = true,
                VERSION = project.VERSION
            };

            return View("Create", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PROJECT_NUMBER,NAME,CUSTOMER,GROUP,MEMBERS,STATUS,START_DATE,END_DATE,VERSION")] CreateProjectViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                PROJECT project = new PROJECT
                {
                    PROJECT_NUMBER = viewModel.PROJECT_NUMBER,
                    CUSTOMER = viewModel.CUSTOMER,
                    NAME = viewModel.NAME,
                    START_DATE = viewModel.START_DATE,
                    END_DATE = viewModel.END_DATE,
                    GROUP = service.FindGroupByName(viewModel.GROUP),
                    EMPLOYEES = (viewModel.MEMBERS == null) ? null : service.FindEmployeesByVisas(viewModel.MEMBERS).ToHashSet(),
                    STATUS = StatusHelper.StringToStatus(viewModel.STATUS),
                    VERSION = viewModel.VERSION
                };

                service.UpdateProjectByProjectNumber(project.PROJECT_NUMBER, project);

                var routes = new RouteValueDictionary();
                routes.Add("searchString", Session["searchString"]);
                routes.Add("projectStatus", Session["projectStatus"]);
                routes.Add("sortOrder", Session["sortOrder"]);

                return RedirectToAction("Index", routes);
            }
            else
            {

                viewModel.Error = FindErrorInModel(ModelState);
                viewModel.AllGroups = CreateSelectListGroup();
                viewModel.AllEmployees = CreateMultiSelectListEmployees();
                viewModel.AllStatus = CreateSelectListStatus();
                viewModel.EditMode = true;

                return View("Create", viewModel);
            }
        }

        [HttpPost]
        public ActionResult Delete(string project_numbers)
        {
            List<int> project_numbers_to_delete = project_numbers.Split(Constants.Seperator)
                                                    .Select(int.Parse).ToList();

            service.DeleteProjectsByProjectNumber(project_numbers_to_delete);

            var routes = new RouteValueDictionary();
            routes.Add("searchString", Session["searchString"]);
            routes.Add("projectStatus", Session["projectStatus"]);
            routes.Add("sortOrder", Session["sortOrder"]);

            return RedirectToAction("Index", routes);
        }

        private MultiSelectList CreateMultiSelectListEmployees()
        {
            IEnumerable<SelectListItem> allEmployees = new List<SelectListItem>(service.FindAllEmployees().Select(e => new SelectListItem
            {
                Value = e.VISA,
                Text = $"{e.VISA}: {e.FIRST_NAME} {e.LAST_NAME}"
            }));

            return new MultiSelectList(allEmployees.OrderBy(e => e.Text), "Value", "Text");
        }

        private SelectList CreateSelectListGroup()
        {
            IEnumerable<SelectListItem> allGroups = new List<SelectListItem>(service.FindAllGroups().Select(g => new SelectListItem
            {
                Value = g.NAME,
                Text = $"{g.NAME}"
            }));

            return new SelectList(allGroups.OrderBy(g => g.Text), "Value", "Text");
        }
        private SelectList CreateSelectListStatus()
        {
            IEnumerable<SelectListItem> allStatus = new List<SelectListItem>(StatusHelper.GetAllStatus().Select(s => new SelectListItem
            {
                Value = s.ToString(),
                Text = StatusHelper.StatusText(s)
            }));

            return new SelectList(allStatus.OrderBy(g => StatusHelper.StringToStatus(g.Text)), "Value", "Text");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns>The error message contains all the error in the modelState</returns>
        private string FindErrorInModel(ModelStateDictionary modelState)
        {
            StringBuilder errors = new StringBuilder();
            foreach (var item in modelState)
            {
                if (item.Value.Errors.Count > 0)
                {
                    foreach (var error in item.Value.Errors)
                    {
                        errors.Append(error.ErrorMessage + Constants.Colon);
                    }
                }
            }
            return errors.ToString();
        }
    }
}