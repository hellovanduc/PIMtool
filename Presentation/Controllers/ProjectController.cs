using PagedList;
using PIMTool.ViewModels;
using Repositories.Enums;
using Repositories.Models;
using Resources.Constants;
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

            ViewBag.Title = $"{Resources.Resources.Resources.Resources.TitleListProject} | PIM tool";
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
            ViewBag.Title = $"{Resources.Resources.Resources.Resources.TitleCreateProject} | PIM tool";

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
                Project project = new Project
                {
                    ProjectNumber = viewModel.PROJECT_NUMBER,
                    Customer = viewModel.CUSTOMER,
                    Name = viewModel.NAME,
                    StartDate = viewModel.START_DATE,
                    EndDate = viewModel.END_DATE,
                    Group = service.FindGroupByName(viewModel.GROUP),
                    Employees = (viewModel.MEMBERS == null) ? null : service.FindEmployeesByVisas(viewModel.MEMBERS).ToHashSet(),
                    Status = viewModel.STATUS
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
            ViewBag.Title = $"{Resources.Resources.Resources.Resources.TitleEditProject} | PIM tool";
            ViewBag.Groups = service.FindAllGroups();
            ViewBag.Employees = service.FindAllEmployees();

            Project project = service.FindProjectByProjectNumber(id);
            CreateProjectViewModel viewModel = new CreateProjectViewModel
            {
                PROJECT_NUMBER = project.ProjectNumber,
                CUSTOMER = project.Customer,
                NAME = project.Name,
                START_DATE = project.StartDate,
                END_DATE = project.EndDate,
                STATUS = project.Status.ToString(),
                GROUP = project.Group.Name,
                MEMBERS = project.Employees.Select(m => m.Visa).ToList(),
                AllEmployees = CreateMultiSelectListEmployees(),
                AllGroups = CreateSelectListGroup(),
                AllStatus = CreateSelectListStatus(),
                EditMode = true,
                VERSION = project.Version
            };

            return View("Create", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PROJECT_NUMBER,NAME,CUSTOMER,GROUP,MEMBERS,STATUS,START_DATE,END_DATE,VERSION")] CreateProjectViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Project project = new Project
                {
                    ProjectNumber = viewModel.PROJECT_NUMBER,
                    Customer = viewModel.CUSTOMER,
                    Name = viewModel.NAME,
                    StartDate = viewModel.START_DATE,
                    EndDate = viewModel.END_DATE,
                    Group = service.FindGroupByName(viewModel.GROUP),
                    Employees = (viewModel.MEMBERS == null) ? null : service.FindEmployeesByVisas(viewModel.MEMBERS).ToHashSet(),
                    Status = viewModel.STATUS,
                    Version = viewModel.VERSION
                };

                service.UpdateProjectByProjectNumber(project.ProjectNumber, project);

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
                Value = e.Visa,
                Text = $"{e.Visa}: {e.FirstName} {e.LastName}"
            }));

            return new MultiSelectList(allEmployees.OrderBy(e => e.Text), "Value", "Text");
        }

        private SelectList CreateSelectListGroup()
        {
            IEnumerable<SelectListItem> allGroups = new List<SelectListItem>(service.FindAllGroups().Select(g => new SelectListItem
            {
                Value = g.Name,
                Text = $"{g.Name}"
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
                        errors.Append(error.ErrorMessage + Constants.Semicolon);
                    }
                }
            }
            return errors.ToString();
        }
    }
}