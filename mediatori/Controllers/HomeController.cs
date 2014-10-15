using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mediatori.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "FireAnt la soluzione per i mediatori creditizi";
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("../Account/Login");
            }
            else
            {
                return View();
            }

            
        }
        public ActionResult Anagrafiche()
        {
            return View();
        }

        public ActionResult Calendar()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Pagina di descrizione dell'app.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Pagina di contatto.";

            return View();
        }
    }
}
