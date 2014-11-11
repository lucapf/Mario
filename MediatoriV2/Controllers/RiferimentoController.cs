using mediatori.Controllers.Business;
using mediatori.Controllers.Business.Anagrafiche.Soggetto;
using mediatori.Models;
using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mediatori.Controllers
{
    public class RiferimentoController : MyBaseController
    {
        private MainDbContext db = new MainDbContext();

        public ActionResult Index()
        {
           // MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            valorizzaViewBag(db);
            return View(new Riferimento());
        }
        
        [ChildActionOnly]
        public ActionResult Create(Riferimento riferimento)
        {
#if DEBUG
            riferimento.valore = "Prova";
#endif
            valorizzaViewBag(db);
            ViewData.TemplateInfo.HtmlFieldPrefix = "riferimento";
            return View("RiferimentoPartialEdit", riferimento);
        }




        [HttpGet]
        public ActionResult riferimentoPartialById(int id, EnumTipoAzione tipoAzione)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            Riferimento r = RiferimentoBusiness.findByPk(id, db);
            return riferimentoDispatcher(r, tipoAzione, db);
        }

        [ChildActionOnly]
        public ActionResult riferimentoPartial(Riferimento riferimento, EnumTipoAzione tipoAzione = EnumTipoAzione.MODIFICA)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            return riferimentoDispatcher(riferimento, tipoAzione, db);
        }

        private ActionResult riferimentoDispatcher(Riferimento riferimento, EnumTipoAzione tipoAzione, MainDbContext db)
        {
            switch (tipoAzione)
            {
                case EnumTipoAzione.MODIFICA:
                    valorizzaViewBag(db);
                    return View("RiferimentoPartialEdit", riferimento);
                case EnumTipoAzione.INSERIMENTO:
                    valorizzaViewBag(db);
                    return View("RiferimentoPartialInsert", riferimento);
                default:
                    return View("RiferimentoPartialDetail", riferimento);
            }
        }
        private void valorizzaViewBag(MainDbContext db)
        {
            ViewBag.listaTipoRiferimento = new SelectList(db.TipoRiferimento, "id", "descrizione");
        }
        [HttpPost]
        public ActionResult CreateForSegnalazione(Riferimento riferimento, int codiceSegnalazione)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            riferimento = RiferimentoBusiness.valorizzaDatiRiferimento(riferimento, db);
            Segnalazione s = new SegnalazioneBusiness().findByPk(codiceSegnalazione, db);
            ModelState.Clear();
            TryValidateModel(riferimento);
            s.contatto.riferimenti.Add(riferimento);
            db.SaveChanges();
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }
        [HttpPost]
        public ActionResult CreateForSoggettoGiuridico(Riferimento riferimento, int codiceSoggettoGiuridico)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            riferimento = RiferimentoBusiness.valorizzaDatiRiferimento(riferimento, db);
            SoggettoGiuridico s = new SoggettoGiuridicoBusiness().findByPK(codiceSoggettoGiuridico, db);
            ModelState.Clear();
            TryValidateModel(riferimento);
            s.riferimenti.Add(riferimento);
            db.SaveChanges();
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }
        [HttpPost]
        public ActionResult Edit(Riferimento riferimento)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            riferimento = RiferimentoBusiness.valorizzaDatiRiferimento(riferimento, db);
            ModelState.Clear();
            
            TryValidateModel(riferimento);
            if (ModelState.IsValid)
            {
                Riferimento riferimentoSalvato = RiferimentoBusiness.save(User.Identity.Name, riferimento, db);
            }

            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }
    }
}
