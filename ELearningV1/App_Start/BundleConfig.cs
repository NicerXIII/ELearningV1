using System.Web;
using System.Web.Optimization;

namespace ELearningV1
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/font-awesome.min.css"));

            bundles.Add(new StyleBundle("~/admin-lte/css/css").Include(
                    "~/admin-lte/css/AdminLTE.min.css",
                    "~/admin-lte/css/skins/_all-skins.min.css"));


            bundles.Add(new StyleBundle("~/Ionicons/css/css").Include(
                    "~/Ionicons/css/ionicons.min.css"));


            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/admin-lte/js/adminlte.min.js").Include(
                    "~/admin-lte/js/adminlte.min.js"));

        }
    }
}
