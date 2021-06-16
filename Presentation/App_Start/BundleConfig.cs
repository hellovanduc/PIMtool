using System.Web.Optimization;

namespace PIMTool
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //  JQuery
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            //  Bootstrap
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"
                      , "~/Scripts/popper.js"));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                      "~/Content/bootstrap/bootstrap.css"));

            //  Custom
            bundles.Add(new ScriptBundle("~/bundles/projectCreate").Include(
                "~/Scripts/custom/project_create.js"));
            bundles.Add(new ScriptBundle("~/bundles/projectList").Include(
                "~/Scripts/custom/project_list.js"));

            bundles.Add(new StyleBundle("~/Content/custom").Include(
                      "~/Content/custom/common.css",
                      "~/Content/custom/layout.css",
                      "~/Content/custom/project_create.css",
                      "~/Content/custom/project_list.css"));

            //  Select2
            bundles.Add(new StyleBundle("~/Content/select2").Include(
                "~/Content/select2/select2.css",
                "~/Content/select2/select2.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/select2").Include(
                "~/Scripts/select2.js",
                "~/Scripts/select2.min.js"));

            //  PagedList
            bundles.Add(new StyleBundle("~/Content/PagedList").Include(
                "~/Content/PagedList.css"));
        }
    }
}
