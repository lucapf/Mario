using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace mediatori
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(DateTime?), new System.Web.Mvc.Html.CustomDateTimeModelBinder());
        }
    }
}
