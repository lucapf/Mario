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
    public class IndirizzoController : MyBaseController
    {
        //
        // GET: /Indirizzo/

        /*public ActionResult Index()
        {
            return View();
        }
         */
        [ChildActionOnly]
        public ActionResult Details(int cedenteId)
        {
            Cedente cedente;
            cedente = db.Cedenti.Include("indirizzi").Where(p => p.id == cedenteId).First();
            if (cedente == null)
            {
                return HttpNotFound();
            }

            IndirizziModel model = new IndirizziModel();
            model.indirizzi = cedente.indirizzi.ToList<Indirizzo>();
            model.contattoId = cedenteId;

            valorizzaDatiViewBag();

            return View("_Indirizzi", model);
        }


        [HttpGet]
        public ActionResult IndirizzoPartialById(int id, EnumTipoAzione tipoAzione = EnumTipoAzione.MODIFICA)
        {
            Indirizzo indirizzo = new Indirizzo { id = id };
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

            if (indirizzo == null || indirizzo.id == 0)
            {
                IndirizzoBusiness.valorizzaDatiDefault(indirizzo);
            }
            switch (tipoAzione)
            {
                case EnumTipoAzione.INSERIMENTO:
                    valorizzaListeIndirizzoEdit(db, indirizzo);
                    return View("IndirizzoPartialInsert", indirizzo);
                //  return View("IndirizzoPartialEdit", indirizzo);
                case EnumTipoAzione.MODIFICA:
                    valorizzaListeIndirizzoEdit(db, indirizzo);
                    return View("IndirizzoPartialEdit", indirizzo);
                default:
                    return View("IndirizzoPartialDetail", indirizzo);
            }
        }

        private void valorizzaDatiViewBag()
        {
            ViewBag.listaProvincia = new SelectList(db.Province.ToList(), "denominazione", "denominazione");
            List<SelectListItem> lsli = new List<SelectListItem>();
            lsli.Add(new SelectListItem { Text = "", Value = "" });
            ViewBag.listaComuni = lsli;

            ViewBag.listaToponimo = new SelectList(db.Toponimi, "sigla", "sigla");
            ViewBag.listaTipoIndirizzo = new SelectList(db.TipoIndirizzo, "id", "descrizione");
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
            eliminaElementiNonCaricati();
            if (ModelState.IsValid)
            {
                Indirizzo indirizzoSalvato = IndirizzoBusiness.save(User.Identity.Name, indirizzo, db);
            }

            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }


        [HttpPost]
        public ActionResult CreateForPersonaFisica(Indirizzo indirizzo, int codiceId)
        {

            indirizzo.provincia = (from p in db.Province
                                   where p.denominazione == indirizzo.provincia.denominazione
                                   select p).First();

            indirizzo.comune = (from c in db.Comuni
                                where c.denominazione == indirizzo.comune.denominazione &&
                                      c.codiceProvincia == indirizzo.provincia.id
                                select c).First();
            ModelState.Clear();
            TryValidateModel(indirizzo);

            if (ModelState.IsValid)
            {
                indirizzo = IndirizzoBusiness.createBySegnalazione(User.Identity.Name, codiceId, indirizzo, db);
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare l'indirizzo, verificare i dati: " + Environment.NewLine + message);
            }
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        [HttpPost]
        public ActionResult CreateForCedente(Indirizzo indirizzo, int codiceCedente)
        {

            //  eliminaElementiNonCaricati();
            if (ModelState.IsValid)
            {
                indirizzo = IndirizzoBusiness.createBySegnalazione(User.Identity.Name, codiceCedente, indirizzo, db);
            }
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        [HttpPost]
        public ActionResult CreateForSoggettoGiuridico(Indirizzo indirizzo, int codiceSoggettoGiuridico)
        {
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
