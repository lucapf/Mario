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
            Segnalazione segnalazione;

            //segnalazione = db.Segnalazioni.Include("prodottoRichiesto").Include("contatto").Include("contatto.provinciaNascita").Include("contatto.comuneNascita").Include("stato").Include("note").Where(s => s.id == id).First();

            segnalazione = db.Segnalazioni.Include("prodottoRichiesto").Include("stato").Include("note").Where(s => s.id == id).First();

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

            valorizzaViewBag();
            Models.Anagrafiche.Contatto contatto;

            if (codiceContatto != null)
            {
                //   c = db.Contatti.Find(codiceContatto);
                contatto = db.Contatti.Include("provinciaNascita").Include("comuneNascita").Where(p => p.id == (int)codiceContatto).First();
                if (contatto != null)
                {
                    model.contatto = contatto;

                    //if (model.segnalazione.contatto.provinciaNascita != null)
                    //{
                    //    ViewBag.listaComuni = new SelectList(db.Comuni.Where(c => c.codiceProvincia == model.segnalazione.contatto.provinciaNascita.id).ToList(), "denominazione", "denominazione");
                    //}
                }
            }

            List<TipoConsensoPrivacy> lstTipiConsensoPrivacy = db.TipoConsensoPrivacy.Where(t => t.attivo == true).ToList();
            foreach (TipoConsensoPrivacy tcp in lstTipiConsensoPrivacy)
            {
                ConsensoPrivacy consensoPrivacy = new ConsensoPrivacy();
                consensoPrivacy.tipoConsensoPrivacy = tcp;
                model.consensoPrivacy.Add(consensoPrivacy);
            }
            return View(model);
        }



        [HttpPost]
        [ActionName("Create")]
        public ActionResult CreatePost(Models.Segnalazione.SegnalazioneCreateModel model)
        {
            Segnalazione segnalazione = model.segnalazione;

            segnalazione.contatto = model.contatto;

            if (segnalazione.contatto.id != 0)
            {
                segnalazione.contatto = db.Contatti.Include("provinciaNascita").Include("comuneNascita").Where(p => p.id == segnalazione.contatto.id).FirstOrDefault();
            }
            else
            {
                segnalazione.contatto.provinciaNascita = db.Province.Where(p => p.denominazione == model.contatto.provinciaNascita.denominazione).FirstOrDefault();
                segnalazione.contatto.comuneNascita = db.Comuni.Where(c => c.denominazione == model.contatto.comuneNascita.denominazione && c.codiceProvincia == model.contatto.provinciaNascita.id).FirstOrDefault();


                if (segnalazione.contatto.impieghi == null)
                {
                    segnalazione.contatto.impieghi = new List<mediatori.Models.Anagrafiche.Impiego>();
                }
                segnalazione.contatto.impieghi.Add(mediatori.Controllers.Business.Anagrafiche.Soggetto.ImpiegoBusiness.valorizzaDatiImpiego(model.impiego, db));


                if (segnalazione.contatto.riferimenti == null)
                {
                    segnalazione.contatto.riferimenti = new List<mediatori.Models.Anagrafiche.Riferimento>();
                }
                segnalazione.contatto.riferimenti.Add(mediatori.Controllers.Business.Anagrafiche.Soggetto.RiferimentoBusiness.valorizzaDatiRiferimento(model.riferimento, db));

            }



            if (segnalazione.note == null)
            {
                segnalazione.note = new List<mediatori.Models.Nota>();
            }
            segnalazione.note.Add(model.nota);


            //Rel. 1.0.0.13
            foreach (mediatori.Models.Anagrafiche.ConsensoPrivacy consenso in model.consensoPrivacy)
            {
                consenso.dataInserimento = DateTime.Now;
                consenso.untenteInserimento = User.Identity.Name;
                consenso.tipoConsensoPrivacy = db.TipoConsensoPrivacy.Find(consenso.tipoConsensoPrivacy.id);
            }

            segnalazione.consensoPrivacy = model.consensoPrivacy;




            SegnalazioneBusiness segnalazioneBusiness = new SegnalazioneBusiness();
            segnalazione = segnalazioneBusiness.popolaDatiSegnalazione(segnalazione, HttpContext.User.Identity.Name, db);

            ModelState.Clear();
            TryValidateModel(segnalazione);
            if (ModelState.IsValid)
            {
                foreach (ConsensoPrivacy consensoPrivacy in segnalazione.consensoPrivacy)
                {                  
                    if (consensoPrivacy.acconsento == false && consensoPrivacy.tipoConsensoPrivacy.obbligatorio)
                    {                  
                        TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Attenzione! per poter procedere al salvataggio della segnalazione è necessario acconsentire al consenso privacy per il  al trattamento dei dati personali ");
                        valorizzaViewBag();

                        model.contatto = model.segnalazione.contatto;
                        return View(model);
                    }
                }

                segnalazione.stato = db.StatiSegnalazione.Where(p => p.descrizione == MyConstants.STATO_INIZIALE_SEGNALAZIONE).FirstOrDefault();
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


            valorizzaViewBag();

            model.contatto = model.segnalazione.contatto;

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
        public ActionResult SegnalazionePartialById(int id, EnumTipoAzione tipoAzione)
        {
            //Segnalazione segnalazione = new SegnalazioneBusiness().findByPk(id, db);
            Segnalazione segnalazione;

            segnalazione = db.Segnalazioni.Find(id);



            if (segnalazione == null)
            {
                return HttpNotFound();
            }

            if (tipoAzione == EnumTipoAzione.MODIFICA)
            {
                valorizzaViewBag();
                //  ViewData.TemplateInfo.HtmlFieldPrefix = "indirizzo";
                return View("SegnalazioneEdit", segnalazione);
            }

            if (tipoAzione == EnumTipoAzione.VISUALIZZAZIONE)
            {
                return View("SegnalazionePartialDetail", segnalazione);
            }

            //  valorizzaViewBag();
            //return View("impiegoPartialEdit", impiego);
            throw new ApplicationException("Azione di inserimento che non si deve presentare");


            //return dispatch(segnalazione, tipoAzione, db);

        }

        private ActionResult dispatch(Segnalazione segnalazione, EnumTipoAzione tipoAzione, MainDbContext db)
        {
            if (segnalazione.preventivi == null) { segnalazione.preventivi = new List<PreventivoSmall>(); }
            switch (tipoAzione)
            {
                case EnumTipoAzione.INSERIMENTO:
                case EnumTipoAzione.MODIFICA:
                    valorizzaViewBag();
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
            valorizzaViewBag();
            return View(segnalazione);
        }


        [HttpPost]
        public ActionResult Edit(Segnalazione segnalazione)
        {
            //ModelState.Remove("tipoContatto.descrizione");
            //ModelState.Remove("tipoAzienda.descrizione");
            //ModelState.Remove("prodottoRichiesto.descrizione");
            //ModelState.Remove("campagnaPubblicitaria.descrizione");
            //ModelState.Remove("canaleAcquisizione.descrizione");
            //ModelState.Remove("altroPrestito.descrizione");
            //ModelState.Remove("fontePubblicitaria.descrizione");
            //ModelState.Remove("tipoLuogoRitrovo.descrizione");

            SegnalazioneBusiness segnalazioneBusiness = new SegnalazioneBusiness();
            segnalazione = segnalazioneBusiness.popolaDatiSegnalazione(segnalazione, db);

            ModelState.Clear();
            TryValidateModel(segnalazione);

            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                  .SelectMany(v => v.Errors)
                  .Select(e => e.ErrorMessage));
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare la segnalazione, verificare i dati: " + Environment.NewLine + message);
                return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
            }


            Segnalazione segnalazioneOriginale = db.Segnalazioni.Include("Contatto").Where(p => p.id == segnalazione.id).First();


            if (segnalazioneOriginale == null)
            {
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare la segnalazione");
                return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
            }

            //dati segnalazione
            segnalazioneOriginale.altroPrestito = segnalazione.altroPrestito;
            segnalazioneOriginale.importoRichiesto = segnalazione.importoRichiesto;
            segnalazioneOriginale.rataRichiesta = segnalazione.rataRichiesta;
            segnalazioneOriginale.durataRichiesta = segnalazione.durataRichiesta;
            segnalazioneOriginale.prodottoRichiesto = segnalazione.prodottoRichiesto;

            //Dati campagnia
            segnalazioneOriginale.fontePubblicitaria = segnalazione.fontePubblicitaria;
            segnalazioneOriginale.canaleAcquisizione = segnalazione.canaleAcquisizione;
            segnalazioneOriginale.tipoLuogoRitrovo = segnalazione.tipoLuogoRitrovo;
            segnalazioneOriginale.tipoContatto = segnalazione.tipoContatto;


            try
            {
                db.SaveChanges();
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, "Modifiche salvate con successo");
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                string messaggio;
                messaggio = MyHelper.getDbEntityValidationException(ex);
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare la segnalazione, verificare i dati: " + Environment.NewLine + messaggio);
            }
            catch (Exception ex)
            {
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare la segnalazione, verificare i dati: " + Environment.NewLine + ex.Message);
            }

            return RedirectToAction("Details", "Segnalazioni", new { id = segnalazione.id });

            // return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
            //return RedirectToAction("Index", "Segnalazioni", new { message = "Aggiornamento " + segnalazione.contatto.nome + " " + segnalazione.contatto.cognome + " efettuato con successo" });
        }




        public void valorizzaViewBag()
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
            ViewBag.listaTipoConsensoPrivacy = new SelectList(db.TipoConsensoPrivacy.Where(t => t.attivo == true).ToList(), "id", "descrizione");
            //ViewBag.listaSesso = new SelectList(new List<SelectListItem> { new SelectListItem { Text = "M", Value = "M" }, new SelectListItem { Text = "F", Value = "F" } }, null);


            //ViewBag.listaProvincia = new SelectList(db.Province.ToList(), "denominazione", "denominazione");

            //List<SelectListItem> lsli = new List<SelectListItem>();
            //lsli.Add(new SelectListItem { Text = "", Value = "" });
            //ViewBag.listaComuni = lsli;
        }

        public ActionResult GetListSegnalazioniContatto(int id)
        {
            ICollection<Segnalazione> listSegnalazioni = db.Segnalazioni.Include("stato").Include("prodottoRichiesto").Where(s => s.contattoId == id).ToList();
            return View("ListaSegnalazioni", listSegnalazioni);
        }
    }
}

