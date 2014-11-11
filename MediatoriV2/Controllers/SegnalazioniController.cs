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
using System.Diagnostics;

namespace mediatori.Controllers
{
    public class SegnalazioniController : MyBaseController
    {

        public ActionResult Index(SegnalazioneSearch segnalazioniSearch, String message)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);

            ViewBag.message = message;
            return View(new SegnalazioneBusiness().findByFilter(segnalazioniSearch, db));
        }



        public ActionResult Assegna(int id)
        {
            int segnalazioneId = id;
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);

            Segnalazione segnalazione = db.Segnalazioni.Include("stato").First(d => d.id == segnalazioneId);
            if (segnalazione == null)
            {
                return HttpNotFound();
            }


            mediatori.Models.etc.Assegnazione assegnazione = new Assegnazione();
            assegnazione.dataInserimento = DateTime.Now;
            assegnazione.segnalazioneId = segnalazioneId;
            //assegnazione.segnalazione = segnalazione;
            assegnazione.statoId = segnalazione.stato.id;
            assegnazione.login = User.Identity.Name;

            db.Assegnazioni.Add(assegnazione);
            db.SaveChanges();



            return RedirectToAction("Assegnazioni", "Home");
        }


        //
        //// GET: /GestioneSegnalazioni/Details/5
        //[HttpGet]
        //public ActionResult Details(int id = 0)
        //{
        //    MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
        //    Segnalazione segnalazione = new SegnalazioneBusiness().findByPk(id, db);

        //    List<mediatori.Models.Anagrafiche.TipoDocumento> tipoDocumento;
        //    tipoDocumento = db.TipoDocumenti.OrderBy(p => p.descrizione).ToList();
        //    ViewData["TIPO_DOCUMENTO"] = tipoDocumento;

        //   if (segnalazione == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(segnalazione);
        //}

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



       


        [HttpGet]
        public ActionResult Create(Models.Segnalazione.SegnalazioneCreateModel model)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);

            valorizzaDatiViewBag(db);


#if DEBUG
            model.segnalazione.contatto.nome = "Nome TEST";
            model.segnalazione.contatto.cognome = "Cognome TEST";
            model.segnalazione.contatto.dataNascita = new DateTime(1975, 11, 7);
            model.segnalazione.contatto.codiceFiscale = "TTTVVV75S07H444B";
            model.segnalazione.contatto.sesso = EnumSesso.MASCHIO;

