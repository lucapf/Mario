using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Security;

namespace mediatori.Controllers
{
    public class MyBaseController : Controller
    {
        protected mediatori.Models.MainDbContext db = new mediatori.Models.MainDbContext();

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        protected override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {


            if (filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                base.OnAuthorization(filterContext);
            }
            else
            {

                if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    //provo a ricavare i dati per la sessione 
                    // System.Web.HttpContext.Current.User.Identity.Name

                }


                // MyHelper.printRequest(Request);

                //  Debug.WriteLine("MyBaseController.OnAuthorization " + System.Web.HttpContext.Current.User.Identity.IsAuthenticated);

                //if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated == false && (User.Identity.GetType() != typeof(System.Security.Principal.WindowsIdentity)))
                if (Session["MySessionData"] == null || (System.Web.HttpContext.Current.User.Identity.IsAuthenticated == false && (User.Identity.GetType() != typeof(System.Security.Principal.WindowsIdentity))))
                {
                    if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        //Cerco di ripopolare la sessone

                        long userId;

                        MyUsers.UserManager manager = new MyUsers.UserManager("DefaultConnection");
                        userId = manager.getUserIdFromLogin(System.Web.HttpContext.Current.User.Identity.Name);

                        if (userId != -1)
                        {
                            mediatori.SessionData session = new mediatori.SessionData(userId);
                            session.Roles = manager.getRoles(userId);
                            session.Profili = manager.getProfili(userId);
                            session.Groups = manager.getGroupSmall(userId);

                            Session["MySessionData"] = session;

                            //if (!String.IsNullOrEmpty(Request["ReturnUrl"]))
                            //{
                              //  filterContext.Result = Redirect(Request["ReturnUrl"]);
                                return;
//                            }

                            
                        }
                        else
                        {
                            FormsAuthentication.SignOut();
                        }


                    }

                    filterContext.Result = new RedirectToRouteResult(
                          new System.Web.Routing.RouteValueDictionary(
                              new
                              {

                                  controller = "Account",
                                  action = "Login",
                                  ReturnUrl = filterContext.HttpContext.Request.RawUrl
                              }));

                }
            }




        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Debug.WriteLine("MyBaseController.OnException");

            if (filterContext.Exception != null)
            {
                Debug.WriteLine("MyBaseController.OnException: " + filterContext.Exception.Message);

                MyManagerCSharp.Log.LogManager log = new MyManagerCSharp.Log.LogManager("DefaultConnection");
                log.openConnection();
                try
                {
                    log.exception("MyBaseController", filterContext.Exception);
                }
                finally
                {
                    log.closeConnection();
                }

            }

            if (filterContext.Exception is System.Web.Mvc.HttpAntiForgeryException)
            {
                filterContext.Result = new ViewResult { ViewName = "~/Account/Login" };
            }
            else
            {

                filterContext.Result = new ViewResult { ViewName = "~/Views/Home/Error" };
            }


        }
    }
}