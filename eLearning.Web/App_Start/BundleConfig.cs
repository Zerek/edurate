﻿using System.Web;
using System.Web.Optimization;

namespace edurate.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*", 
                        "~/Scripts/jquery.validate*"));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/scaffold").Include(
                        "~/Scripts/scaffold/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/scaffold").Include(
                        "~/Content/themes/scaffold/css/bootstrap.css",
                        "~/Content/themes/scaffold/css/bootstrap-responsive.css",
                        "~/Content/themes/scaffold/css/docs.css",
                        "~/Content/themes/scaffold/css/style.css",
                        "~/Content/themes/scaffold/css/color.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new StyleBundle("~/Content/themes/admin/css").Include(
                        "~/Content/themes/admin/bootstrap/css/bootstrap.min.css",
                        "~/Content/themes/admin/css/metro.css",
                        "~/Content/themes/admin/bootstrap/css/bootstrap-responsive.min.css",
                        "~/Content/themes/admin/font-awesome/css/font-awesome.css", 
                        "~/Content/themes/admin/css/style.css",
                        "~/Content/themes/admin/css/style_responsive.css", 
                        "~/Content/themes/admin/css/style_default.css", 
                        "~/Content/themes/admin/fancybox/source/jquery.fancybox.css", 
                        "~/Content/themes/admin/gritter/css/jquery.gritter.css", 
                        "~/Content/themes/admin/uniform/css/uniform.default.css", 
                        "~/Content/themes/admin/bootstrap-daterangepicker/daterangepicker.css", 
                        "~/Content/themes/admin/fullcalendar/fullcalendar/bootstrap-fullcalendar.css"
                ));
            bundles.Add(new ScriptBundle("~/Content/themes/admin/js").Include(
                        "~/Content/themes/admin/js/jquery-1.8.3.min.js",
                        "~/Content/themes/admin/breakpoints/breakpoints.js",
                        "~/Content/themes/admin/jquery-slimscroll/jquery-ui-1.9.2.custom.min.js",
                        "~/Content/themes/admin/jquery-slimscroll/jquery.slimscroll.min.js",
                        "~/Content/themes/admin/fullcalendar/fullcalendar/fullcalendar.min.js", 
                        "~/Content/themes/admin/bootstrap/js/bootstrap.min.js", 
                        "~/Content/themes/admin/js/jquery.blockui.js", 
                        "~/Content/themes/admin/js/jquery.cookie.js",
                        "~/Content/themes/admin/js/app.js"
                ));

        }
    }
}