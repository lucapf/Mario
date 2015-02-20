using BusinessModel.Log;
using mediatori.Controllers.Business.Anagrafiche;
using mediatori.Models;
using mediatori.Models.Anagrafiche;
using mediatori.Models.Configurazione;
using mediatori.Models.etc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mediatori.Controllers
{

    [MyAuthorize(Roles = new string[] { MyConstants.Profilo.AMMINISTRATORE })]
    public class ConfigurazioniController : MyBaseController
    {

        public ActionResult Index()
        {

            List<MenuElement> model = new List<MenuElement>(){
                    //new MenuElement(){display="Home", ordinamento=1,livello=1,role="Amministratore",action="Index", controller="Home"},
                    //new MenuElement(){display="Configurazioni", ordinamento=1,livello=1,role="Amministratore",action="Index", controller="Configurazioni"},
                    new MenuElement(){display="Canale Acquisizione", ordinamento=1,livello=1,role="Amministratore",action="canaleAcquisizione",controller="Configurazioni"},
                    new MenuElement(){display="Fonti Pubblicitarie", ordinamento=1,livello=1,role="Amministratore",action="fontePubblicitaria",controller="Configurazioni"},
                    new MenuElement(){display="Province", ordinamento=1,livello=1,role="Amministratore",action="Province", controller="Configurazioni"},
                    new MenuElement(){display="Stato", ordinamento=1,livello=1,role="Amministratore",action="stato",controller="Configurazioni"},
                    new MenuElement(){display="Toponimi", ordinamento=1,livello=1,role="Amministratore",action="Toponimi",controller="Configurazioni"},
                    new MenuElement(){display="Assumibilità Amministrazione", ordinamento=1,livello=1,role="Amministratore",action="assumibilitaAmministrazione",controller="Configurazioni"},
                    new MenuElement(){display="Tipo Agenzia", ordinamento=1,livello=1,role="Amministratore",action="tipoAgenzia",controller="Configurazioni"},
                    new MenuElement(){display="Tipo Azienda", ordinamento=1,livello=1,role="Amministratore",action="tipologiaAzienda",controller="Configurazioni"},
                    new MenuElement(){display="Tipo Categoria", ordinamento=1,livello=1,role="Amministratore",action="tipoCategoriaAmministrazione",controller="Configurazioni"},
                    new MenuElement(){display="Tipo Contatto", ordinamento=1,livello=1,role="Amministratore",action="tipoContatto",controller="Configurazioni"},
                    new MenuElement(){display="Tipo Documento Identita", ordinamento=1,livello=1,role="Amministratore",action="tipoDocumentoIdentita",controller="Configurazioni"},
                    new MenuElement(){display="Tipo Ente Rilascio", ordinamento=1,livello=1,role="Amministratore",action="tipoEnteRilascio",controller="Configurazioni"},
                    new MenuElement(){display="Tipo Erogazione", ordinamento=1,livello=1,role="Amministratore",action="tipoErogazione",controller="Configurazioni"},
                    new MenuElement(){display="Tipo Impiego", ordinamento=1,livello=1,role="Amministratore",action="tipoContrattoImpiego",controller="Configurazioni"},
                    new MenuElement(){display="Tipo Indirizzo", ordinamento=1,livello=1,role="Amministratore",action="tipologiaIndirizzo",controller="Configurazioni"},
                    new MenuElement(){display="Tipo Luogo Ritrovo", ordinamento=1,livello=1,role="Amministratore",action="tipoLuogoRitrovo",controller="Configurazioni"},
                    new MenuElement(){display="Tipo Prestito", ordinamento=1,livello=1,role="Amministratore",action="tipologiaPrestito",controller="Configurazioni"},
                    new MenuElement(){display="Tipo Prodotto", ordinamento=1,livello=1,role="Amministratore",action="tipoProdotto",controller="Configurazioni"},
                    new MenuElement(){display="Tipo Riferimento", ordinamento=1,livello=1,role="Amministratore",action="tipoRiferimento",controller="Configurazioni"},
                    new MenuElement(){display="Tipo Consenso Privacy", ordinamento=1,livello=1,role="Amministratore",action="tipoConsensoPrivacy",controller="Configurazioni"}
                };

            //new MenuElement(){display="Nuova Rete ", ordinamento=1,livello=1,role="Amministratore",url="Rete/Create"}
            return View(model);
        }



        #region Province
        //GET : /Configurazioni/Province
        [HttpGet]
        public ActionResult Province()
        {
            List<Provincia> listaProvince = db.Province.ToList();
            return View(listaProvince);
        }

        #endregion province
        #region comuni
        [HttpGet]

        public ActionResult comuni(ComuneSearch comuneSearch)
        {
            if (comuneSearch == null) return View();
            IList<Comune> listComune = null;
            IQueryable<Comune> comuni = db.Comuni.Include("provincia");
            if (comuneSearch.codiceProvincia > 0)
            {
                comuni = comuni.Where(c => c.codiceProvincia == comuneSearch.codiceProvincia);
            }
            if (comuneSearch.denominazione != null && comuneSearch.denominazione != String.Empty)
            {
                comuni = comuni.Where(c => c.denominazione == comuneSearch.denominazione);
            }
            if (comuneSearch.id > 0)
            {
                comuni = comuni.Where(c => c.id == comuneSearch.id);
            }
            listComune = comuni.ToList();
            return View(listComune);
        }



        #endregion comuni
        #region toponimi
        [HttpGet]

        public ActionResult toponimi(ComuneSearch comuneSearch, String errorMessage, String message)
        {

            ViewBag.errorMessage = errorMessage == null ? String.Empty : errorMessage;
            ViewBag.message = message == null ? String.Empty : message;
            return View(
                (from toponimi in db.Toponimi select toponimi).ToList()
                );
        }

        [HttpPost]

        public ActionResult toponimo(String sigla)
        {

            if (db.Toponimi.Find(sigla) != null)
            {
                return RedirectToAction("toponimi", new { errorMessage = "Toponimo " + sigla + " già censito" });
            }
            else
            {

                db.Toponimi.Add(new Toponimo { sigla = sigla });
                db.SaveChanges();

            }

            return RedirectToAction("toponimi", new { message = "inserimento toponimo : " + sigla + " avvenuto con successo" });
        }

        [HttpGet]
        public ActionResult cancellaToponimo(String sigla)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;

            Toponimo toponimo = db.Toponimi.Find(sigla);
            if (toponimo == null)
            {
                errorMessage = "";
            }
            else
            {
                db.Toponimi.Remove(toponimo);
                db.SaveChanges();
                message = "toponimo " + toponimo.sigla + " eliminato con successo";
            }

            return RedirectToAction("toponimi", new { errorMessage = errorMessage, message = message });
        }
        #endregion toponimi


        #region fontePubblicitaria

        [HttpGet]
        public ActionResult fontePubblicitaria()
        {
            Models.Configurazione.ConfigurazioneModel model = new Models.Configurazione.ConfigurazioneModel("Fonte pubblicitaria");

            model.listaEntita = db.FontiPubblicitarie.OrderBy(p => p.descrizione).Select(p => new Models.Configurazione.EntitaModel { id = p.id, descrizione = p.descrizione }).ToList();
            return View("Shared", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult fontePubblicitaria(int id, String descrizione)
        {
            descrizione = descrizione.Trim();

            FontePubblicitaria checkExists;

            if (id == 0)
            {
                //NUOVO
                checkExists = (from fp in db.FontiPubblicitarie where fp.descrizione == descrizione select fp).FirstOrDefault();

                if (checkExists != null)
                {
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, String.Format("Fonte pubblicitaria '{0}' già censita", descrizione));
                }
                else
                {
                    db.FontiPubblicitarie.Add(new FontePubblicitaria { descrizione = descrizione });
                    db.SaveChanges();
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, String.Format("Fonte pubblicitaria '{0}' inserta con successo", descrizione));
                }
            }
            else
            {
                //MODIFICA
                checkExists = db.FontiPubblicitarie.Find(id);
                if (checkExists == null)
                {
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, String.Format("Impossibile modifcare la fonte pubblicitaria '{0}' in quanto non censita", id));
                }
                else
                {
                    checkExists.descrizione = descrizione;

                    db.SaveChanges();
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, String.Format("Fonte pubblicitaria '{0}' modificata con successo", checkExists.descrizione));
                }
            }

            return RedirectToAction("fontePubblicitaria");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult fontePubblicitariaDelete(int id)
        {
            FontePubblicitaria checkExists = db.FontiPubblicitarie.Find(id);
            if (checkExists == null)
            {
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, String.Format("Impossibile eliminare la fonte pubblicitaria '{0}' in quanto non censita", id));
            }
            else
            {
                db.FontiPubblicitarie.Remove(checkExists);
                db.SaveChanges();
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, String.Format("Fonte pubblicitaria '{0}' eliminata con successo", checkExists.descrizione));
            }
            return RedirectToAction("fontePubblicitaria");
        }
        #endregion fontePubblicitaria

        #region tipologiaPrestito
        [HttpGet]

        public ActionResult tipologiaPrestito(String errorMessage, String message)
        {

            ViewBag.errorMessage = errorMessage == null ? String.Empty : errorMessage;
            ViewBag.message = message == null ? String.Empty : message;
            return View(
                (from tipologiaPrestito in db.TipoPrestito select tipologiaPrestito).ToList()
                );
        }

        [HttpPost]

        public ActionResult tipologiaPrestito(String descrizione)
        {

            if ((from fp in db.TipoPrestito
                 where fp.descrizione == descrizione
                 select fp).FirstOrDefault() != null)
            {
                return RedirectToAction("tipologiaPrestito", new
                {
                    errorMessage = "tipologia prestito " + descrizione
                        + " già censita"
                });
            }
            else
            {
                db.TipoPrestito.Add(new TipologiaPrestito { descrizione = descrizione });
                db.SaveChanges();
            }
            return RedirectToAction("tipologiaPrestito", new { message = "inserimento tipologia prestito : " + descrizione + " avvenuta con successo" });
        }

        [HttpGet]

        public ActionResult cancellaTipologiaPrestito(int id)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            TipologiaPrestito tipologiaPrestito = db.TipoPrestito.Find(id);
            if (tipologiaPrestito == null)
            {
                errorMessage = "impossibile eliminare la tipologia prestito " + id + " in quanto non censita";
            }
            else
            {
                db.TipoPrestito.Remove(tipologiaPrestito);
                db.SaveChanges();
                message = "tipologia prestito " + tipologiaPrestito.descrizione + " eliminata con successo";
            }
            return RedirectToAction("tipologiaPrestito", new { errorMessage = errorMessage, message = message });
        }
        #endregion tipologiaPrestito
        #region tipologiaAzienda
        [HttpGet]

        public ActionResult tipologiaAzienda(String errorMessage, String message)
        {

            ViewBag.errorMessage = errorMessage == null ? String.Empty : errorMessage;
            ViewBag.message = message == null ? String.Empty : message;
            return View(
                (from tipologiaAzienda in db.TipoAzienda select tipologiaAzienda).ToList()
                );
        }

        [HttpPost]

        public ActionResult tipologiaAzienda(TipologiaAzienda ta)
        {
            if ((from fp in db.TipoAzienda
                 where (fp.descrizione == ta.descrizione)
                 select fp).FirstOrDefault() != null)
            {
                return RedirectToAction("tipologiaAzienda", new
                {
                    errorMessage = "tipologia Azienda " + ta.descrizione
                        + " già censita"
                });
            }
            else
            {
                db.TipoAzienda.Add(ta);
                db.SaveChanges();
            }
            return RedirectToAction("tipologiaAzienda", new { message = "inserimento tipologia Azienda : " + ta.descrizione + " avvenuta con successo" });
        }

        [HttpGet]

        public ActionResult cancellaTipoAzienda(int id)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            TipologiaAzienda tipologiaAzienda = db.TipoAzienda.Find(id);
            if (tipologiaAzienda == null)
            {
                errorMessage = "impossibile eliminare la tipologia Azienda " + id + " in quanto non censita";
            }
            else
            {
                db.TipoAzienda.Remove(tipologiaAzienda);
                db.SaveChanges();
                message = "tipologia Azienda " + tipologiaAzienda.descrizione + " eliminata con successo";
            }
            return RedirectToAction("tipologiaAzienda", new { errorMessage = errorMessage, message = message });
        }
        #endregion tipologiaAzienda
        #region tipologiaIndirizzo
        [HttpGet]

        public ActionResult tipologiaIndirizzo(String errorMessage, String message)
        {

            ViewBag.errorMessage = errorMessage == null ? String.Empty : errorMessage;
            ViewBag.message = message == null ? String.Empty : message;
            return View(
                (from tipologiaIndirizzo in db.TipoIndirizzo select tipologiaIndirizzo).ToList()
                );
        }

        [HttpPost]

        public ActionResult tipologiaIndirizzo(TipologiaIndirizzo ta)
        {
            if ((from fp in db.TipoIndirizzo
                 where (fp.descrizione == ta.descrizione)
                 select fp).FirstOrDefault() != null)
            {
                return RedirectToAction("tipologiaIndirizzo", new
                {
                    errorMessage = "tipologia Indirizzo " + ta.descrizione
                        + " già censita"
                });
            }
            else
            {
                db.TipoIndirizzo.Add(ta);
                db.SaveChanges();
            }
            return RedirectToAction("tipologiaIndirizzo", new { message = "inserimento tipologia Indirizzo : " + ta.descrizione + " avvenuta con successo" });
        }

        [HttpGet]

        public ActionResult cancellaTipoIndirizzo(int id)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            TipologiaIndirizzo tipologiaIndirizzo = db.TipoIndirizzo.Find(id);
            if (tipologiaIndirizzo == null)
            {
                errorMessage = "impossibile eliminare la tipologia Indirizzo " + id + " in quanto non censita";
            }
            else
            {
                db.TipoIndirizzo.Remove(tipologiaIndirizzo);
                db.SaveChanges();
                message = "tipologia Indirizzo " + tipologiaIndirizzo.descrizione + " eliminata con successo";
            }
            return RedirectToAction("tipologiaIndirizzo", new { errorMessage = errorMessage, message = message });
        }
        #endregion tipologiaIndirizzo
        #region tipoContrattoImpiego
        [HttpGet]

        public ActionResult tipoContrattoImpiego(String errorMessage, String message)
        {

            ViewBag.errorMessage = errorMessage == null ? String.Empty : errorMessage;
            ViewBag.message = message == null ? String.Empty : message;
            return View(
                (from tipoContrattoImpiego in db.TipoContrattoImpiego select tipoContrattoImpiego).ToList()
                );
        }

        [HttpPost]

        public ActionResult tipoContrattoImpiego(TipoContrattoImpiego ta)
        {
            if ((from fp in db.TipoContrattoImpiego
                 where (fp.descrizione == ta.descrizione)
                 select fp).FirstOrDefault() != null)
            {
                return RedirectToAction("tipoContrattoImpiego", new
                {
                    errorMessage = "Tipo contratto " + ta.descrizione
                        + " già censito"
                });
            }
            else
            {
                db.TipoContrattoImpiego.Add(ta);
                db.SaveChanges();
            }
            return RedirectToAction("tipoContrattoImpiego", new { message = "inserimento Tipo contratto : " + ta.descrizione + " avvenuto con successo" });
        }

        [HttpGet]

        public ActionResult cancellaTipoContrattoImpiego(int id)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            TipoContrattoImpiego tipoContrattoImpiego = db.TipoContrattoImpiego.Find(id);
            if (tipoContrattoImpiego == null)
            {
                errorMessage = "impossibile eliminare il tipo contratto " + id + " in quanto non censita";
            }
            else
            {
                db.TipoContrattoImpiego.Remove(tipoContrattoImpiego);
                db.SaveChanges();
                message = "tipo contratto " + tipoContrattoImpiego.descrizione + " eliminato con successo";
            }
            return RedirectToAction("tipoContrattoImpiego", new { errorMessage = errorMessage, message = message });
        }
        #endregion tipoContrattoImpiego
        #region tipoEnteRilascio
        [HttpGet]

        public ActionResult tipoEnteRilascio(String errorMessage, String message)
        {

            ViewBag.errorMessage = errorMessage == null ? String.Empty : errorMessage;
            ViewBag.message = message == null ? String.Empty : message;
            return View(
                (from tipoEnteRilascio in db.TipoEnteRilascio select tipoEnteRilascio).ToList()
                );
        }

        [HttpPost]

        public ActionResult tipoEnteRilascio(TipoEnteRilascio ta)
        {
            if ((from fp in db.TipoEnteRilascio
                 where (fp.descrizione == ta.descrizione)
                 select fp).FirstOrDefault() != null)
            {
                return RedirectToAction("tipoEnteRilascio", new
                {
                    errorMessage = "Tipo ente " + ta.descrizione
                        + " già censito"
                });
            }
            else
            {
                db.TipoEnteRilascio.Add(ta);
                db.SaveChanges();
            }
            return RedirectToAction("tipoEnteRilascio", new { message = "inserimento tipo Ente rilascio : " + ta.descrizione + " avvenuto con successo" });
        }

        [HttpGet]

        public ActionResult cancellaTipoEnteRilascio(int id)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            TipoEnteRilascio tipoEnteRilascio = db.TipoEnteRilascio.Find(id);
            if (tipoEnteRilascio == null)
            {
                errorMessage = "impossibile eliminare il tipo ente rilascio " + id + " in quanto non censita";
            }
            else
            {
                db.TipoEnteRilascio.Remove(tipoEnteRilascio);
                db.SaveChanges();
                message = "tipo ente rilascio " + tipoEnteRilascio.descrizione + " eliminato con successo";
            }
            return RedirectToAction("tipoEnteRilascio", new { errorMessage = errorMessage, message = message });
        }
        #endregion tipoEnteRilascio
        #region tipoDocumentoIdentita
        [HttpGet]

        public ActionResult tipoDocumentoIdentita(String errorMessage, String message)
        {

            ViewBag.errorMessage = errorMessage == null ? String.Empty : errorMessage;
            ViewBag.message = message == null ? String.Empty : message;
            return View(
                (from tipoDocumentoIdentita in db.TipoDocumentiIdentita select tipoDocumentoIdentita).ToList()
                );
        }

        [HttpPost]

        public ActionResult tipoDocumentoIdentita(TipoDocumentoIdentita ta)
        {
            if ((from fp in db.TipoDocumentiIdentita
                 where (fp.descrizione == ta.descrizione)
                 select fp).FirstOrDefault() != null)
            {
                return RedirectToAction("tipoDocumentoIdentita", new
                {
                    errorMessage = "Tipo Documento identita " + ta.descrizione
                        + " già censito"
                });
            }
            else
            {
                db.TipoDocumentiIdentita.Add(ta);
                db.SaveChanges();
            }
            return RedirectToAction("tipoDocumentoIdentita", new { message = "inserimento tipo Docmento identita : " + ta.descrizione + " avvenuto con successo" });
        }

        [HttpGet]

        public ActionResult cancellaTipoDocumentoIdentita(int id)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            TipoDocumentoIdentita tipoDocumentoIdentita = db.TipoDocumentiIdentita.Find(id);
            if (tipoDocumentoIdentita == null)
            {
                errorMessage = "impossibile eliminare il tipo documento dientita " + id + " in quanto non censito";
            }
            else
            {
                db.TipoDocumentiIdentita.Remove(tipoDocumentoIdentita);
                db.SaveChanges();
                message = "tipo documento identita " + tipoDocumentoIdentita.descrizione + " eliminato con successo";
            }
            return RedirectToAction("tipoDocumentoIdentita", new { errorMessage = errorMessage, message = message });
        }
        #endregion tipoDocumentoIdentita


        #region gruppoLavorazione

        //[HttpGet]
        //public ActionResult GruppoLavorazione()
        //{
        //    mediatori.Models.GruppiLavorazioneModel model = new GruppiLavorazioneModel();
        //    model.gruppiLavorazione = db.GruppiLavorazione.OrderBy(g => g.nome).ToList();
        //    return View(model);
        //}


        //[HttpPost]
        //public ActionResult GruppoLavorazioneAdd(GruppoLavorazione gruppo)
        //{

        //    if ((from fp in db.GruppiLavorazione where (fp.nome == gruppo.nome) select fp).FirstOrDefault() != null)
        //    {
        //        TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Gruppo già censito. Cambiare il nome.");
        //        return RedirectToAction("GruppoLavorazione");
        //    }

        //    db.GruppiLavorazione.Add(gruppo);
        //    db.SaveChanges();

        //    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, "Gruppo " + gruppo.nome  + " inserito con successo");
        //    return RedirectToAction("GruppoLavorazione");
        //}



        //public ActionResult GruppoLavorazioneUsers(int gruppoId)
        //{
        //    List<MyManagerCSharp.Models.MyItem> risultato = null;

        //    MyUsers.UserManager manager = new MyUsers.UserManager("utenti");
        //    manager.openConnection();

        //    try
        //    {
        //      //  risultato = manager.getUsers();
        //    }
        //    finally
        //    {
        //        manager.closeConnection();
        //    }



        //    return Json(risultato, JsonRequestBehavior.AllowGet);
        //}




        //[HttpPost]
        //public ActionResult GruppoLavorazioneEdit(GruppoLavorazione gruppo, List<String> utentiAssociati)
        //{

        //        String errorMessage = String.Empty;
        //        //gruppo.utenti = GruppoLavorazioneUtils.toTockenizedView(utentiAssociati);

        //        gruppo.utenti = String.Join(";", utentiAssociati );

        //        if (gruppo.nome == String.Empty)
        //        {
        //            ModelState.AddModelError("nome", "il campo nome è obbligatorio");
        //            errorMessage = "nome non specificato";
        //        }

        //        if ((from fp in db.GruppiLavorazione
        //             where (fp.nome == gruppo.nome)
        //             select fp).FirstOrDefault() != null)
        //        {
        //            ModelState.AddModelError("nome", "gruppo già censito");
        //            errorMessage = "gruppo già censito";
        //        }

        //        if (!ModelState.IsValid)
        //        {
        //            return RedirectToAction("gruppoLavorazione", new
        //            {
        //                errorMessage = ModelState
        //            });
        //        }



        //        //UPDATE
        //        db.GruppiLavorazione.Attach(gruppo);
        //        var entry = db.Entry(gruppo);
        //        entry.Property(e => e.nome).IsModified = true;
        //        entry.Property(e => e.utenti).IsModified = true;
        //        db.SaveChanges();


        //        TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, "Gruppo " + gruppo.nome + " modificato con successo");
        //    return RedirectToAction("gruppoLavorazione");
        //}

        //[HttpGet]

        //public ActionResult CancellaGruppoLavorazione(int id)
        //{
        //    String errorMessage = string.Empty;
        //    string message = String.Empty;
        //        GruppoLavorazione gruppoLavorazione = db.GruppiLavorazione.Find(id);
        //        if (gruppoLavorazione == null)
        //        {
        //            errorMessage = "impossibile eliminare il gruppo di lavorazione " + id + " in quanto non censito";
        //        }
        //        else
        //        {
        //            db.GruppiLavorazione.Remove(gruppoLavorazione);
        //            db.SaveChanges();
        //            message = "Gruppo di lavorazione " + gruppoLavorazione.nome + " eliminato con successo";
        //        }
        //    return RedirectToAction("gruppoLavorazione", new { errorMessage = errorMessage, message = message });
        //}

        //[HttpPost]

        //public ActionResult AggiornaGruppiAssegnazione(int id, List<String> utentiAssociati)
        //{
        //    if (id == 0) return RedirectToAction("gruppoLavorazione", new { errorMessage = "necessario fornire il codice gruppo" });
        //        GruppoLavorazione gl = db.GruppiLavorazione.Find(id);
        //        gl.utenti = GruppoLavorazioneUtils.toTockenizedView(utentiAssociati);
        //        db.SaveChanges();
        //    return RedirectToAction("gruppoLavorazione", new { message = "Aggiornamento avvenuto con successo" });

        //}

        #endregion gruppoLavorazione


        #region stato

        [HttpGet]
        public ActionResult Stato()
        {
            mediatori.Models.StatoModel model = new StatoModel();
            model.listaStati = db.StatiSegnalazione.OrderBy(p => p.descrizione).ToList();
            // model.listaStatiBase = new SelectList(from EnumStatoBase e in EnumStatoBase.GetValues(typeof(EnumStatoBase)) select new { Id = e, Name = e.ToString() }, "Id", "Name", null);
            // model.listaEntitaAssociate = new SelectList(from EnumEntitaAssociataStato e in EnumEntitaAssociataStato.GetValues(typeof(EnumEntitaAssociataStato)) select new { Id = e, Name = e.ToString() }, "Id", "Name", null);
            //model.listaGruppiDiLavorazione = new SelectList(from GruppoLavorazione gl in db.GruppiLavorazione select new { Id = gl.id, Name = gl.nome }, "Id", "Name", null);
            MyUsers.GroupManager managerGroup = new MyUsers.GroupManager(db.Database.Connection);

            managerGroup.openConnection();
            try
            {
                model.listaGruppi = managerGroup.getList();
                foreach (mediatori.Models.etc.Stato stato in model.listaStati)
                {
                    if (stato.gruppoId != null)
                    {
                        stato.gruppo = managerGroup.getGroup((long)stato.gruppoId);
                    }
                }
            }
            finally
            {
                managerGroup.closeConnection();
            }
            return View(model);
        }




        [HttpPost]
        public ActionResult Stato(Stato stato)
        {
            //Usata sia in fase di INSERT che di UPDATE
            //if (stato.gruppoId == null)
            //{
            //    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Indicare il gruppo di lavorazione");
            //    return RedirectToAction("Stato");
            //}

            //            stato.gruppo = (db.GruppiLavorazione.Find(stato.gruppoLavorazione.id));

            //          ModelState.Remove("gruppoLavorazione.nome");
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare lo stato, verificare i dati: " + Environment.NewLine + message);
                return RedirectToAction("Stato");
            }


            if (stato.id == 0)
            {
                db.StatiSegnalazione.Add(stato);
                db.SaveChanges();

                // LogEventi le = LogEventiManager.getEventoForCreate(User.Identity.Name, stato.id, EnumEntitaRiferimento.STATO);
                //LogEventiManager.save(le, db);
            }
            else
            {
                //La combo sul tipo entità è disabilita e di conseguenza non viene inviato il valore!
                if (stato.descrizione == MyConstants.STATO_INIZIALE_PRATICA)
                {
                    stato.entitaAssociata = EnumEntitaAssociataStato.PRATICA;
                }

                if (stato.descrizione == MyConstants.STATO_INIZIALE_SEGNALAZIONE)
                {
                    stato.entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE;
                }

                //UPDATE
                db.StatiSegnalazione.Attach(stato);
                db.Entry(stato).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                //LogEventi le = LogEventiManager.getEventoForUpdate(User.Identity.Name, stato.id, EnumEntitaRiferimento.STATO, null, stato);
                //LogEventiManager.save(le, db);
            }

            TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, "Salvataggio completato con successo");
            return RedirectToAction("Stato");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult cancellaStato(int id)
        {
            Stato statoDaCancellare = db.StatiSegnalazione.Find(id);

            if (statoDaCancellare == null)
            {
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile cancellare lo stato richiesto, verificare i dati");
                return RedirectToAction("Stato");
            }

            if (statoDaCancellare.descrizione == MyConstants.STATO_INIZIALE_PRATICA || statoDaCancellare.descrizione == MyConstants.STATO_INIZIALE_SEGNALAZIONE)
            {
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile cancellare lo stato richiesto. Si tratta di uno stato indispensabile per il corretto funzionamento del sistema");
                return RedirectToAction("Stato");
            }

            db.StatiSegnalazione.Remove(statoDaCancellare);
            db.SaveChanges();

            LogEventiManager.save(LogEventiManager.getEventoForDelete(User.Identity.Name, id, EnumEntitaRiferimento.STATO), db);

            TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, String.Format("Cancellazione stato '{0}' completato con successo", statoDaCancellare.descrizione));
            return RedirectToAction("Stato");
        }

        #endregion stato
        #region tipoCampagnaPubblicitaria
        [HttpGet]

        public ActionResult tipoCampagnaPubblicitaria(String errorMessage, String message)
        {

            ViewBag.errorMessage = errorMessage == null ? String.Empty : errorMessage;
            ViewBag.message = message == null ? String.Empty : message;
            return View(
                (from tipoCampagnaPubblicitaria in db.TipoCampagnaPubblicitaria select tipoCampagnaPubblicitaria).ToList()
                );
        }

        [HttpPost]

        public ActionResult tipoCampagnaPubblicitaria(TipoCampagnaPubblicitaria ta)
        {
            if ((from fp in db.TipoCampagnaPubblicitaria
                 where (fp.descrizione == ta.descrizione)
                 select fp).FirstOrDefault() != null)
            {
                return RedirectToAction("tipoCampagnaPubblicitaria", new
                {
                    errorMessage = "Tipo Documento identita " + ta.descrizione + " già censito"
                });
            }
            else
            {
                db.TipoCampagnaPubblicitaria.Add(ta);
                db.SaveChanges();
            }
            return RedirectToAction("TipoCampagnaPubblicitaria", new { message = "inserimento campagna  : " + ta.descrizione + " avvenuto con successo" });
        }

        [HttpGet]

        public ActionResult cancellaCampagnaPubblicitaria(int id)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            TipoCampagnaPubblicitaria tipoCampagnaPubblicitaria = db.TipoCampagnaPubblicitaria.Find(id);
            if (tipoCampagnaPubblicitaria == null)
            {
                errorMessage = "impossibile eliminare la campagna pubblicitaria " + id + " in quanto non censito";
            }
            else
            {
                db.TipoCampagnaPubblicitaria.Remove(tipoCampagnaPubblicitaria);
                db.SaveChanges();
                message = "campagna pubblicitaria " + tipoCampagnaPubblicitaria.descrizione + " eliminata con successo";
            }
            return RedirectToAction("TipoCampagnaPubblicitaria", new { errorMessage = errorMessage, message = message });
        }
        #endregion tipoCampagnaPubblicitaria


        #region CanaleAcquisizione
        [HttpGet]
        public ActionResult canaleAcquisizione()
        {
            Models.Configurazione.ConfigurazioneModel model = new Models.Configurazione.ConfigurazioneModel("Canale acquisizione");

            model.listaEntita = db.TipoCanaleAcquisizione.OrderBy(p => p.descrizione).Select(p => new Models.Configurazione.EntitaModel { id = p.id, descrizione = p.descrizione }).ToList();
            return View("Shared", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult canaleAcquisizione(int id, string descrizione)
        {

            descrizione = descrizione.Trim();

            TipoCanaleAcquisizione checkExists;

            if (id == 0)
            {
                //NUOVO
                checkExists = (from fp in db.TipoCanaleAcquisizione where fp.descrizione == descrizione select fp).FirstOrDefault();

                if (checkExists != null)
                {
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, String.Format("Canale acquisizione '{0}' già censito", descrizione));
                }
                else
                {
                    db.TipoCanaleAcquisizione.Add(new TipoCanaleAcquisizione { descrizione = descrizione });
                    db.SaveChanges();
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, String.Format("Canale acquisizione '{0}' inserto con successo", descrizione));
                }
            }
            else
            {
                //MODIFICA
                checkExists = db.TipoCanaleAcquisizione.Find(id);
                if (checkExists == null)
                {
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, String.Format("Impossibile modifcare il canale acquisizione '{0}' in quanto non censito", id));
                }
                else
                {
                    checkExists.descrizione = descrizione;

                    db.SaveChanges();
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, String.Format("Canale acquisizione '{0}' modificato con successo", checkExists.descrizione));
                }
            }

            return RedirectToAction("canaleAcquisizione");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult canaleAcquisizioneDelete(int id)
        {
            TipoCanaleAcquisizione checkExists = db.TipoCanaleAcquisizione.Find(id);
            if (checkExists == null)
            {
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, String.Format("Impossibile eliminare il canale acquisizione '{0}' in quanto non censito", id));
            }
            else
            {
                db.TipoCanaleAcquisizione.Remove(checkExists);
                db.SaveChanges();
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, String.Format("Canale acquisizione '{0}' eliminato con successo", checkExists.descrizione));
            }
            return RedirectToAction("canaleAcquisizione");

        }
        #endregion tipoCanaleAcquisizione


        #region tipoLuogoritrovo
        [HttpGet]

        public ActionResult tipoLuogoRitrovo(String errorMessage, String message)
        {

            ViewBag.errorMessage = errorMessage == null ? String.Empty : errorMessage;
            ViewBag.message = message == null ? String.Empty : message;
            return View(
                (from tipoLuogoRitrovo in db.TipoLuogoRitrovo select tipoLuogoRitrovo).ToList()
                );
        }

        [HttpPost]

        public ActionResult tipoLuogoRitrovo(TipoLuogoRitrovo ta)
        {
            if ((from fp in db.TipoLuogoRitrovo
                 where (fp.descrizione == ta.descrizione)
                 select fp).FirstOrDefault() != null)
            {
                return RedirectToAction("tipoLuogoRitrovo", new
                {
                    errorMessage = "Luogo ritrovo " + ta.descrizione + " già censito"
                });
            }
            else
            {
                db.TipoLuogoRitrovo.Add(ta);
                db.SaveChanges();
            }
            return RedirectToAction("tipoLuogoRitrovo", new { message = "inserimento luogo ritrovo  : " + ta.descrizione + " avvenuto con successo" });
        }

        [HttpGet]

        public ActionResult cancellaTipoLuogoRitrovo(int id)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            TipoLuogoRitrovo tipoLuogoRitrovo = db.TipoLuogoRitrovo.Find(id);
            if (tipoLuogoRitrovo == null)
            {
                errorMessage = "impossibile eliminare Luogo ritrovo " + id + " in quanto non censito";
            }
            else
            {
                db.TipoLuogoRitrovo.Remove(tipoLuogoRitrovo);
                db.SaveChanges();
                message = "Luogo ritrovo " + tipoLuogoRitrovo.descrizione + " eliminato con successo";
            }
            return RedirectToAction("TipoLuogoRitrovo", new { errorMessage = errorMessage, message = message });
        }
        #endregion tipoLuogoritrovo
        #region tipoContatto
        [HttpGet]

        public ActionResult tipoContatto(String errorMessage, String message)
        {

            ViewBag.errorMessage = errorMessage == null ? String.Empty : errorMessage;
            ViewBag.message = message == null ? String.Empty : message;
            return View(
                (from tipoContatto in db.TipoContatto select tipoContatto).ToList()
                );
        }

        [HttpPost]

        public ActionResult tipoContatto(TipoContatto ta)
        {
            if ((from fp in db.TipoContatto
                 where (fp.descrizione == ta.descrizione)
                 select fp).FirstOrDefault() != null)
            {
                return RedirectToAction("tipoContatto", new
                {
                    errorMessage = "tipo contatto " + ta.descrizione + " già censito"
                });
            }
            else
            {
                db.TipoContatto.Add(ta);
                db.SaveChanges();
            }
            return RedirectToAction("tipoContatto", new { message = "inserimento tipo contatto  : " + ta.descrizione + " avvenuto con successo" });
        }

        [HttpGet]

        public ActionResult cancellaTipoContatto(int id)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            TipoContatto tipoContatto = db.TipoContatto.Find(id);
            if (tipoContatto == null)
            {
                errorMessage = "impossibile eliminare il tipo contatto " + id + " in quanto non censito";
            }
            else
            {
                db.TipoContatto.Remove(tipoContatto);
                db.SaveChanges();
                message = "tipo Contatto " + tipoContatto.descrizione + " eliminato con successo";
            }
            return RedirectToAction("TipoContatto", new { errorMessage = errorMessage, message = message });
        }
        #endregion tipoContatto
        #region tipoRiferimento
        [HttpGet]

        public ActionResult tipoRiferimento(String errorMessage, String message)
        {

            ViewBag.errorMessage = errorMessage == null ? String.Empty : errorMessage;
            ViewBag.message = message == null ? String.Empty : message;
            return View(
                (from tipoRiferimento in db.TipoRiferimento select tipoRiferimento).ToList()
                );
        }

        [HttpPost]

        public ActionResult tipoRiferimento(TipoRiferimento ta)
        {
            if ((from fp in db.TipoRiferimento
                 where (fp.descrizione == ta.descrizione)
                 select fp).FirstOrDefault() != null)
            {
                return RedirectToAction("tipoRiferimento", new
                {
                    errorMessage = "tipo riferimento " + ta.descrizione + " già censito"
                });
            }
            else
            {
                db.TipoRiferimento.Add(ta);
                db.SaveChanges();
            }
            return RedirectToAction("tipoRiferimento", new { message = "inserimento tipo riferimento  : " + ta.descrizione + " avvenuto con successo" });
        }

        [HttpGet]

        public ActionResult cancellaTipoRiferimento(int id)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            TipoRiferimento tipoRiferimento = db.TipoRiferimento.Find(id);
            if (tipoRiferimento == null)
            {
                errorMessage = "impossibile eliminare il tipo contatto " + id + " in quanto non censito";
            }
            else
            {
                db.TipoRiferimento.Remove(tipoRiferimento);
                db.SaveChanges();
                message = "tipo Contatto " + tipoRiferimento.descrizione + " eliminato con successo";
            }
            return RedirectToAction("TipoRiferimento", new { errorMessage = errorMessage, message = message });
        }
        #endregion tipoRiferimento
        #region tipoCategoriaImpiego
        [HttpGet]

        public ActionResult tipoCategoriaImpiego(String errorMessage, String message)
        {

            ViewBag.errorMessage = errorMessage == null ? String.Empty : errorMessage;
            ViewBag.message = message == null ? String.Empty : message;
            return View(
                (from categoriaImpiego in db.TipoCategoriaImpiego select categoriaImpiego).ToList()
                );
        }

        [HttpPost]

        public ActionResult tipoCategoriaImpiego(TipoCategoriaImpiego ta)
        {
            if ((from fp in db.TipoCategoriaImpiego
                 where (fp.descrizione == ta.descrizione)
                 select fp).FirstOrDefault() != null)
            {
                return RedirectToAction("tipoCategoriaImpiego", new
                {
                    errorMessage = "tipo categoriaImpiego " + ta.descrizione + " già censito"
                });
            }
            else
            {
                db.TipoCategoriaImpiego.Add(ta);
                db.SaveChanges();
            }
            return RedirectToAction("tipoCategoriaImpiego", new { message = "inserimento Categoria impiego  : " + ta.descrizione + " avvenuto con successo" });
        }

        [HttpGet]

        public ActionResult cancellaTipoCategoriaImpiego(int id)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            TipoCategoriaImpiego tipoCategoriaImpiego = db.TipoCategoriaImpiego.Find(id);
            if (tipoCategoriaImpiego == null)
            {
                errorMessage = "impossibile eliminare il tipo categoria impiego" + id + " in quanto non censito";
            }
            else
            {
                db.TipoCategoriaImpiego.Remove(tipoCategoriaImpiego);
                db.SaveChanges();
                message = "categoria impiego " + tipoCategoriaImpiego.descrizione + " eliminato con successo";
            }
            return RedirectToAction("TipoCategoriaImpiego", new { errorMessage = errorMessage, message = message });
        }
        #endregion tipoCategoriaImpiego
        #region tipoProdotto
        [HttpGet]

        public ActionResult tipoProdotto(String errorMessage, String message)
        {

            ViewBag.errorMessage = errorMessage == null ? String.Empty : errorMessage;
            ViewBag.message = message == null ? String.Empty : message;
            return View(
                (from tipoProdotto in db.TipoProdotto select tipoProdotto).ToList()
                );
        }

        [HttpPost]

        public ActionResult tipoProdotto(TipoProdotto ta)
        {
            if ((from fp in db.TipoProdotto
                 where (fp.descrizione == ta.descrizione)
                 select fp).FirstOrDefault() != null)
            {
                return RedirectToAction("tipoProdotto", new
                {
                    errorMessage = "prodotto " + ta.descrizione + " già censito"
                });
            }
            else
            {
                db.TipoProdotto.Add(ta);
                db.SaveChanges();
            }
            return RedirectToAction("tipoProdotto", new { message = "inserimento prodotto  : " + ta.descrizione + " avvenuto con successo" });
        }

        [HttpGet]

        public ActionResult cancellaTipoProdotto(int id)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            TipoProdotto tipoProdotto = db.TipoProdotto.Find(id);
            if (tipoProdotto == null)
            {
                errorMessage = "impossibile eliminare il prodotto " + id + " in quanto non censito";
            }
            else
            {
                db.TipoProdotto.Remove(tipoProdotto);
                db.SaveChanges();
                message = "Prodotto " + tipoProdotto.descrizione + " eliminato con successo";
            }
            return RedirectToAction("TipoProdotto", new { errorMessage = errorMessage, message = message });
        }
        #endregion tipotipoProdotto
        #region tipoCategoriaAmministrazione
        [HttpGet]

        public ActionResult tipoCategoriaAmministrazione(String errorMessage, String message)
        {

            ViewBag.errorMessage = errorMessage == null ? String.Empty : errorMessage;
            ViewBag.message = message == null ? String.Empty : message;
            return View(
                (from tipoCategoriaAmministrazione in db.TipoCategoriaAmministrazione select tipoCategoriaAmministrazione).ToList()
                );
        }

        [HttpPost]

        public ActionResult tipoCategoriaAmministrazione(TipoCategoriaAmministrazione ta)
        {
            if ((from fp in db.TipoCategoriaAmministrazione
                 where (fp.descrizione == ta.descrizione)
                 select fp).FirstOrDefault() != null)
            {
                return RedirectToAction("tipoCategoriaAmministrazione", new
                {
                    errorMessage = "Categoria " + ta.descrizione + " già censita"
                });
            }
            else
            {
                db.TipoCategoriaAmministrazione.Add(ta);
                db.SaveChanges();
            }
            return RedirectToAction("tipoCategoriaAmministrazione", new { message = "inserimento categoria  : " + ta.descrizione + " avvenuto con successo" });
        }

        [HttpGet]

        public ActionResult cancellaTipoCategoriaAmministrazione(int id)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            TipoCategoriaAmministrazione tipoCategoriaAmministrazione = db.TipoCategoriaAmministrazione.Find(id);
            if (tipoCategoriaAmministrazione == null)
            {
                errorMessage = "impossibile eliminare la categoria " + id + " in quanto non censita";
            }
            else
            {
                db.TipoCategoriaAmministrazione.Remove(tipoCategoriaAmministrazione);
                db.SaveChanges();
                message = "Categoria " + tipoCategoriaAmministrazione.descrizione + " eliminata con successo";
            }
            return RedirectToAction("TipoCategoriaAmministrazione", new { errorMessage = errorMessage, message = message });
        }
        #endregion tipoCategoriaAmministrazione


        #region tipoAssumibilitaAmministrazione
        [HttpGet]
        public ActionResult assumibilitaAmministrazione()
        {
            Models.Configurazione.ConfigurazioneModel model = new Models.Configurazione.ConfigurazioneModel("Assumibilita Amministrazione");

            model.listaEntita = db.TipoAssumibilitaAmministrazione.OrderBy(p => p.descrizione).Select(p => new Models.Configurazione.EntitaModel { id = p.id, descrizione = p.descrizione }).ToList();
            return View("Shared", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult assumibilitaAmministrazione(int id, string descrizione)
        {

            descrizione = descrizione.Trim();
            TipoAssumibilitaAmministrazione checkExists;

            if (id == 0)
            {
                //NUOVO
                checkExists = (from fp in db.TipoAssumibilitaAmministrazione where fp.descrizione == descrizione select fp).FirstOrDefault();

                if (checkExists != null)
                {
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, String.Format("Assumibilità amministrazione '{0}' già censita", descrizione));
                }
                else
                {
                    db.TipoAssumibilitaAmministrazione.Add(new TipoAssumibilitaAmministrazione { descrizione = descrizione });
                    db.SaveChanges();
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, String.Format("Assumibilità amministrazione '{0}' inserta con successo", descrizione));
                }
            }
            else
            {
                //MODIFICA
                checkExists = db.TipoAssumibilitaAmministrazione.Find(id);
                if (checkExists == null)
                {
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, String.Format("Impossibile modifcare l'assumibilità amministrazione '{0}' in quanto non censita", id));
                }
                else
                {
                    checkExists.descrizione = descrizione;

                    db.SaveChanges();
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, String.Format("Assumibilità amministrazione '{0}' modificata con successo", checkExists.descrizione));
                }
            }

            return RedirectToAction("assumibilitaAmministrazione");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult assumibilitaAmministrazioneDelete(int id)
        {

            TipoAssumibilitaAmministrazione checkExists = db.TipoAssumibilitaAmministrazione.Find(id);
            if (checkExists == null)
            {
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, String.Format("Impossibile eliminare l'assumibilità amministrazione '{0}' in quanto non censita", id));
            }
            else
            {
                db.TipoAssumibilitaAmministrazione.Remove(checkExists);
                db.SaveChanges();
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, String.Format("Assumibilità amministrazione '{0}' eliminata con successo", checkExists.descrizione));
            }
            return RedirectToAction("assumibilitaAmministrazione");
        }
        #endregion tipoAssumibilitaAmministrazione


        #region tipoErogazione
        [HttpGet]

        public ActionResult tipoErogazione(String errorMessage, String message)
        {

            ViewBag.errorMessage = errorMessage == null ? String.Empty : errorMessage;
            ViewBag.message = message == null ? String.Empty : message;
            return View(db.TipoErogazione.ToList());
        }

        [HttpPost]

        public ActionResult tipoErogazione(TipoErogazione tipoErog)
        {
            if ((from fp in db.TipoErogazione
                 where ((fp.descrizione == tipoErog.descrizione) || (fp.sigla == tipoErog.sigla))
                 select fp).FirstOrDefault() != null)
            {
                return RedirectToAction("tipoErogazione", new
                {
                    errorMessage = "tipo erogazione " + tipoErog.descrizione + " già censita"
                });
            }
            else
            {
                db.TipoErogazione.Add(tipoErog);
                db.SaveChanges();
            }
            return RedirectToAction("tipoErogazione", new { message = "inserimento tipo erogazione: " + tipoErog.descrizione + " avvenuto con successo" });
        }

        [HttpGet]

        public ActionResult cancellaTipoErogazione(string sigla)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            TipoErogazione tipoErogazione = db.TipoErogazione.Find(sigla);
            if (tipoErogazione == null)
            {
                errorMessage = "impossibile eliminare il erogazione " + sigla + " in quanto non censito";
            }
            else
            {
                db.TipoErogazione.Remove(tipoErogazione);
                db.SaveChanges();
                message = "tipo erogazione " + tipoErogazione.descrizione + " eliminata con successo";
            }
            return RedirectToAction("TipoErogazione", new { errorMessage = errorMessage, message = message });
        }
        #endregion tipoErogazione


        #region tipoAgenzia
        [HttpGet]
        public ActionResult tipoAgenzia()
        {

            Models.Configurazione.ConfigurazioneModel model = new Models.Configurazione.ConfigurazioneModel("Tipo agenzia");

            model.listaEntita = db.TipoAgenzia.OrderBy(p => p.descrizione).Select(p => new Models.Configurazione.EntitaModel { id = p.id, descrizione = p.descrizione }).ToList();
            return View("Shared", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult tipoAgenzia(int id, string descrizione)
        {
            descrizione = descrizione.Trim();

            TipoAgenzia checkExists;

            if (id == 0)
            {
                //NUOVO
                checkExists = (from fp in db.TipoAgenzia where fp.descrizione == descrizione select fp).FirstOrDefault();

                if (checkExists != null)
                {
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, String.Format("Tipo agenzia '{0}' già censita", descrizione));
                }
                else
                {
                    db.TipoAgenzia.Add(new TipoAgenzia { descrizione = descrizione });
                    db.SaveChanges();
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, String.Format("Tipo agenzia '{0}' inserto con successo", descrizione));
                }
            }
            else
            {
                //MODIFICA
                checkExists = db.TipoAgenzia.Find(id);
                if (checkExists == null)
                {
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, String.Format("Impossibile modifcare il tipo agenzia '{0}' in quanto non censito", id));
                }
                else
                {
                    checkExists.descrizione = descrizione;

                    db.SaveChanges();
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, String.Format("Tipo agenzia '{0}' modificato con successo", checkExists.descrizione));
                }
            }

            return RedirectToAction("tipoAgenzia");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult tipoAgenziaDelete(int id)
        {
            TipoAgenzia checkExists = db.TipoAgenzia.Find(id);
            if (checkExists == null)
            {
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, String.Format("Impossibile eliminare il tipo agenzia '{0}' in quanto non censito", id));
            }
            else
            {
                db.TipoAgenzia.Remove(checkExists);
                db.SaveChanges();
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, String.Format("Tipo agenzia '{0}' eliminato con successo", checkExists.descrizione));
            }
            return RedirectToAction("tipoAgenzia");


        }
        #endregion tipoErogazione
        #region Parametro
        [HttpGet]

        public ActionResult Parametro(String errorMessage, String message)
        {

            ViewBag.errorMessage = errorMessage == null ? String.Empty : errorMessage;
            ViewBag.message = message == null ? String.Empty : message;
            return View(db.Parametri.ToList());
        }
        [HttpPost]

        public ActionResult Parametro(Parametro p)
        {
            Parametro parDadb = db.Parametri.Find(p.id);
            parDadb.value = p.value;
            db.SaveChanges();
            ViewBag.message = "parametro salvato con successo";
            return View(db.Parametri.ToList());
        }
        #endregion
        #region TipoConsensoPrivacy

        [HttpGet]
        public ActionResult TipoConsensoPrivacy()
        {
            TipoConsensoPrivacyModel model = new TipoConsensoPrivacyModel();
            model.listaTipiConsenso = db.TipoConsensoPrivacy.OrderBy(t => t.descrizione).ToList<TipoConsensoPrivacy>();
            return View(model);
        }




        #endregion
    }
}

