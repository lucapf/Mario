using mediatori.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mediatori.Controllers
{
    public class HomeController : MyBaseController
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Anagrafiche()
        {
            return View();
        }


        public ActionResult Sicurezza()
        {
            return View();
        }

        public ActionResult Configurazioni()
        {
            return View();
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        

        public ActionResult Calendar()
        {
            return View();
        }

        public ActionResult Assegnazioni(mediatori.Models.AssegnazioniModel model)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);

            // model.DaAssegnare = db.Segnalazioni.Include("contatto").Include("prodottoRichiesto").ToList();

            //List<mediatori.Models.etc.GruppoLavorazione> gruppi = db.gruppiLavorazione.Where ( p => p.utenti.Contains (""User.Identity.Name

            model.DaAssegnare = (from s in db.Segnalazioni.Include("stato").Include("contatto").Include("prodottoRichiesto")
                                 where !(
                                 from a in db.Assegnazioni where a.segnalazioneId == s.id && a.statoId == s.stato.id select a.segnalazioneId
                                 ).Contains(s.id) && s.stato.gruppoLavorazione.utenti.Contains(";" + User.Identity.Name + ";")
                                 select s).ToList();


            //  model.Assegnate = db.Assegnazioni.ToList();

            //model.Assegnate = (from s in db.Segnalazioni.Include("stato")
            //                   join a in db.Assegnazioni.Include("Segnalazione").Include("Segnalazione.contatto") on s.id equals a.segnalazioneId
            //                  where s.stato.id == a.statoId
            //                  select a).ToList();

            model.Assegnate = (from a in db.Assegnazioni.Include("Segnalazione").Include("Segnalazione.contatto").Include("Segnalazione.stato").Include("Segnalazione.prodottoRichiesto")
                               where a.segnalazione.stato.id == a.statoId && a.stato.gruppoLavorazione.utenti.Contains(";" + User.Identity.Name + ";")
                               select a).ToList();



            if (Request.IsAjaxRequest())
            {
                List<mediatori.Models.MyItem> risultato = new List<MyItem>();
                risultato.Add(new MyItem(model.DaAssegnare.Count.ToString(), "Da_assegnare"));
                risultato.Add(new MyItem(model.Assegnate.Count.ToString(), "Assegnate"));

                return Json(risultato, JsonRequestBehavior.AllowGet);

            }

            return View(model);
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
