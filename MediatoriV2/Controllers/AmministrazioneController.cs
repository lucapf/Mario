using mediatori.Controllers.Business;
using mediatori.Controllers.Business.Anagrafiche;
using mediatori.Controllers.Business.Anagrafiche.Soggetto;
using mediatori.Controllers.CQS;
using mediatori.Filters;
using mediatori.Models;
using mediatori.Models.Anagrafiche;
using mediatori.Models.etc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mediatori.Controllers
{
    public class AmministrazioneController : MyBaseController
    {
        //
        // GET: /Amministrazione/

        public ActionResult Index(AmministrazioneFilter amministrazioneFilter)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);

            List<Amministrazione> listaAmministrazioni = AmministrazioneBusiness.findByFilter(amministrazioneFilter, db);
            return View(listaAmministrazioni);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            AmministrazioneBusiness amministrazioneBusiness = new AmministrazioneBusiness();
            Amministrazione amministrazione = amministrazioneBusiness.findByPK(id, db);
            return View(amministrazione);
        }

        [ChildActionOnly]
        public ActionResult AmministrazionePartial(Amministrazione amministrazione, EnumTipoAzione tipoAzione)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            valorizzaViewBag(db);
            amministrazione = new AmministrazioneBusiness().completaEVerifica(amministrazione, db);
            return dispatch(amministrazione, tipoAzione);
        }
        
        
        [HttpGet]
        public ActionResult amministrazionePartialById(int id, EnumTipoAzione tipoAzione)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            Amministrazione s = new AmministrazioneBusiness().findByPK(id, db);
            valorizzaViewBag(db);
            return dispatch(s, tipoAzione);
        }
        [ChildActionOnly]
        public ActionResult ViewDetail(Amministrazione amministrazione)
        {
            return View("AmministrazionePartialDetail", amministrazione);
        }
        [HttpPost]
        public ActionResult Edit(Amministrazione amministrazione)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            AmministrazioneBusiness amministrazioneBusiness = new AmministrazioneBusiness();
            Amministrazione amministrazioneOriginale = amministrazioneBusiness.findByPK(amministrazione.id, db);
            amministrazioneOriginale.partitaIva = amministrazione.partitaIva;
            amministrazioneOriginale.capitaleSociale = amministrazione.capitaleSociale;
            amministrazioneOriginale.tipoNaturaGiuridica = db.tipoNaturaGiuridica.Find(amministrazione.tipoNaturaGiuridica.id);
            amministrazioneOriginale.stato = db.statiSegnalazione.Find(amministrazione.stato.id) ;
            amministrazioneOriginale.tipoCategoria= db.TipoCategoriaAmministrazione.Find(amministrazione.tipoCategoria.id);
            amministrazioneOriginale.assumibilita = db.TipoAssumibilitaAmministrazione.Find(amministrazione.assumibilita.id);
           
            ModelState.Clear();
            TryValidateModel(amministrazioneOriginale);
            if (ModelState.IsValid)
            {
               db.SaveChanges();
            }
           
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }





        [HttpGet]
        public ActionResult Create(int codiceAmministazione = 0)
        {
            var amministrazione = new AmministrazioneCreate();
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            valorizzaViewBag(db);
            amministrazione = valorizzaDatiAmministrazione(amministrazione, db);
            if (codiceAmministazione != 0)
            {
                amministrazione.amministrazione = new AmministrazioneBusiness().findByPK(codiceAmministazione, db);

            }
            return View(amministrazione);
        }
        [HttpPost]
        public ActionResult Create(AmministrazioneCreate ammCreate)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);

            Amministrazione amministazione = ammCreate.amministrazione;
            amministazione.soggettoGiuridico = ammCreate.soggettoGiuridico;
            amministazione.soggettoGiuridico.tipoSoggettoGiuridico = "AMMINISTRAZIONE";
            amministazione.soggettoGiuridico.indirizzi = ammCreate.indirizzi;
            amministazione.soggettoGiuridico.note = ammCreate.note;
            amministazione.soggettoGiuridico.riferimenti = ammCreate.riferimenti;
            AmministrazioneBusiness.valorizzaDati(amministazione, User.Identity.Name, db);
            ModelState.Clear();
            TryValidateModel(amministazione);
            if (ModelState.IsValid)
            {
                db.amministazioni.Add(amministazione);
                db.SaveChanges();
            }
            return View("Details", amministazione);
        }
        private void valorizzaViewBag(MainDbContext db)
        {
            ViewBag.listaTipoNaturaGiuridica = new SelectList(db.tipoNaturaGiuridica.OrderBy(p =>p.descrizione), "id", "Descrizione");
            ViewBag.listaTipoCategoria = new SelectList(db.TipoCategoriaAmministrazione, "id", "Descrizione");
            ViewBag.listaTipoAssumibilita = new SelectList(db.TipoAssumibilitaAmministrazione, "id", "Descrizione");
            IQueryable<Stato> listaStati = db.statiSegnalazione.Where(m =>
                    m.entitaAssociata == EnumEntitaAssociataStato.AMMINISTRAZIONE);
            ViewBag.listaStati = new SelectList(listaStati, "id", "descrizione");

        }

        private AmministrazioneCreate valorizzaDatiAmministrazione(AmministrazioneCreate ammCreate, MainDbContext db)
        {
            ammCreate.amministrazione = new Amministrazione();
            ammCreate.amministrazione.soggettoGiuridico = new SoggettoGiuridico();
            ammCreate.amministrazione = new AmministrazioneBusiness().completaEVerifica(ammCreate.amministrazione,db);
            ammCreate.indirizzi = new List<Indirizzo>();
            ammCreate.indirizzi.Add(IndirizzoBusiness.valorizzaDatiDefault(new Indirizzo()));
            ammCreate.riferimenti = new List<Riferimento>();
            ammCreate.riferimenti.Add(RiferimentoBusiness.valorizzaDatiDefault(new Riferimento()));
            ammCreate.note = new List<Nota>();
            ammCreate.note.Add(new Nota());
            return ammCreate;
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
