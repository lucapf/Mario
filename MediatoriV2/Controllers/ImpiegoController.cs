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
    public class ImpiegoController : MyBaseController
    {
        private MainDbContext db = new MainDbContext();

        public ActionResult Index()
        {

            valorizzaViewBag(db);
            return View(new ImpiegoBusiness());
        }
        [HttpGet]
        public ActionResult ImpiegoPartialById(int id, EnumTipoAzione tipoAzione = EnumTipoAzione.VISUALIZZAZIONE)
        {
            Impiego impiego = new Impiego { id = id };
            impiego.tipoImpiego = new TipoContrattoImpiego();
            impiego.categoriaImpiego = new TipoCategoriaImpiego();
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            if (impiego.id > 0)
            {
                impiego = (from i in db.impieghi.Include("tipoImpiego").Include("categoriaImpiego") where i.id == impiego.id select i).First();

            }
            if (tipoAzione == EnumTipoAzione.MODIFICA || tipoAzione == EnumTipoAzione.INSERIMENTO)
            {
                valorizzaViewBag(db);
                return View("impiegoPartialEdit", impiego);
            }
            else
            {
                return View("impiegoInnerDetail", impiego);
            }
        }



        [ChildActionOnly]
        public ActionResult Create(Impiego impiego)
        {
          //  Impiego impiego = new Impiego();

#if DEBUG
            impiego.azienda = "Azienda";
            impiego.aziendaSedeLavoro = "Sede lavoro";
            impiego.dataAssunzione = new DateTime(2000, 8, 1);
            impiego.mansione = "Impiegato";
            impiego.mensilita = 14;
            impiego.stipendioLordoAnnuo = 20000;
            impiego.stipendioLordoMensile = 1200;
            impiego.stipendioNettoMensile = 900;
#endif


            valorizzaViewBag(db);

            ViewData.TemplateInfo.HtmlFieldPrefix = "impiego";
            

            return View("ImpiegoPartialEdit", impiego);
        }


        [ChildActionOnly]
        public ActionResult impiegoPartial(Impiego impiego, EnumTipoAzione tipoAzione = EnumTipoAzione.MODIFICA)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            switch (tipoAzione)
            {
                case EnumTipoAzione.MODIFICA:
                    valorizzaViewBag(db);
                    return View("ImpiegoPartialEdit", impiego);
                case EnumTipoAzione.INSERIMENTO:
                    valorizzaViewBag(db);
                    return View("ImpiegoPartialInsert", impiego);
                default:
                    return View("ImpiegoPartialDetail", impiego);
            }
        }
        [HttpPost]
        public ActionResult Edit(Impiego impiego)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            completaDatiImpiegoFromRequest(impiego, db);
            Impiego impiegoOriginale = (from i in db.impieghi.Include("tipoImpiego") where i.id == impiego.id select i).First();

            LogEventi le = LogEventiManager.getEventoForUpdate(User.Identity.Name, impiego.id, EnumEntitaRiferimento.IMPIEGO, impiegoOriginale, impiego);
            impiegoOriginale = (Impiego)CopyObject.simpleCompy(impiegoOriginale, impiego);

            LogEventiManager.save(le, db);
            db.SaveChanges();

            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        private static Impiego completaDatiImpiegoFromRequest(Impiego impiego, MainDbContext db)
        {
            // impiego.dataLicenziamento = impiego.dataLicenziamento.Year == 01 ? new DateTime(2050, 12, 31) : impiego.dataLicenziamento;

            impiego.tipoImpiego = db.TipoContrattoImpiego.Find(impiego.tipoImpiego.id);
            impiego.categoriaImpiego = db.TipoCategoriaImpiego.Find(impiego.categoriaImpiego.id);
            return impiego;
        }

        [HttpPost]
        public ActionResult CreateForCedente(Impiego impiego, int codiceCedente)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            impiego = completaDatiImpiegoFromRequest(impiego, db);
            ModelState.Remove("tipoImpiego.descrizione");
            if (ModelState.IsValid)
            {
                Cedente cedente = RicercaCedenteBusiness.find(codiceCedente, db);
                cedente.impieghi.Add(impiego);
                LogEventiManager.save(LogEventiManager.getEventoForCreate(User.Identity.Name, impiego.id, EnumEntitaRiferimento.IMPIEGO), db);
                db.SaveChanges();
            }
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }
        [HttpPost]
        public ActionResult CreateForSegnalazione(Impiego impiego, int codiceSegnalazione)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            impiego = completaDatiImpiegoFromRequest(impiego, db);
            ModelState.Clear();
            TryValidateModel(impiego);
            if (ModelState.IsValid)
            {
                Segnalazione segnalazione = new SegnalazioneBusiness().findByPk(codiceSegnalazione, db);
                segnalazione.contatto.impieghi.Add(impiego);
                LogEventiManager.save(LogEventiManager.getEventoForCreate(User.Identity.Name, impiego.id, EnumEntitaRiferimento.IMPIEGO), db);
                db.SaveChanges();
            }
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        private void valorizzaViewBag(MainDbContext db)
        {
            ViewBag.listaTipoImpiego = new SelectList(db.TipoContrattoImpiego, "id", "descrizione");
            ViewBag.listaCategoriaImpiego = new SelectList(db.TipoCategoriaImpiego, "id", "descrizione");
            //ViewBag.oggi = System.DateTime.Now.Year;
            //ViewBag.inizioValiditaImpiego = System.DateTime.Now.AddYears(-40).Year;

        }
    }
}
