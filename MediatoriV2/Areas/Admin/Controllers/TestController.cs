using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWebApplication.Areas.Admin.Controllers
{
    [MyAuthorize(Roles = "ADMIN")]
    public class TestController : MyBaseController
    {
        //
        // GET: /Admin/Test/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Tabs()
        {
            return View();
        }

    }
}
