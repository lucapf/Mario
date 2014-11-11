using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mediatori.Controllers
{
    public class PraticheController : Controller
    {

        private mediatori.Models.MainDbContext db = new mediatori.Models.MainDbContext();

        public ActionResult Index(Models.Pratica.SearchPratica model)
        {
             model.Pratiche = db.pratiche.Include("Contatto").Include("prodottoRichiesto") ;
           
            return View(model);
        }



        public ActionResult Details(int id)
        {
            mediatori.Models.Pratica.Pratica model;
            model = db.pratiche.Where(p => p.id == id).FirstOrDefault();

            if (model == null)
            {
                return HttpNotFound();
            }


            //mediatori.Controllers.SegnalazioniController.setSegnalazioneCreateModel ()

            return View(model);
        }

    }
}