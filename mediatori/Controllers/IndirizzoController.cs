using mediatori.Controllers.Business;
using mediatori.Controllers.Business.Anagrafiche.Soggetto;
using mediatori.Controllers.CQS;
using mediatori.Models;
using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mediatori.Controllers
{
    public class IndirizzoController : Controller
    {
        //
        // GET: /Indirizzo/

        /*public ActionResult Index()
        {
            return View();
        }
         */
        [HttpGet]
        public ActionResult IndirizzoPartialById(int id, EnumTipoAzione tipoAzione = EnumTipoAzione.MODIFICA)
        {
            Indirizzo indirizzo = new Indirizzo { id = id };
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            valorizzaListeIndirizzoDetails(db);
            if (indirizzo.id > 0)
            {
                indirizzo = IndirizzoBusiness.findIndirizzo(id, db);
            }
            if (tipoAzione == EnumTipoAzione.MODIFICA)
            {
                valorizzaListeIndirizzoDetails(db);
                var codiceProvicia = indirizzo.id == 0 ? 0 : indirizzo.provincia.id;
                ViewBag.listaProvincia = new SelectList((from p in db.Province select p).ToList(), "denominazione", "denominazione");
                ViewBag.listaComuni = new SelectList((from c in db.Comuni where c.provincia.id == codiceProvicia select c).ToList(), "denominazione", "denominazione");
                return View("IndirizzoPartialEdit", indirizzo);
            }
            else
            {
                return View("IndirizzoPartialDetail", indirizzo);
            }
        }
        // GET /Indirizzo/IndirizzoPartial
        [ChildActionOnly]
        public ActionResult IndirizzoPartial(Indirizzo indirizzo, EnumTipoAzione tipoAzione = EnumTipoAzione.MODIFICA)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            if (indirizzo == null || indirizzo.id==0)
            {
                IndirizzoBusiness.valorizzaDatiDefault(indirizzo);
            }
            switch (tipoAzione)
            {  
                case EnumTipoAzione.INSERIMENTO:
                    valorizzaListeIndirizzoEdit(db, indirizzo);
                    return View("IndirizzoPartialInsert", indirizzo);
                case EnumTipoAzione.MODIFICA:
                    valorizzaListeIndirizzoEdit(db, indirizzo);
                    return View("IndirizzoPartialEdit", indirizzo);
                default:
                    return View("IndirizzoPartialDetail", indirizzo);
            }
        }

        private void valorizzaListeIndirizzoEdit(MainDbContext db, Indirizzo indirizzo)
        {

            valorizzaListeIndirizzoDetails(db);
            ViewBag.listaProvincia = new SelectList(db.Province.ToList(), "denominazione", "denominazione");
            ViewBag.listaComuni = new SelectList((from c in db.Comuni where c.provincia.id == indirizzo.provincia.id select c), "denominazione", "denominazione");
        }


        public void valorizzaListeIndirizzoDetails(MainDbContext db)
        {
            List<SelectListItem> lsli = new List<SelectListItem>();
            lsli.Add(new SelectListItem { Text = "", Value = "" });
            ViewBag.listaToponimo = new SelectList(db.Toponimi, "sigla", "sigla");
            ViewBag.listaTipoIndirizzo = new SelectList(db.TipoIndirizzo, "id", "descrizione");
            ViewBag.listaProvincia = new SelectList(lsli, "Text", "Value");
            ViewBag.listaComuni = new SelectList(lsli, "Text", "Value");
        }
        public void eliminaElementiNonCaricati()
        {
            ModelState.Remove("tipoIndirizzo.descrizione");
            ModelState.Remove("provincia.sigla");
        }

        [HttpPost]
        public ActionResult Edit(Indirizzo indirizzo)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            eliminaElementiNonCaricati();
            if (ModelState.IsValid)
            {
                Indirizzo indirizzoSalvato = IndirizzoBusiness.save(User.Identity.Name, indirizzo, db);
            }

            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }
        /**
         * da capire non mi torna un gran che
         */
        [HttpPost]
        public ActionResult CreateForCedente(Indirizzo indirizzo, int codiceSegnalazione)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);

            eliminaElementiNonCaricati();
            if (ModelState.IsValid)
            {
                indirizzo = IndirizzoBusiness.createBySegnalazione(User.Identity.Name, codiceSegnalazione, indirizzo, db);
            }
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        [HttpPost]
        public ActionResult CreateForSoggettoGiuridico(Indirizzo indirizzo, int codiceSoggettoGiuridico)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            indirizzo = IndirizzoBusiness.valorizzaDatiPerInserimentoCancellazione(indirizzo, db);
            ModelState.Clear();
            TryValidateModel(indirizzo);
            if (ModelState.IsValid)
            {
                indirizzo = IndirizzoBusiness.createBySoggettoGiuridico(User.Identity.Name, codiceSoggettoGiuridico, indirizzo, db);
            }
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

    }
}
