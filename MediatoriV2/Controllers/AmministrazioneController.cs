using mediatori.Controllers.Business;
using mediatori.Controllers.Business.Anagrafiche;
using mediatori.Controllers.Business.Anagrafiche.Soggetto;
using mediatori.Filters;
using mediatori.Models;
using mediatori.Models.Anagrafiche;
using mediatori.Models.etc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessModel.Anagrafiche;
using System.Diagnostics;
using BusinessModel.Anagrafiche.Amministrazione;

namespace mediatori.Controllers
{
    public class AmministrazioneController : MyBaseController
    {
        private AmministrazioneManager manager = null;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (db != null)
            {
                manager = new AmministrazioneManager(db.Database.Connection);
            }
        }

        public ActionResult Index(SearchAmministrazione model)
        {
            manager.openConnection();
            try
            {
                manager.getList(model);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                manager.closeConnection();
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            AmministrazioneBusiness amministrazioneBusiness = new AmministrazioneBusiness();
            Amministrazione amministrazione = amministrazioneBusiness.findByPK(id, db);
            return View(amministrazione);
        }

        [ChildActionOnly]
        public ActionResult AmministrazionePartial(Amministrazione amministrazione, EnumTipoAzione tipoAzione)
        {
            valorizzaViewBag();
            amministrazione = new AmministrazioneBusiness().completaEVerifica(amministrazione, db);
            return dispatch(amministrazione, tipoAzione);
        }

        [HttpGet]
        public ActionResult AmministrazionePartialById(int id, EnumTipoAzione tipoAzione)
        {
            Amministrazione amministrazione = new AmministrazioneBusiness().findByPK(id, db);

            if (amministrazione == null)
            {
                return HttpNotFound();
            }

            if (tipoAzione == EnumTipoAzione.MODIFICA)
            {
                valorizzaViewBag();
                return View("AmministrazioneEdit", amministrazione);
            }

            if (tipoAzione == EnumTipoAzione.VISUALIZZAZIONE)
            {
                return View("AmministrazionePartialDetail", amministrazione);
            }

            throw new ApplicationException("Azione di inserimento che non si deve presentare");

        }

        //[ChildActionOnly]
        //public ActionResult ViewDetail(Amministrazione amministrazione)
        //{
        //    return View("AmministrazionePartialDetail", amministrazione);
        //}

        [HttpPost]
        public ActionResult Edit(Amministrazione amministrazione)
        {
            AmministrazioneBusiness amministrazioneBusiness = new AmministrazioneBusiness();
            Amministrazione amministrazioneOriginale = amministrazioneBusiness.findByPK(amministrazione.id, db);

            if (amministrazioneOriginale == null)
            {
                return HttpNotFound();
            }


            amministrazioneOriginale.partitaIva = amministrazione.partitaIva;
            amministrazioneOriginale.capitaleSociale = amministrazione.capitaleSociale;

            amministrazioneOriginale.tipoNaturaGiuridica = db.tipoNaturaGiuridica.Find(amministrazione.tipoNaturaGiuridica.id);
            amministrazioneOriginale.tipoCategoria = db.TipoCategoriaAmministrazione.Find(amministrazione.tipoCategoria.id);
            amministrazioneOriginale.assumibilita = db.TipoAssumibilitaAmministrazione.Find(amministrazione.assumibilita.id);

            amministrazioneOriginale.soggettoGiuridico.codiceFiscale = amministrazione.soggettoGiuridico.codiceFiscale;
            amministrazioneOriginale.soggettoGiuridico.ragioneSociale = amministrazione.soggettoGiuridico.ragioneSociale;
            amministrazioneOriginale.isEnabled = amministrazione.isEnabled;

            ModelState.Clear();
            
            TryValidateModel(amministrazioneOriginale);
            if (ModelState.IsValid)
            {
                db.SaveChanges();
            }

            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        [HttpGet]
        public ActionResult Create()
        {
            AmministrazioneCreateModel model = new AmministrazioneCreateModel();
            valorizzaViewBag();
            model = valorizzaDatiCreateModel(model, db);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(AmministrazioneCreateModel model)
        {
            Amministrazione amministazione = model.amministrazione;
            amministazione.soggettoGiuridico = model.soggettoGiuridico;
            amministazione.soggettoGiuridico.tipoSoggettoGiuridico = EnumEntitaRiferimento.AMMINISTRAZIONE.ToString();

            amministazione.soggettoGiuridico.indirizzi = new List<Indirizzo>();
            amministazione.soggettoGiuridico.indirizzi.Add(model.indirizzo);

            amministazione.soggettoGiuridico.note = new List<Nota>();
            amministazione.soggettoGiuridico.note.Add(model.nota);

            amministazione.soggettoGiuridico.riferimenti = new List<Riferimento>();
            amministazione.soggettoGiuridico.riferimenti.Add(model.riferimento);

            AmministrazioneBusiness.valorizzaDati(amministazione, User.Identity.Name, db);

            ModelState.Clear();
            TryValidateModel(amministazione);

            if (ModelState.IsValid)
            {
                db.Amministazioni.Add(amministazione);
                db.SaveChanges();
            }
            //  return View("Details", amministazione);
            return RedirectToAction("Index");
        }

        private void valorizzaViewBag()
        {
            ViewBag.listaTipoNaturaGiuridica = new SelectList(db.tipoNaturaGiuridica.OrderBy(p => p.descrizione), "id", "Descrizione");
            ViewBag.listaTipoCategoria = new SelectList(db.TipoCategoriaAmministrazione, "id", "Descrizione");
            ViewBag.listaTipoAssumibilita = new SelectList(db.TipoAssumibilitaAmministrazione, "id", "Descrizione");
            //IQueryable<Stato> listaStati = db.StatiSegnalazione.Where(m =>         m.entitaAssociata == EnumEntitaAssociataStato.AMMINISTRAZIONE);
            //ViewBag.listaStati = new SelectList(listaStati, "id", "descrizione");

        }

        private AmministrazioneCreateModel valorizzaDatiCreateModel(AmministrazioneCreateModel model, MainDbContext db)
        {
            model.amministrazione = new Amministrazione();
           //model.amministrazione.soggettoGiuridico = new SoggettoGiuridico();
            model.amministrazione = new AmministrazioneBusiness().completaEVerifica(model.amministrazione, db);
            model.amministrazione.isEnabled = true;

            model.indirizzo = IndirizzoBusiness.valorizzaDatiDefault(new Indirizzo());
            model.riferimento = RiferimentoBusiness.valorizzaDatiDefault(new Riferimento());
            model.nota = new Nota();

            model.soggettoGiuridico = new SoggettoGiuridico();
#if DEBUG
            model.soggettoGiuridico.codiceFiscale = "ZZZYYY55S66H406B";
            model.soggettoGiuridico.ragioneSociale = "Amministrazione di prova";

            model.amministrazione.partitaIva = "2222222222222";
            model.amministrazione.capitaleSociale = 10000;

#endif

            return model;
        }

        private ActionResult dispatch(Amministrazione amministrazione, EnumTipoAzione tipoAzione)
        {
            switch (tipoAzione)
            {
                case EnumTipoAzione.INSERIMENTO:
                case EnumTipoAzione.VISUALIZZAZIONE:
                    return View("AmministrazionePartialDetail", amministrazione);
                case EnumTipoAzione.MODIFICA:
                    return View("AmministrazionePartialEdit", amministrazione);
            }
            return View("AmministrazionePartialDetail", amministrazione);
        }
    }
}
