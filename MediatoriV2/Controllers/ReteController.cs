using mediatori.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mediatori.Controllers
{
    public class ReteController : MyBaseController
    {
        //
        // GET: /Rete/

        public ActionResult Create()
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            valorizzaViewBag(db);
            return View("Manage");
        }
        private void valorizzaViewBag(MainDbContext db)
        {
            ViewBag.listaAgenzia = db.Agenzia.Include("soggettoGiuridico").ToList(); 
        }

    }
}
