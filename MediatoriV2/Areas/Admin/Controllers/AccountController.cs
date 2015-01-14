using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MyUsers;
using System.Web.Security;
using System.Diagnostics;
using mediatori.Areas.Admin.Models;

namespace MyWebApplication.Areas.Admin.Controllers
{
   
    public class AccountController : MyBaseController
    {
     
        private MyUsers.UserManager manager = new MyUsers.UserManager("DefaultConnection");

        //[Authorize]
        //[MyAuthorize(Roles = "Administrators")]
        //public ActionResult Manage()
        //{
        //    MyWebApplication.Areas.Admin.Models.UserProfile model = new MyWebApplication.Areas.Admin.Models.UserProfile();
        //    manager.openConnection();

        //    long userId = -1;

        //    try
        //    {
        //        if (User.Identity is System.Security.Principal.WindowsIdentity)
        //        {
        //            userId = manager.getUserIdFromSID(new System.Security.Principal.SecurityIdentifier((User.Identity as System.Security.Principal.WindowsIdentity).User.Value));
        //        }
        //        else if (User.Identity is MyUsers.MyCustomIdentity)
        //        {
        //            userId = (User.Identity as MyCustomIdentity).UserId;
        //        }
        //        else if (User.Identity is System.Web.Security.FormsIdentity)
        //        {
        //            userId = (Session["MySessionData"] as MyManagerCSharp.MySessionData).UserId;
        //        }

        //        MyWebApplication.Controllers.AccountController.setUserProfileModel(model, userId);
        //    }
        //    finally
        //    {
        //        manager.closeConnection();
        //    }
        //    return View(model);
        //}



        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            LogOnModel model = new LogOnModel();
            model.Password = "";
            return View(model);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LogOnModel model, string returnUrl)
        {
            TempData["AREA"] = "Admin";

            mediatori.Controllers.AccountController controller = new mediatori.Controllers.AccountController();
            controller.ControllerContext = ControllerContext;
            controller.TempData = TempData;

            mediatori.Models.LogOnModel model2 = new mediatori.Models.LogOnModel();
            model2.Password = model.Password;
            model2.UserName = model.UserName;
            model2.RememberMe = model.RememberMe;

            return controller.Login(model2, returnUrl);
           // return View(model);
        }


        //
        // POST: /Account/LogOff

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            TempData["AREA"] = "Admin";

            //MyWebApplication.Controllers.AccountController controller = new MyWebApplication.Controllers.AccountController();
            //controller.ControllerContext = ControllerContext;
            //controller.TempData = TempData;
            //return controller.LogOff();

            FormsAuthentication.SignOut();

            MyManagerCSharp.Log.LogUserManager log = new MyManagerCSharp.Log.LogUserManager("DefaultConnection");
            log.openConnection();
            try
            {
                string ip = Request.UserHostAddress;
                log.insert((Session["MySessionData"] as MyManagerCSharp.MySessionData).UserId, MyManagerCSharp.Log.LogUserManager.LogType.Logout, ip);
            }
            catch (Exception ex)
            {
                //potrebbe esserci un errore in quanto in fase di sviluppo si memorizza un id di sessione diverso
                //il problema si presenta ad esemoio se si cambia il puntamento al DB

                //ignoro l'errore
                Debug.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                log.closeConnection();
            }

            //Session["MySessionData"] = null;
            (Session["MySessionData"] as MyManagerCSharp.MySessionData).LogOff();


            if (TempData["AREA"] == "Mobile")
            {
                return RedirectToAction("Index", "Mobile", new { area = "Mobile" });
            }

            if (TempData["AREA"] == "Admin")
            {
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }


            return RedirectToAction("Index", "Home");

       
        }


    }
}
