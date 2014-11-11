using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Diagnostics;

namespace mediatori
{
    // Nota: per istruzioni su come abilitare la modalità classica di IIS6 o IIS7, 
    // visitare il sito Web all'indirizzo http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();


            ModelBinders.Binders.Add(typeof(double), new DoubleModelBinder());
            ModelBinders.Binders.Add(typeof(double?), new DoubleModelBinder());

            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
            ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());
        }



        protected void Session_Start(object sender, EventArgs e)
        {
            Debug.WriteLine("Session_Start");

        }


        protected void Application_Error(Object sender, EventArgs e)
        {
            Exception objError;
            objError = Server.GetLastError();
            if (objError != null)
            {
                Debug.WriteLine("Application_Error: " + objError.GetType().Name + " Message:" + objError.Message);

                //switch (objError.GetType().Name)
                //{
                //    case "MyException":
                //        Server.TransferRequest("~/Home/Error?MyError=" + Server.UrlEncode(objError.Message));
                //        break;
                //    case "HttpAntiForgeryException":
                //        Server.TransferRequest("~/Account/Login");
                //        break;
                //}
            }
        }
    }
}