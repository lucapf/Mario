using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mediatori.Models.Anagrafiche;
using mediatori.Models;
using mediatori.Helpers;
using mediatori.Controllers.Business;
using mediatori.Controllers.Business.Anagrafiche;
using mediatori.Filters;
using mediatori.Models.etc;

namespace mediatori.Controllers
{
    public class GestioneSegnalazioniController : Controller
    {


        //
        // GET: /GestioneSegnalazioni/

        public ActionResult Index(SegnalazioneSearch segnalazioniSearch, String message)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            

            ViewBag.message = message;
            return View(new SegnalazioneBusiness().findByFilter(segnalazioniSearch, db));
        }

        //
        // GET: /GestioneSegnalazioni/Details/5
        [HttpGet]
        public ActionResult Details(int id = 0)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            Segnalazione segnalazione = new SegnalazioneBusiness().findByPk(id, db);
           if (segnalazione == null)
            {
                return HttpNotFound();
            }
            return View(segnalazione);
        }
        

        //
        // GET: /GestioneSegnalazioni/Create
        [HttpGet]
        public ActionResult Create(int codiceContatto=0)
        {
            var segnalazioneCreate = new SegnalazioneCreate();     
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            segnalazioneCreate = valorizzaDatiViewBagSegnalazione(segnalazioneCreate, new MainDbContext(HttpContext.Request.Url.AbsoluteUri));
            if (codiceContatto != 0)
            {
                Contatto contatto = new ContattoBusiness().findByPK(codiceContatto,db);
                segnalazioneCreate.impieghi = contatto.impieghi;
                segnalazioneCreate.riferimenti = contatto.riferimenti;
                segnalazioneCreate.segnalazione.contatto = contatto;
            }
            return View(segnalazioneCreate);
        }

        //
        // POST: /GestioneSegnalazioni/Create

        [HttpPost]
        public ActionResult Create(SegnalazioneCreate segnalazioneCreate)
        { 
           
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            SegnalazioneBusiness segnalazioneBusiness = new SegnalazioneBusiness();
            Segnalazione segnalazione= segnalazioneCreate.segnalazione;
            segnalazione.contatto.impieghi=segnalazioneCreate.impieghi;
            segnalazione.stato = db.statiSegnalazione.Find(21);
            foreach (Nota nota  in segnalazioneCreate.note){
                if (nota.valore != null)
                {
                    if (segnalazione.note == null)
                    {
                        segnalazione.note = new List<Nota>();
                    }
                     segnalazione.note.Add(nota);
                }
            }
            segnalazione.contatto.riferimenti=segnalazioneCreate.riferimenti;
           
            segnalazione = segnalazioneBusiness.popolaDatiSegnalazione(segnalazione, HttpContext.User.Identity.Name,db);
            ModelState.Clear();
            TryValidateModel(segnalazione);
            if (ModelState.IsValid)
            {
                segnalazioneBusiness.create(segnalazione, HttpContext.User.Identity.Name, db);
                return RedirectToAction("Index", "GestioneSegnalazioni", new { message = "inserimento segnalazione" + segnalazione.contatto.nome + " " + segnalazione.contatto.cognome + " avvenuto con successo" });
            }
            valorizzaDatiViewBagSegnalazione(segnalazioneCreate, db);

            return View(segnalazione);
        }

        [HttpGet]
        public ActionResult segnalazionePartialById(int id, EnumTipoAzione tipoAzione)
        {
             MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
             Segnalazione segnalazione = new SegnalazioneBusiness().findByPk(id, db);
             return dispatch(segnalazione, tipoAzione, db);

        }

        private ActionResult dispatch(Segnalazione segnalazione, EnumTipoAzione tipoAzione, MainDbContext db)
        {
            if (segnalazione.preventivi == null) { segnalazione.preventivi = new List<Preventivo>(); }
           switch (tipoAzione){
               case EnumTipoAzione.INSERIMENTO:
               case EnumTipoAzione.MODIFICA :
                   valorizzaDatiViewBag(db);
                   return View("SegnalazionePartialEdit", segnalazione);
               default:
                   return View("SegnalazionePartialDetail", segnalazione);
           }
        }
        //
        // GET: /GestioneSegnalazioni/Edit/5
        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            Segnalazione segnalazione = db.Segnalazioni.Find(id);
            if (segnalazione == null)
            {
                return HttpNotFound();
            }
            valorizzaDatiViewBag(db);
            return View(segnalazione);
        }

        //
        // POST: /GestioneSegnalazioni/Edit/5

        [HttpPost]
        public ActionResult Edit(Segnalazione segnalazione)
        {
              MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
           
          
            if (ModelState.IsValid)
            {

                Segnalazione segnalazioneOriginale = db.Segnalazioni.Find(segnalazione.id);
                segnalazioneOriginale = (Segnalazione)CopyObject.copy(segnalazioneOriginale, segnalazione);
                db.SaveChanges();
                return RedirectToAction("Index", "GestioneSegnalazioni", new { message = "Aggiornamento " + segnalazione.contatto.nome + " " + segnalazione.contatto.cognome + " efettuato con successo" });
            }

            valorizzaDatiViewBag( db);
            return View(segnalazione);
        }

       
     

        public void valorizzaDatiViewBag(MainDbContext db)
        {
            ViewBag.listaProdotti = new SelectList(db.TipoProdotto.ToList(), "id", "descrizione");
            ViewBag.listaTipoLuogoRitrovo = new SelectList(db.TipoLuogoRitrovo.ToList(), "id", "descrizione");
            ViewBag.listaTipoPrestito = new SelectList(db.TipoPrestito.ToList(), "id", "descrizione");
            ViewBag.listaTipoContatto = new SelectList(db.TipoContatto.ToList(), "id", "descrizione");
            ViewBag.listaTipoCanaleAcquisizione = new SelectList(db.TipoCanaleAcquisizione.ToList(), "id", "descrizione");
            ViewBag.listaTipoAzienda = new SelectList(db.TipoAzienda.ToList(), "id", "descrizione");
            ViewBag.listaStati = new SelectList(
                db.statiSegnalazione.Where(s => s.entitaAssociata==EnumEntitaAssociataStato.SEGNALAZIONE).ToList(),"id","descrizione");
            ViewBag.listaFontePubblicitaria = new SelectList(db.FontiPubblicitarie.ToList(), "id", "descrizione");

            ViewBag.listaSesso = new List<SelectListItem>() { new SelectListItem { Text = "M", Value = "M" },
                                                             new SelectListItem { Text = "F", Value = "F"}};
           
        }
        public SegnalazioneCreate valorizzaDatiViewBagSegnalazione(SegnalazioneCreate segnalazioneCreate, MainDbContext db)
        {
            valorizzaDatiViewBag(db);
            segnalazioneCreate.segnalazione = new Segnalazione();
            if (segnalazioneCreate.segnalazione.note == null)
            {
                segnalazioneCreate.segnalazione.note = new List<Nota>();
                segnalazioneCreate.segnalazione.note.Add(new Nota());
            }
           

            if (segnalazioneCreate.impieghi == null)
            {
                segnalazioneCreate.impieghi = new List<Impiego>();
                segnalazioneCreate.impieghi.Add(new Impiego());
            }
            if (segnalazioneCreate.note == null)
            {
                segnalazioneCreate.note = new List<Nota>();
                segnalazioneCreate.note.Add(new Nota());
            }
            if (segnalazioneCreate.riferimenti == null)
            {
                segnalazioneCreate.riferimenti = new List<Riferimento>();
                segnalazioneCreate.riferimenti.Add(new Riferimento());
            }
            if (segnalazioneCreate.riferimenti == null)
            {
                segnalazioneCreate.riferimenti = new List<Riferimento>();
                segnalazioneCreate.riferimenti.Add(new Riferimento());
            }
            if (segnalazioneCreate.segnalazione.contatto == null)
            {
                segnalazioneCreate.segnalazione.contatto = new Contatto();
            }
            ViewBag.listaTipiContratto = new SelectList(db.TipoContrattoImpiego.ToList(), "id", "descrizione");
            ViewBag.listaCategoriaImpiego = new SelectList(db.TipoCategoriaImpiego.ToList(), "id", "descrizione");
            return segnalazioneCreate;
        }
        protected override void Dispose(bool disposing)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            db.Dispose();
            base.Dispose(disposing);
        }
    }
    public class segnalazioneListValues
    {
        public List<SelectListItem> listaTipologiaPrestito { get; set; }
        public List<SelectListItem> listaSesso { get; set; }
        public List<SelectListItem> listaTipologiaAzienda { get; set; }
        public List<SelectListItem> listaFontePubblicitaria { get; set; }
    }

}

