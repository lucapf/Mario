using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mediatori.Controllers
{
    public class PraticheController : MyBaseController
    {

        public ActionResult Index(Models.Pratica.SearchPratica model)
        {
            model.Pratiche = db.Pratiche.Include("cedente").Include("prodottoRichiesto");
            return View(model);
        }

        public ActionResult Details(int id)
        {
            mediatori.Models.Pratica.Pratica model;
            model = db.Pratiche.Include("cedente").Include("preventivi").Include("note").Where(p => p.id == id).FirstOrDefault();

            if (model == null)
            {
                return HttpNotFound();
            }



            return View(model);
        }

    }
}