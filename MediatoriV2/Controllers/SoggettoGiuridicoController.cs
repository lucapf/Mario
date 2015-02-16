using mediatori.Controllers.Business.Anagrafiche.Soggetto;
using mediatori.Filters;
using mediatori.Models;
using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using BusinessModel.Anagrafiche.SoggettoGiuridico;
using BusinessModel.Log;

namespace mediatori.Controllers.Business
{
    public class SoggettoGiuridicoController : MyBaseController
    {

        private SoggettoGiuridicoManager manager = null;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (db != null)
            {
                manager = new SoggettoGiuridicoManager(db.Database.Connection);
            }
        }


        public ActionResult Index(SearchSoggettoGiuridico model)
        {
            // model.tipoPersonaFisica = PersonaFisicaManager.TipoPersonaFisica.Cedente;
            //return View(db.Cedenti.ToList());
            manager.openConnection();
            try
            {
                manager.getList(model);
            }
            finally
            {
                manager.closeConnection();
            }

            valorizzaViewBag();

            return View(model);
        }




        //private static void valorizzaDatiBaseModel(SoggettoGiuridico soggettoG)
        //{
        //    soggettoG.indirizzi = new List<Indirizzo>();
        //    Indirizzo indirizzo = new Indirizzo();
        //    indirizzo.provincia = new Provincia();
        //    indirizzo.comune = new Comune();
        //    indirizzo.tipoIndirizzo = new TipologiaIndirizzo();
        //    indirizzo.toponimo = new Toponimo();
        //    soggettoG.indirizzi.Add(indirizzo);
        //    soggettoG.note = new List<Nota>();
        //    soggettoG.note.Add(new Nota());
        //    soggettoG.riferimenti = new List<Riferimento>();
        //    Riferimento r = new Riferimento();
        //    r.tipoRiferimento = new TipoRiferimento();
        //    soggettoG.riferimenti.Add(r);

        //}

        private SoggettoGiuridicoCreateModel valorizzaDatiCreateModel(SoggettoGiuridicoCreateModel model, MainDbContext db)
        {
            model.soggettoGiuridico = new SoggettoGiuridico();


            model.indirizzo = IndirizzoBusiness.valorizzaDatiDefault(new Indirizzo());
            model.riferimento = RiferimentoBusiness.valorizzaDatiDefault(new Riferimento());
            model.nota = new Nota();

            model.soggettoGiuridico = new SoggettoGiuridico();
#if DEBUG
            model.soggettoGiuridico.codiceFiscale = "ZZZYYY55S66H406B";
            model.soggettoGiuridico.ragioneSociale = "Società di prova";


#endif

            return model;
        }






        private void valorizzaViewBag()
        {
            ViewBag.listaTipiSoggettiGiuridici =
                new SelectList(from EnumTipoSoggettoGiuridico e in Enum.GetValues(typeof(EnumTipoSoggettoGiuridico))
                               select new { id = e.ToString(), Name = e.ToString() }, "id", "Name");
        }

        [ChildActionOnly]
        public ActionResult soggettoGiuridicoPartial(SoggettoGiuridico soggettoGiuridico, EnumTipoAzione tipoAzione)
        {
            valorizzaViewBag();
            return dispatch(soggettoGiuridico, tipoAzione);
        }

        [ChildActionOnly]
        public ActionResult SoggettoGiuridicoDetail(SoggettoGiuridico soggettoGiuridico, EnumTipoAzione tipoAzione)
        {
            valorizzaViewBag();
            return View("DetailsNoLayout", soggettoGiuridico);
        }

        [HttpGet]
        public ActionResult SoggettoGiuridicoPartialById(int id, EnumTipoAzione tipoAzione)
        {
            SoggettoGiuridico soggettoGiuridico = new SoggettoGiuridicoBusiness().findByPK(id, db);

            if (soggettoGiuridico == null)
            {
                return HttpNotFound();
            }

            if (tipoAzione == EnumTipoAzione.MODIFICA)
            {
                valorizzaViewBag();
                return View("SoggettoGiuridicoEdit", soggettoGiuridico);
            }

            if (tipoAzione == EnumTipoAzione.VISUALIZZAZIONE)
            {
                return View("SoggettoGiuridicoPartialDetail", soggettoGiuridico);
            }

            throw new ApplicationException("Azione di inserimento che non si deve presentare");

        }

        [HttpGet]
        public ActionResult Create()
        {
            SoggettoGiuridicoCreateModel model = new SoggettoGiuridicoCreateModel();
            model = valorizzaDatiCreateModel(model, db);
            valorizzaViewBag();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(SoggettoGiuridicoCreateModel model)
        {
            SoggettoGiuridico soggettoGiuridico = model.soggettoGiuridico;
            soggettoGiuridico.indirizzi = new List<Indirizzo>();
            soggettoGiuridico.indirizzi.Add(model.indirizzo);

            soggettoGiuridico.note = new List<Nota>();
            soggettoGiuridico.note.Add(model.nota);

            soggettoGiuridico.riferimenti = new List<Riferimento>();
            soggettoGiuridico.riferimenti.Add(model.riferimento);


            SoggettoGiuridicoBusiness sogettoGiuridicoBusiness = new SoggettoGiuridicoBusiness();
            soggettoGiuridico = sogettoGiuridicoBusiness.completaDati(soggettoGiuridico, User.Identity.Name, db);


            ModelState.Clear();
            TryValidateModel(soggettoGiuridico);

            if (ModelState.IsValid)
            {
                db.SoggettiGiuridici.Add(soggettoGiuridico);

                try
                {
                    db.SaveChanges();
                    LogEventiManager.save(LogEventiManager.getEventoForCreate(User.Identity.Name, soggettoGiuridico.id, EnumEntitaRiferimento.SOCIETA), db);

                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    string messaggio;
                    messaggio = MyHelper.getDbEntityValidationException(ex);
                    ViewBag.erroMessage = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare il soggetto giuridico , verificare i dati: " + Environment.NewLine + messaggio);

                    valorizzaViewBag();
                    return View(model);
                }



                return RedirectToAction("Index");
            }


            var message = string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));

            Debug.WriteLine(message);

            ViewBag.erroMessage = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare il soggetto giuridico , verificare i dati: " + Environment.NewLine + message);

            valorizzaViewBag();
            return View(model);
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
