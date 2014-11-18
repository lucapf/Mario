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
       

        public ActionResult Index()
        {
           // MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            valorizzaViewBag();
            return View(new Riferimento());
        }


        [ChildActionOnly]
        public ActionResult Details(int contattoId)
        {
            Contatto contatto;
            contatto = db.Contatti.Include("riferimenti").Include("riferimenti.tipoRiferimento").Where(p => p.id == contattoId).First();
            if (contatto == null)
            {
                return HttpNotFound();
            }

            RiferimentiModel model = new RiferimentiModel();
            model.riferimenti  = contatto.riferimenti.ToList<Riferimento>();
            model.contattoId = contattoId;

            valorizzaViewBag();

            return View("_Riferimenti", model);
        }



        
        [ChildActionOnly]
        public ActionResult Create(Riferimento riferimento)
        {
#if DEBUG
            riferimento.valore = "Prova";
#endif
            valorizzaViewBag();
            ViewData.TemplateInfo.HtmlFieldPrefix = "riferimento";
            return View("RiferimentoPartialEdit", riferimento);
        }




        [HttpGet]
        public ActionResult riferimentoPartialById(int id, EnumTipoAzione tipoAzione)
        {
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
                    valorizzaViewBag();
                    return View("RiferimentoPartialEdit", riferimento);
                case EnumTipoAzione.INSERIMENTO:
                    valorizzaViewBag();
                    return View("RiferimentoPartialInsert", riferimento);
                default:
                    return View("RiferimentoPartialDetail", riferimento);
            }
        }
        private void valorizzaViewBag()
        {
            ViewBag.listaTipoRiferimento = new SelectList(db.TipoRiferimento, "id", "descrizione");
        }


        [HttpPost]
        public ActionResult CreateForContatto(Riferimento riferimento, int codiceContatto)
        {
            Contatto contatto = db.Contatti.Include("riferimenti").Where( c => c.id == codiceContatto).First() ;
            if (contatto == null)
            {
                return HttpNotFound();
            }

            riferimento.tipoRiferimento = db.TipoRiferimento.Find(riferimento.tipoRiferimento.id);

            ModelState.Clear();
            TryValidateModel(riferimento);

            contatto.riferimenti.Add(riferimento);
           
            try
            {
                db.SaveChanges();
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, "Riferimento creato con successo");
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                string messaggio;
                messaggio = MyHelper.getDbEntityValidationException(ex);
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile creare un nuovo riferimento: " + Environment.NewLine + messaggio);
            }

            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }


        [HttpPost]
        public ActionResult CreateForSegnalazione(Riferimento riferimento, int codiceSegnalazione)
        {
          
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
