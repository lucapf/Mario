using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mediatori.Controllers
{
    public class ReportsController : MyBaseController
    {
       
        public ActionResult Index()
        {
            return View();
        }
    }
}