#endif

            //model.impieghi.Add(new Impiego());
            //model.riferimenti.Add(new Riferimento());


            return View(model);
        }



        [HttpPost]
        [ActionName("Create")]
        public ActionResult CreatePost(Models.Segnalazione.SegnalazioneCreateModel model)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);

            Segnalazione segnalazione = model.segnalazione;
            if (segnalazione.contatto.impieghi == null)
            {
                segnalazione.contatto.impieghi = new List<mediatori.Models.Anagrafiche.Impiego>();
            }
            segnalazione.contatto.impieghi.Add(model.impiego);



            if (segnalazione.contatto.riferimenti == null)
            {
                segnalazione.contatto.riferimenti = new List<mediatori.Models.Anagrafiche.Riferimento>();
            }
            segnalazione.contatto.riferimenti.Add(model.riferimento);


            if (segnalazione.note == null)
            {
                segnalazione.note = new List<mediatori.Models.Nota>();
            }
            segnalazione.note.Add(model.nota);



            SegnalazioneBusiness segnalazioneBusiness = new SegnalazioneBusiness();
            segnalazione = segnalazioneBusiness.popolaDatiSegnalazione(segnalazione, HttpContext.User.Identity.Name, db);

            ModelState.Clear();
            TryValidateModel(segnalazione);
            if (ModelState.IsValid)
            {

                segnalazione.stato = db.statiSegnalazione.Find(MyConstants.STATO_INIZIALE_SEGNALAZIONE);

                if (segnalazione.stato == null)
                {
                    throw new MyManagerCSharp.MyException("Stato iniziale della segnalazione NON valido");
                }

                db.Segnalazioni.Add(segnalazione);

                try
                {
                    db.SaveChanges();
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, "Inserimento segnalazione" + model.segnalazione.contatto.nome + " " + model.segnalazione.contatto.cognome + " avvenuto con successo");
                    return RedirectToAction("Index", "Segnalazioni");
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    string messaggio;
                    messaggio = MyHelper.getDbEntityValidationException(ex);
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare il soggetto giuridico , verificare i dati: " + Environment.NewLine + messaggio);
                }
            }


            valorizzaDatiViewBag(db);

            var message = string.Join(" | ", ModelState.Values
               .SelectMany(v => v.Errors)
               .Select(e => e.ErrorMessage));

            Debug.WriteLine(message);

            if (TempData["Message"] == null)
            {
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare la segnalazione , verificare i dati: " + Environment.NewLine + message);
            }

            return View(model);
        }





        //
        // POST: /GestioneSegnalazioni/Create

        [HttpPost]
        public ActionResult CreateOLD(SegnalazioneCreate segnalazioneCreate)
        {

            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            SegnalazioneBusiness segnalazioneBusiness = new SegnalazioneBusiness();
            Segnalazione segnalazione = segnalazioneCreate.segnalazione;
            segnalazione.contatto.impieghi = segnalazioneCreate.impieghi;
            segnalazione.stato = db.statiSegnalazione.Find(1);
            foreach (Nota nota in segnalazioneCreate.note)
            {
                if (nota.valore != null)
                {
                    if (segnalazione.note == null)
                    {
                        segnalazione.note = new List<Nota>();
                    }
                    segnalazione.note.Add(nota);
                }
            }
            segnalazione.contatto.riferimenti = segnalazioneCreate.riferimenti;

            segnalazione = segnalazioneBusiness.popolaDatiSegnalazione(segnalazione, HttpContext.User.Identity.Name, db);
            ModelState.Clear();
            TryValidateModel(segnalazione);
            if (ModelState.IsValid)
            {
                segnalazioneBusiness.create(segnalazione, HttpContext.User.Identity.Name, db);
                return RedirectToAction("Index", "Segnalazioni", new { message = "inserimento segnalazione" + segnalazione.contatto.nome + " " + segnalazione.contatto.cognome + " avvenuto con successo" });
            }

            var message = string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));

            Debug.WriteLine(message);

            ViewBag.erroMessage = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare la segnalazione , verificare i dati: " + Environment.NewLine + message);

            valorizzaDatiViewBagSegnalazione(segnalazioneCreate, db);

            return View(segnalazioneCreate);
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
            switch (tipoAzione)
            {
                case EnumTipoAzione.INSERIMENTO:
                case EnumTipoAzione.MODIFICA:
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
                return RedirectToAction("Index", "Segnalazioni", new { message = "Aggiornamento " + segnalazione.contatto.nome + " " + segnalazione.contatto.cognome + " efettuato con successo" });
            }

            valorizzaDatiViewBag(db);
            return View(segnalazione);
        }




        public void valorizzaDatiViewBag(MainDbContext db)
        {
            ViewBag.listaTipiContratto = new SelectList(db.TipoContrattoImpiego, "id", "descrizione");
            ViewBag.listaProdotti = new SelectList(db.TipoProdotto.ToList(), "id", "descrizione");
            ViewBag.listaTipoLuogoRitrovo = new SelectList(db.TipoLuogoRitrovo.ToList(), "id", "descrizione");
            ViewBag.listaTipoPrestito = new SelectList(db.TipoPrestito.ToList(), "id", "descrizione");
            ViewBag.listaTipoContatto = new SelectList(db.TipoContatto.ToList(), "id", "descrizione");
            ViewBag.listaTipoCanaleAcquisizione = new SelectList(db.TipoCanaleAcquisizione.ToList(), "id", "descrizione");
            ViewBag.listaTipoAzienda = new SelectList(db.TipoAzienda.ToList(), "id", "descrizione");
            ViewBag.listaStati = new SelectList(db.statiSegnalazione.Where(s => s.entitaAssociata == EnumEntitaAssociataStato.SEGNALAZIONE).ToList(), "id", "descrizione");
            ViewBag.listaFontePubblicitaria = new SelectList(db.FontiPubblicitarie.ToList(), "id", "descrizione");
            ViewBag.listaSesso = new SelectList(new List<SelectListItem> { new SelectListItem { Text = "M", Value = "M" }, new SelectListItem { Text = "F", Value = "F" } }, null);
        }

        public SegnalazioneCreate valorizzaDatiViewBagSegnalazione(SegnalazioneCreate segnalazioneCreate, MainDbContext db)
        {
            valorizzaDatiViewBag(db);

            if (segnalazioneCreate.segnalazione == null)
            {

                segnalazioneCreate.segnalazione = new Segnalazione();
            }


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
    
    }
    public class segnalazioneListValues
    {
        public List<SelectListItem> listaTipologiaPrestito { get; set; }
        public List<SelectListItem> listaSesso { get; set; }
        public List<SelectListItem> listaTipologiaAzienda { get; set; }
        public List<SelectListItem> listaFontePubblicitaria { get; set; }
    }

}

