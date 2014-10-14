using System.Web;
using System.Web.Optimization;

namespace mediatori
{
    public class BundleConfig
    {
        // Per ulteriori informazioni sul Bundling, visitare il sito Web all'indirizzo http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js", 
                        "~/Scripts/jquery-{version}.min.js", 
                        "~/Scripts/jquery-{version}.intellisense.js",
                        "~/Scripts/jquery-{version}.min.map", "~/Scripts/jquery.form*"));
            bundles.Add(new ScriptBundle("~/bundles/scheduler")
                .Include("~/Scripts/dhtmlxscheduler/dhtmlxscheduler.js",
                "~/Scripts/dhtmlxscheduler/locale/locale_it.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js", "~/Scripts/jquery-ui-{version}.min.js"));
           
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*","~/Scripts/jquery.unobtrusive-ajax.min.js",
                        "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/fireAntUtils").Include(
                        "~/Scripts/fireAntUtils.js"));

            // Utilizzare la versione di sviluppo di Modernizr per eseguire attività di sviluppo e formazione. Successivamente, quando si è
            // pronti per passare alla produzione, utilizzare lo strumento di compilazione disponibile all'indirizzo http://modernizr.com per selezionare solo i test necessari.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));
            //bundles.Add(new StyleBundle("~/Content/css/scheduler").Include("~/Content/dhtmlxscheduler/dhtmlxscheduler.css"));
            bundles.Add(new StyleBundle("~/Content/css/scheduler").Include("~/Content/dhtmlxscheduler/dhtmlxscheduler_glossy.css"));
            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                         "~/Content/themes/base/theme.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.spinner.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                         "~/Content/themes/base/selectmenu.css",
                        "~/Content/themes/base/jquery.ui.all.css"));
        }
    }
}