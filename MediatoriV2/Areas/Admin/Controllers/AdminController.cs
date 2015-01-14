using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace MyWebApplication.Areas.Admin.Controllers
{
    public class AdminController : MyBaseController
    {
        [MyAuthorize(Roles = "ADMINISTRATORS")]
        public ActionResult Index()
        {

            Models.ModelHome model = new Models.ModelHome();

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            model.version = fvi.FileVersion;

            //model.versionMVC = HttpContext.ApplicationInstance.GetType().Assembly.GetName().Version.ToString(); 
            model.versionMVC = typeof(System.Web.Mvc.Controller).Assembly.GetName().Version.ToString();

            //    System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); 

            return View(model);
        }


        public ActionResult Error()
        {
            Debug.WriteLine("MyError:" + TempData["MyError"]);

            if (TempData["MyError"] != null && !String.IsNullOrEmpty(TempData["MyError"].ToString()))
            {
                MyManagerCSharp.Log.LogManager log = new MyManagerCSharp.Log.LogManager("DefaultConnection");
                log.openConnection();
                try
                {
                    log.error(TempData["MyError"].ToString(), "AdminController.Error");
                }
                finally
                {
                    log.closeConnection();
                }
            }
            return View();
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult MD5(string textinput)
        {
            if (!String.IsNullOrEmpty(textinput))
            {
                ViewBag.valore = textinput;
                ViewBag.risultato = MyManagerCSharp.SecurityManager.getMD5Hash(textinput);
            }
            return View();
        }
    }
}
