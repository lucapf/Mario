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

        public ActionResult Index(SegnalazioneSearch segnalazioniSearch)
        {
            IQueryable<Segnalazione> listaSegnalazioni = db.Segnalazioni.Include("contatto").Include("prodottoRichiesto").Include("stato");

            if (segnalazioniSearch.cognome != null)
            {
                listaSegnalazioni = listaSegnalazioni.Where(s => s.contatto.cognome == segnalazioniSearch.cognome);

            }
            if (segnalazioniSearch.nome != null)
            {
                listaSegnalazioni = listaSegnalazioni.Where(s => s.contatto.nome == segnalazioniSearch.nome);

            }
            if (segnalazioniSearch.dataInserimentoA != null)
            {
                listaSegnalazioni = listaSegnalazioni.Where(s => s.dataInserimento <= segnalazioniSearch.dataInserimentoA);

            }
            if (segnalazioniSearch.dataInserimentoDa != null)
            {
                listaSegnalazioni = listaSegnalazioni.Where(s => s.dataInserimento >= segnalazioniSearch.dataInserimentoDa);

            }

            Debug.WriteLine("Profilo: " + (Session["MySessionData"] as MyManagerCSharp.MySessionData).Profili);
            if ((Session["MySessionData"] as MyManagerCSharp.MySessionData).Profili.IndexOf(MyConstants.Profilo.COLLABORATORE.ToString()) > -1)
            {
                listaSegnalazioni = listaSegnalazioni.Where(p => p.utenteInserimento == User.Identity.Name);
            }

            listaSegnalazioni.OrderByDescending(s => s.id);
            listaSegnalazioni.Take(50);
            List<Segnalazione> segnalazioni = listaSegnalazioni.Where(o => !(o is Models.Pratica.Pratica)).ToList();

            return View(segnalazioni);
        }

        public ActionResult Assegna(int id)
        {
            int segnalazioneId = id;

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

            //Segnalazione segnalazione = new SegnalazioneBusiness().findByPk(id, db);
            Segnalazione segnalazione;

            segnalazione = db.Segnalazioni.Include("contatto").Include("stato").Include("note").Where(s => s.id == id).First();

            if (segnalazione == null)
            {
                return HttpNotFound();
            }
            mediatori.Models.SegnalazioneDetailsModel model = new SegnalazioneDetailsModel();
            model.segnalazione = segnalazione;


            // StatoSearch statoSearch = new StatoSearch();

            //statoSearch.successiviDi = segnalazione.stato.id;
            //statoSearch.entita = EnumEntitaAssociataStato.SEGNALAZIONE;

            model.listaStatiSuccessivi = new mediatori.Controllers.Business.CQS.StatoBusiness().getStatiSuccessivi(segnalazione.stato, db);

            return View(model);
        }


        [HttpGet]
        public ActionResult Create(int? codiceContatto, Models.Segnalazione.SegnalazioneCreateModel model)
        {
            Debug.WriteLine("codiceContatto: " + codiceContatto);

            valorizzaDatiViewBag();

            if (codiceContatto != null)
            {
                Models.Anagrafiche.Contatto contatto = db.Contatti.Find(codiceContatto);
                if (contatto != null)
                {
                    model.segnalazione.contatto = contatto;

                    if (model.segnalazione.contatto.provinciaNascita != null)
                    {
                        ViewBag.listaComuni = new SelectList(db.Comuni.Where(c => c.codiceProvincia == model.segnalazione.contatto.provinciaNascita.id).ToList(), "denominazione", "denominazione");
                    }
                }
            }


            //model.segnalazione.contatto.nome = "Nome TEST";
            //model.segnalazione.contatto.cognome = "Cognome TEST";
            //model.segnalazione.contatto.dataNascita = new DateTime(1975, 11, 7);
            //model.segnalazione.contatto.codiceFiscale = "TTTVVV75S07H444B";
            //model.segnalazione.contatto.sesso = EnumSesso.MASCHIO;

            return View(model);
        }



        [HttpPost]
        [ActionName("Create")]
        public ActionResult CreatePost(Models.Segnalazione.SegnalazioneCreateModel model)
        {
            Segnalazione segnalazione = model.segnalazione;
            segnalazione.contatto.provinciaNascita = db.Province.Where(p => p.denominazione == segnalazione.contatto.provinciaNascita.denominazione).FirstOrDefault();
            segnalazione.contatto.comuneNascita = db.Comuni.Where(c => c.denominazione == segnalazione.contatto.comuneNascita.denominazione && c.codiceProvincia == segnalazione.contatto.provinciaNascita.id).FirstOrDefault();

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
                segnalazione.stato = db.StatiSegnalazione.Find(MyConstants.STATO_INIZIALE_SEGNALAZIONE);

                if (segnalazione.stato == null)
                {
                    throw new MyManagerCSharp.MyException("Stato iniziale della segnalazione NON valido");
                }

                db.Segnalazioni.Add(segnalazione);

                try
                {
                    db.SaveChanges();
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, "Inserimento segnalazione per " + model.segnalazione.contatto.nome + " " + model.segnalazione.contatto.cognome + " avvenuto con successo");
                    return RedirectToAction("Index", "Segnalazioni");
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    string messaggio;
                    messaggio = MyHelper.getDbEntityValidationException(ex);
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare il soggetto giuridico , verificare i dati: " + Environment.NewLine + messaggio);
                }
            }


            valorizzaDatiViewBag();

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


        [HttpGet]
        public ActionResult segnalazionePartialById(int id, EnumTipoAzione tipoAzione)
        {
            Segnalazione segnalazione = new SegnalazioneBusiness().findByPk(id, db);
            return dispatch(segnalazione, tipoAzione, db);

        }

        private ActionResult dispatch(Segnalazione segnalazione, EnumTipoAzione tipoAzione, MainDbContext db)
        {
            if (segnalazione.preventivi == null) { segnalazione.preventivi = new List<PreventivoSmall>(); }
            switch (tipoAzione)
            {
                case EnumTipoAzione.INSERIMENTO:
                case EnumTipoAzione.MODIFICA:
                    valorizzaDatiViewBag();
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
            Segnalazione segnalazione = db.Segnalazioni.Find(id);
            if (segnalazione == null)
            {
                return HttpNotFound();
            }
            valorizzaDatiViewBag();
            return View(segnalazione);
        }

        //
        // POST: /GestioneSegnalazioni/Edit/5

        [HttpPost]
        public ActionResult Edit(Segnalazione segnalazione)
        {
            if (ModelState.IsValid)
            {
                Segnalazione segnalazioneOriginale = db.Segnalazioni.Find(segnalazione.id);
                segnalazioneOriginale = (Segnalazione)CopyObject.copy(segnalazioneOriginale, segnalazione);
                db.SaveChanges();
                return RedirectToAction("Index", "Segnalazioni", new { message = "Aggiornamento " + segnalazione.contatto.nome + " " + segnalazione.contatto.cognome + " efettuato con successo" });
            }

            valorizzaDatiViewBag();
            return View(segnalazione);
        }




        public void valorizzaDatiViewBag()
        {
            ViewBag.listaTipiContratto = new SelectList(db.TipoContrattoImpiego, "id", "descrizione");
            ViewBag.listaProdotti = new SelectList(db.TipoProdotto.ToList(), "id", "descrizione");
            ViewBag.listaTipoLuogoRitrovo = new SelectList(db.TipoLuogoRitrovo.ToList(), "id", "descrizione");
            ViewBag.listaTipoPrestito = new SelectList(db.TipoPrestito.ToList(), "id", "descrizione");
            ViewBag.listaTipoContatto = new SelectList(db.TipoContatto.ToList(), "id", "descrizione");
            ViewBag.listaTipoCanaleAcquisizione = new SelectList(db.TipoCanaleAcquisizione.ToList(), "id", "descrizione");
            ViewBag.listaTipoAzienda = new SelectList(db.TipoAzienda.ToList(), "id", "descrizione");
            ViewBag.listaStati = new SelectList(db.StatiSegnalazione.Where(s => s.entitaAssociata == EnumEntitaAssociataStato.SEGNALAZIONE).ToList(), "id", "descrizione");
            ViewBag.listaFontePubblicitaria = new SelectList(db.FontiPubblicitarie.ToList(), "id", "descrizione");
            ViewBag.listaSesso = new SelectList(new List<SelectListItem> { new SelectListItem { Text = "M", Value = "M" }, new SelectListItem { Text = "F", Value = "F" } }, null);


            ViewBag.listaProvincia = new SelectList(db.Province.ToList(), "denominazione", "denominazione");

            List<SelectListItem> lsli = new List<SelectListItem>();
            lsli.Add(new SelectListItem { Text = "", Value = "" });
            ViewBag.listaComuni = lsli;
        }
    }
}

