using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PIMTool.ViewModels
{
    public class CreateProjectViewModel
    {
        [Display(Name = "ProjectNumber", ResourceType = typeof(Resources.Resources.Resources.Resources))]
        [Required(ErrorMessageResourceName = "ProjectNumberRequireError", ErrorMessageResourceType = typeof(Resources.Resources.Resources.Resources))]
        [Range(1000, 9999, ErrorMessageResourceName = "ProjectNumberRangeError", ErrorMessageResourceType = typeof(Resources.Resources.Resources.Resources))]
        [Remote("IsProjectNumberExist", "api", AdditionalFields = "EditMode")]
        [DisplayFormat(DataFormatString = "{0:#.#}", ApplyFormatInEditMode = true)]
        public int PROJECT_NUMBER { get; set; }

        [Display(Name = "ProjectName", ResourceType = typeof(Resources.Resources.Resources.Resources))]
        [Required(ErrorMessageResourceName = "ProjectNameRequireError", ErrorMessageResourceType = typeof(Resources.Resources.Resources.Resources))]
        [StringLength(50, ErrorMessageResourceName = "NameTooLongError", ErrorMessageResourceType = typeof(Resources.Resources.Resources.Resources))]
        public string NAME { get; set; }

        [Display(Name = "Customer", ResourceType = typeof(Resources.Resources.Resources.Resources))]
        [Required(ErrorMessageResourceName = "CutomerRequireError", ErrorMessageResourceType = typeof(Resources.Resources.Resources.Resources))]
        [StringLength(50, ErrorMessageResourceName = "CustomerTooLongError", ErrorMessageResourceType = typeof(Resources.Resources.Resources.Resources))]
        public string CUSTOMER { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Resources.Resources.Resources.Resources))]
        [Required(ErrorMessageResourceName = "StatusRequireError", ErrorMessageResourceType = typeof(Resources.Resources.Resources.Resources))]
        public string STATUS { get; set; }

        public SelectList AllStatus { get; set; }

        [Display(Name = "StartDate", ResourceType = typeof(Resources.Resources.Resources.Resources))]
        [Required(ErrorMessageResourceName = "StartDateRequireError", ErrorMessageResourceType = typeof(Resources.Resources.Resources.Resources))]
        [DataType(DataType.Date, ErrorMessageResourceName = "DateOnlyError", ErrorMessageResourceType = typeof(Resources.Resources.Resources.Resources))]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime START_DATE { get; set; }


        [Display(Name = "EndDate", ResourceType = typeof(Resources.Resources.Resources.Resources))]
        [DataType(DataType.Date, ErrorMessageResourceName = "DateOnlyError", ErrorMessageResourceType = typeof(Resources.Resources.Resources.Resources))]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? END_DATE { get; set; }

        [Display(Name = "Members", ResourceType = typeof(Resources.Resources.Resources.Resources))]
        public List<string> MEMBERS { get; set; }

        public MultiSelectList AllEmployees { get; set; }

        [Display(Name = "Group", ResourceType = typeof(Resources.Resources.Resources.Resources))]
        [Required(ErrorMessageResourceName = "GroupRequireError", ErrorMessageResourceType = typeof(Resources.Resources.Resources.Resources))]
        public string GROUP { get; set; }

        public SelectList AllGroups { get; set; }

        public int VERSION { get; set; }

        public bool EditMode { get; set; }
        public string Error { get; set; }
    }
}