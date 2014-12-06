using mediatori.Controllers.Business.Anagrafiche.Soggetto;
using mediatori.Controllers.CQS;
using mediatori.Filters;
using mediatori.Models;
using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace mediatori.Controllers.Business
{
    public class SoggettoGiuridicoController : MyBaseController
    { 
       

        private static void valorizzaDatiBaseModel(SoggettoGiuridico soggettoG)
        {
            soggettoG.indirizzi = new List<Indirizzo>();
            Indirizzo indirizzo = new Indirizzo();
            indirizzo.provincia = new Provincia();
            indirizzo.comune = new Comune();
            indirizzo.tipoIndirizzo = new TipologiaIndirizzo();
            indirizzo.toponimo = new Toponimo();
            soggettoG.indirizzi.Add(indirizzo);
            soggettoG.note = new List<Nota>();
            soggettoG.note.Add(new Nota());
            soggettoG.riferimenti = new List<Riferimento>();
            Riferimento r = new Riferimento();
            r.tipoRiferimento = new TipoRiferimento();
            soggettoG.riferimenti.Add(r);
           
        }

        public ActionResult Index(SoggettoGiuridicoSearch soggettoGiuridicoSearch)
        {
            List<SoggettoGiuridico> listaSoggetti = new SoggettoGiuridicoBusiness().findByFilter(soggettoGiuridicoSearch, db);
            ValorizzaViewBag();
            return View(listaSoggetti);
        }

        private void ValorizzaViewBag()
        {
            ViewBag.listaTipiSoggettiGiuridici =  
                new SelectList(from EnumTipoSoggettoGiuridico e in Enum.GetValues(typeof(EnumTipoSoggettoGiuridico))
                               select new { id = e.ToString(), Name = e.ToString() }, "id", "Name");
        }
       
        [ChildActionOnly]
        public ActionResult soggettoGiuridicoPartial(SoggettoGiuridico soggettoGiuridico, EnumTipoAzione tipoAzione)
        {
            ValorizzaViewBag();
            return dispatch(soggettoGiuridico, tipoAzione);
        }

        [ChildActionOnly]
        public ActionResult SoggettoGiuridicoDetail(SoggettoGiuridico soggettoGiuridico, EnumTipoAzione tipoAzione)
        {
            ValorizzaViewBag();
            return View("DetailsNoLayout", soggettoGiuridico);
        }

       
        public ActionResult soggettoGiuridicoPartialById(int id, EnumTipoAzione tipoAzione)
        {   
             SoggettoGiuridico s = new SoggettoGiuridicoBusiness().findByPK(id, db);
             if (db.Amministazioni.Where(m => m.soggettoGiuridico.id == id).ToList().Count >0)
             {
                 ViewBag.tipoSoggetto = "AMMINISTRAZIONE";
             }
             ValorizzaViewBag();
            return dispatch(s, tipoAzione);
        }

        [HttpGet]
        public ActionResult Create()
        {
            SoggettoGiuridico soggettoG = new SoggettoGiuridico();
            valorizzaDatiBaseModel(soggettoG);
            ValorizzaViewBag();
            return View(soggettoG);
        }

        [HttpPost]
        public ActionResult Create(SoggettoGiuridico soggettoGiuridico)
        {
            SoggettoGiuridicoBusiness sogettoGiuridicoBusiness = new SoggettoGiuridicoBusiness();
            ModelState.Clear();
            soggettoGiuridico = sogettoGiuridicoBusiness.completaDati(soggettoGiuridico, User.Identity.Name, db);
            TryValidateModel(soggettoGiuridico);

            if (ModelState.IsValid)
            {
                db.SoggettiGiuridici.Add(soggettoGiuridico);

                try
                {
                    db.SaveChanges();
                    LogEventiManager.save(
                  LogEventiManager.getEventoForCreate(User.Identity.Name, soggettoGiuridico.id,
                  EnumEntitaRiferimento.SOCIETA), db);

                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    string messaggio;
                    messaggio = MyHelper.getDbEntityValidationException(ex);
                    ViewBag.erroMessage = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare il soggetto giuridico , verificare i dati: " + Environment.NewLine + messaggio);

                    ValorizzaViewBag();
                    return View(soggettoGiuridico);
                }
                
                

                return RedirectToAction("Index");
            }


            var message = string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));

            Debug.WriteLine(message);

            ViewBag.erroMessage = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare il soggetto giuridico , verificare i dati: " + Environment.NewLine + message);

            ValorizzaViewBag();
            return View(soggettoGiuridico);
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            SoggettoGiuridicoBusiness sogettoGiuridicoBusiness = new SoggettoGiuridicoBusiness();
            SoggettoGiuridico soggettoGiuridico = sogettoGiuridicoBusiness.findByPK(id, db);
            return View(soggettoGiuridico);
        }
        [HttpPost]
        public ActionResult Edit(SoggettoGiuridico soggettoGiuridico)
        {
            SoggettoGiuridicoBusiness sogettoGiuridicoBusiness = new SoggettoGiuridicoBusiness();
            SoggettoGiuridico soggettoOriginale = sogettoGiuridicoBusiness.findByPK(soggettoGiuridico.id, db);
            soggettoOriginale.codiceFiscale = soggettoGiuridico.codiceFiscale;
            soggettoOriginale.ragioneSociale = soggettoGiuridico.ragioneSociale;
            soggettoOriginale.tipoSoggettoGiuridico = soggettoGiuridico.tipoSoggettoGiuridico;
            LogEventi le = LogEventiManager.getEventoForUpdate(User.Identity.Name, soggettoGiuridico.id, EnumEntitaRiferimento.IMPIEGO, soggettoOriginale, soggettoGiuridico);
         
            LogEventiManager.save(le, db);
            db.SaveChanges();
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }
        private ActionResult dispatch(SoggettoGiuridico soggettoGiuridico, EnumTipoAzione tipoAzione)
        {
            switch (tipoAzione)
            {
                case EnumTipoAzione.INSERIMENTO:
                case EnumTipoAzione.VISUALIZZAZIONE:
                    return View("SoggettoGiuridicoPartialDetail", soggettoGiuridico);
                case EnumTipoAzione.MODIFICA:
                    return View("SoggettoGiuridicoPartialEdit", soggettoGiuridico);
            }
            return View("ContattoPartialDetail", soggettoGiuridico);
        }

    }
}
