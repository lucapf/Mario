using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Web.Mvc;

namespace mediatori.Controllers
{
    public class MyBaseController : Controller
    {


        protected override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {


            if (filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                base.OnAuthorization(filterContext);
            }
            else
            {


                // MyHelper.printRequest(Request);

                Debug.WriteLine("MyBaseController.OnAuthorization " + System.Web.HttpContext.Current.User.Identity.IsAuthenticated);

                //if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated == false && (User.Identity.GetType() != typeof(System.Security.Principal.WindowsIdentity)))
                if (Session["MySessionData"] == null || (System.Web.HttpContext.Current.User.Identity.IsAuthenticated == false && (User.Identity.GetType() != typeof(System.Security.Principal.WindowsIdentity))))
                {
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