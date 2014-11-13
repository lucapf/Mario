using mediatori.Controllers.Business.Anagrafiche;
using mediatori.Controllers.CQS;
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
    public class ConfigurazioniController : MyBaseController
    {
        public ActionResult Index()
        {

            List<MenuElement> model = new List<MenuElement>(){
                    new MenuElement(){display="Province", ordinamento=1,livello=1,role="Amministratore",url="Province"},
                    new MenuElement(){display="Toponimi", ordinamento=1,livello=1,role="Amministratore",url="Toponimi"},
                    new MenuElement(){display="Fonti Pubblicitarie", ordinamento=1,livello=1,role="Amministratore",url="fontePubblicitaria"},
                    new MenuElement(){display="Tipo Prestito", ordinamento=1,livello=1,role="Amministratore",url="tipologiaPrestito"}, 
                    new MenuElement(){display="Tipo Azienda", ordinamento=1,livello=1,role="Amministratore",url="tipologiaAzienda"},
                    new MenuElement(){display="Tipo Indirizzo", ordinamento=1,livello=1,role="Amministratore",url="tipologiaIndirizzo"},
                    new MenuElement(){display="Tipo Impiego", ordinamento=1,livello=1,role="Amministratore",url="tipoContrattoImpiego"},
                    new MenuElement(){display="Tipo Ente Rilascio", ordinamento=1,livello=1,role="Amministratore",url="tipoEnteRilascio"}, 
                    new MenuElement(){display="Tipo Documento Identita", ordinamento=1,livello=1,role="Amministratore",url="tipoDocumentoIdentita"},
                    new MenuElement(){display="Gruppi di lavorazione", ordinamento=1,livello=1,role="Amministratore",url="tipoCampagnaPubblicitaria"},
                    new MenuElement(){display="Tipo Contatto", ordinamento=1,livello=1,role="Amministratore",url="tipoContatto"},
                    new MenuElement(){display="Canale Acquisizione", ordinamento=1,livello=1,role="Amministratore",url="tipoCanaleAcquisizione"},
                    new MenuElement(){display="Tipo Luogo Ritrovo", ordinamento=1,livello=1,role="Amministratore",url="tipoLuogoRitrovo"},
                    new MenuElement(){display="Tipo Riferimento", ordinamento=1,livello=1,role="Amministratore",url="tipoRiferimento"},
                    new MenuElement(){display="Tipo Prodotto", ordinamento=1,livello=1,role="Amministratore",url="tipoProdotto"},
                    new MenuElement(){display="Tipo Categoria", ordinamento=1,livello=1,role="Amministratore",url="tipoCategoriaAmministrazione"},
                    new MenuElement(){display="Tipo Agenzia", ordinamento=1,livello=1,role="Amministratore",url="tipoAgenzia"},
                    new MenuElement(){display="Tipo Erogazione", ordinamento=1,livello=1,role="Amministratore",url="tipoErogazione"},
                    new MenuElement(){display="Tipo Assumibilita", ordinamento=1,livello=1,role="Amministratore",url="tipoAssumibilitaAmministrazione"},
                    new MenuElement(){display="Stato", ordinamento=1,livello=1,role="Amministratore",url="stato"}
                    
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
        [HttpGet]
        public String popolaDropDownlistProvince()
        {

            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            return new PopolaDropDownListAnagrafiche().popolaDropDownListProvince(db);
        }
        #endregion province
        #region comuni
        [HttpGet]

        public ActionResult comuni(ComuneSearch comuneSearch)
        {
            if (comuneSearch == null) return View();
            IList<Comune> listComune = null;
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
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
        [HttpGet]
        public String popolaDropDownlistComuni(String comboComunElementId, String denominazioneProvincia)
        {

            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            return new PopolaDropDownListAnagrafiche().popolaDropDownListComuniJSON(comboComunElementId, denominazioneProvincia, db);
        }


        #endregion comuni
        #region toponimi
        [HttpGet]

        public ActionResult toponimi(ComuneSearch comuneSearch, String errorMessage, String message)
        {

            ViewBag.errorMessage = errorMessage == null ? String.Empty : errorMessage;
            ViewBag.message = message == null ? String.Empty : message;
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            return View(
                (from toponimi in db.Toponimi select toponimi).ToList()
                );
        }

        [HttpPost]

        public ActionResult toponimo(String sigla)
        {
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
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
            }

            return RedirectToAction("toponimi", new { message = "inserimento toponimo : " + sigla + " avvenuto con successo" });
        }

        [HttpGet]

        public ActionResult cancellaToponimo(String sigla)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
            {
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
            }
            return RedirectToAction("toponimi", new { errorMessage = errorMessage, message = message });
        }
        #endregion toponimi
        #region fontePubblicitaria
        [HttpGet]

        public ActionResult fontePubblicitaria(String errorMessage, String message)
        {

            ViewBag.errorMessage = errorMessage == null ? String.Empty : errorMessage;
            ViewBag.message = message == null ? String.Empty : message;
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            return View(
                (from fontePubblicitaria in db.FontiPubblicitarie select fontePubblicitaria).ToList()
                );
        }

        [HttpPost]

        public ActionResult fontePubblicitaria(String descrizione)
        {
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
            {
                if ((from fp in db.FontiPubblicitarie
                     where fp.descrizione == descrizione
                     select fp).FirstOrDefault() != null)
                {
                    return RedirectToAction("fontePubblicitaria", new
                    {
                        errorMessage = "fonte pubblicitaria " + descrizione
                            + " già censita"
                    });
                }
                else
                {
                    db.FontiPubblicitarie.Add(new FontePubblicitaria { descrizione = descrizione });
                    db.SaveChanges();
                }
            }
            return RedirectToAction("fontePubblicitaria", new { message = "inserimento Fonte pubblicitaria : " + descrizione + " avvenuta con successo" });
        }

        [HttpGet]

        public ActionResult cancellaFontePubblicitaria(int id)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
            {
                FontePubblicitaria fontePubblicitaria = db.FontiPubblicitarie.Find(id);
                if (fontePubblicitaria == null)
                {
                    errorMessage = "impossibile eliminare la fonte pubblicitaria " + id + " in quanto non censita";
                }
                else
                {
                    db.FontiPubblicitarie.Remove(fontePubblicitaria);
                    db.SaveChanges();
                    message = "fonte pubblicitaria " + fontePubblicitaria.descrizione + " eliminata con successo";
                }
            }
            return RedirectToAction("fontePubblicitaria", new { errorMessage = errorMessage, message = message });
        }
        #endregion fontePubblicitaria
        #region tipologiaPrestito
        [HttpGet]

        public ActionResult tipologiaPrestito(String errorMessage, String message)
        {

            ViewBag.errorMessage = errorMessage == null ? String.Empty : errorMessage;
            ViewBag.message = message == null ? String.Empty : message;
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            return View(
                (from tipologiaPrestito in db.TipoPrestito select tipologiaPrestito).ToList()
                );
        }

        [HttpPost]

        public ActionResult tipologiaPrestito(String descrizione)
        {
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
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
            }
            return RedirectToAction("tipologiaPrestito", new { message = "inserimento tipologia prestito : " + descrizione + " avvenuta con successo" });
        }

        [HttpGet]

        public ActionResult cancellaTipologiaPrestito(int id)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
            {
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
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            return View(
                (from tipologiaAzienda in db.TipoAzienda select tipologiaAzienda).ToList()
                );
        }

        [HttpPost]

        public ActionResult tipologiaAzienda(TipologiaAzienda ta)
        {
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
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
            }
            return RedirectToAction("tipologiaAzienda", new { message = "inserimento tipologia Azienda : " + ta.descrizione + " avvenuta con successo" });
        }

        [HttpGet]

        public ActionResult cancellaTipoAzienda(int id)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
            {
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
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            return View(
                (from tipologiaIndirizzo in db.TipoIndirizzo select tipologiaIndirizzo).ToList()
                );
        }

        [HttpPost]

        public ActionResult tipologiaIndirizzo(TipologiaIndirizzo ta)
        {
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
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
            }
            return RedirectToAction("tipologiaIndirizzo", new { message = "inserimento tipologia Indirizzo : " + ta.descrizione + " avvenuta con successo" });
        }

        [HttpGet]

        public ActionResult cancellaTipoIndirizzo(int id)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
            {
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
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            return View(
                (from tipoContrattoImpiego in db.TipoContrattoImpiego select tipoContrattoImpiego).ToList()
                );
        }

        [HttpPost]

        public ActionResult tipoContrattoImpiego(TipoContrattoImpiego ta)
        {
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
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
            }
            return RedirectToAction("tipoContrattoImpiego", new { message = "inserimento Tipo contratto : " + ta.descrizione + " avvenuto con successo" });
        }

        [HttpGet]

        public ActionResult cancellaTipoContrattoImpiego(int id)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
            {
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
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            return View(
                (from tipoEnteRilascio in db.TipoEnteRilascio select tipoEnteRilascio).ToList()
                );
        }

        [HttpPost]

        public ActionResult tipoEnteRilascio(TipoEnteRilascio ta)
        {
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
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
            }
            return RedirectToAction("tipoEnteRilascio", new { message = "inserimento tipo Ente rilascio : " + ta.descrizione + " avvenuto con successo" });
        }

        [HttpGet]

        public ActionResult cancellaTipoEnteRilascio(int id)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
            {
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
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            return View(
                (from tipoDocumentoIdentita in db.TipoDocumentiIdentita select tipoDocumentoIdentita).ToList()
                );
        }

        [HttpPost]

        public ActionResult tipoDocumentoIdentita(TipoDocumentoIdentita ta)
        {
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
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
            }
            return RedirectToAction("tipoDocumentoIdentita", new { message = "inserimento tipo Docmento identita : " + ta.descrizione + " avvenuto con successo" });
        }

        [HttpGet]

        public ActionResult cancellaTipoDocumentoIdentita(int id)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
            {
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
            }
            return RedirectToAction("tipoDocumentoIdentita", new { errorMessage = errorMessage, message = message });
        }
        #endregion tipoDocumentoIdentita
        #region gruppoLavorazione
        [HttpGet]

        public ActionResult GruppoLavorazione(String errorMessage, String message)
        {

            ViewBag.errorMessage = errorMessage == null ? String.Empty : errorMessage;
            ViewBag.message = message == null ? String.Empty : message;
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            List<String> users = db.Database.SqlQuery<String>("select UserName from dbo.UserProfile").ToList();
            List<GruppoLavorazioneView> lglw = GruppoLavorazioneUtils.toView((from gruppoLavorazione in db.gruppiLavorazione select gruppoLavorazione).ToList(), users);
            ViewBag.gruppoLavorazioneEmpty = new GruppoLavorazioneView(users);
            return View(lglw);
        }

        [HttpPost]

        public ActionResult GruppoLavorazione(GruppoLavorazione ta, List<String> utentiAssociati)
        {
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
            {
                String errorMessage = String.Empty;
                ta.utenti = GruppoLavorazioneUtils.toTockenizedView(utentiAssociati);
                if (ta.nome == String.Empty)
                {
                    ModelState.AddModelError("nome", "il campo nome è obbligatorio");
                    errorMessage = "nome non specificato";
                }
                if ((from fp in db.gruppiLavorazione
                     where (fp.nome == ta.nome)
                     select fp).FirstOrDefault() != null)
                {
                    ModelState.AddModelError("nome", "gruppo già censito");
                    errorMessage = "gruppo già censito";
                }
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("gruppoLavorazione", new
                    {
                        errorMessage = ModelState
                    });
                }
                db.gruppiLavorazione.Add(ta);
                db.SaveChanges();

            }
            return RedirectToAction("gruppoLavorazione", new { message = "inserimento Gruppo Lavorazione : " + ta.nome + " avvenuto con successo" });
        }

        [HttpGet]

        public ActionResult CancellaGruppoLavorazione(int id)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
            {
                GruppoLavorazione gruppoLavorazione = db.gruppiLavorazione.Find(id);
                if (gruppoLavorazione == null)
                {
                    errorMessage = "impossibile eliminare il gruppo di lavorazione " + id + " in quanto non censito";
                }
                else
                {
                    db.gruppiLavorazione.Remove(gruppoLavorazione);
                    db.SaveChanges();
                    message = "Gruppo di lavorazione " + gruppoLavorazione.nome + " eliminato con successo";
                }
            }
            return RedirectToAction("gruppoLavorazione", new { errorMessage = errorMessage, message = message });
        }

        [HttpPost]

        public ActionResult AggiornaGruppiAssegnazione(int id, List<String> utentiAssociati)
        {
            if (id == 0) return RedirectToAction("gruppoLavorazione", new { errorMessage = "necessario fornire il codice gruppo" });
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
            {
                GruppoLavorazione gl = db.gruppiLavorazione.Find(id);
                gl.utenti = GruppoLavorazioneUtils.toTockenizedView(utentiAssociati);
                db.SaveChanges();
            }
            return RedirectToAction("gruppoLavorazione", new { message = "Aggiornamento avvenuto con successo" });

        }

        #endregion gruppoLavorazione
        #region stato
        [HttpGet]

        public ActionResult Stato(String errorMessage, String message)
        {

            ViewBag.errorMessage = errorMessage == null ? String.Empty : errorMessage;
            ViewBag.message = message == null ? String.Empty : message;
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            List<StatoView> lstStatoiView = new List<StatoView>();
            foreach (Stato stato in getStati(db))
            {
                lstStatoiView.Add(new StatoView(stato, db));
            }
            ViewBag.statoViewEmpty = new StatoView(db);
            return View(lstStatoiView);
        }

        [HttpGet]

        public ActionResult ModificaStato(int id)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            Stato statoToEdit = getStato(db, id);
            return View("StatoPartialEdit", new StatoView(statoToEdit, db));
        }
        [HttpPost]

        public ActionResult ModificaStato(Stato stato)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            if (stato.gruppoLavorazione == null)
                return RedirectToAction("Stato",
                    new { errorMessage = "indicare il gruppo di lavorazione" });
            stato.gruppoLavorazione = (db.gruppiLavorazione.Find(stato.gruppoLavorazione.id));
            ModelState.Remove("gruppoLavorazione.nome");
            if (!ModelState.IsValid)
                return RedirectToAction("Stato",
                       new { errorMessage = "non tutti i campi obbligatori sono stati inseriti" });
            Stato statoOriginale = getStato(db, stato.id);
            LogEventi le = LogEventiManager.getEventoForUpdate(User.Identity.Name, stato.id, EnumEntitaRiferimento.STATO, statoOriginale, stato);
            statoOriginale = (Stato)CopyObject.simpleCompy(statoOriginale, stato);
            LogEventiManager.save(le, db);
            return RedirectToAction("Stato",
                    new { message = String.Format("Stato {0} inserito con successo", stato.descrizione) });
        }



        private Stato getStato(MainDbContext db, int id)
        {
            return (from s in db.statiSegnalazione.Include("gruppoLavorazione") where s.id == id select s).First();
        }
        private List<Stato> getStati(MainDbContext db)
        {
            return (from s in db.statiSegnalazione.Include("gruppoLavorazione") select s).ToList();
        }

        [HttpPost]

        public ActionResult Stato(Stato s)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            if (s.gruppoLavorazione == null)
                return RedirectToAction("Stato",
                    new { errorMessage = "indicare il gruppo di lavorazione" });
            s.gruppoLavorazione = (db.gruppiLavorazione.Find(s.gruppoLavorazione.id));
            ModelState.Remove("gruppoLavorazione.nome");
            if (!ModelState.IsValid)
                return RedirectToAction("Stato", new { errorMessage = "non tutti i dati obbligatori sono stati inseriti, impossibile procedere" });
            db.statiSegnalazione.Add(s);
            LogEventi le = LogEventiManager.getEventoForCreate(User.Identity.Name, s.id, EnumEntitaRiferimento.STATO);
            LogEventiManager.save(le, db);
            db.SaveChanges();
            return RedirectToAction("Stato", new { message = String.Format("inserimento stato {0} avvenuto con successo", s.descrizione) });

        }
        public ActionResult CancellaStato(int id)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            Stato statoDaCancellare = db.statiSegnalazione.Find(id);
            db.statiSegnalazione.Remove(statoDaCancellare);
            LogEventiManager.save(LogEventiManager.getEventoForDelete(User.Identity.Name, id, EnumEntitaRiferimento.STATO), db);
            return RedirectToAction("Stato", new { message = String.Format("cancellazione stato {0} avvenuto con successo", statoDaCancellare.descrizione) });
        }
        #endregion stato
        #region tipoCampagnaPubblicitaria
        [HttpGet]

        public ActionResult tipoCampagnaPubblicitaria(String errorMessage, String message)
        {

            ViewBag.errorMessage = errorMessage == null ? String.Empty : errorMessage;
            ViewBag.message = message == null ? String.Empty : message;
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            return View(
                (from tipoCampagnaPubblicitaria in db.TipoCampagnaPubblicitaria select tipoCampagnaPubblicitaria).ToList()
                );
        }

        [HttpPost]

        public ActionResult tipoCampagnaPubblicitaria(TipoCampagnaPubblicitaria ta)
        {
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
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
            }
            return RedirectToAction("TipoCampagnaPubblicitaria", new { message = "inserimento campagna  : " + ta.descrizione + " avvenuto con successo" });
        }

        [HttpGet]

        public ActionResult cancellaCampagnaPubblicitaria(int id)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
            {
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
            }
            return RedirectToAction("TipoCampagnaPubblicitaria", new { errorMessage = errorMessage, message = message });
        }
        #endregion tipoCampagnaPubblicitaria
        #region tipoContatto
        [HttpGet]

        public ActionResult tipoCanaleAcquisizione(String errorMessage, String message)
        {

            ViewBag.errorMessage = errorMessage == null ? String.Empty : errorMessage;
            ViewBag.message = message == null ? String.Empty : message;
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            return View(
                (from tipoCanaleAcquisizione in db.TipoCanaleAcquisizione select tipoCanaleAcquisizione).ToList()
                );
        }

        [HttpPost]

        public ActionResult tipoCanaleAcquisizione(TipoCanaleAcquisizione ta)
        {
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
            {
                if ((from fp in db.TipoCanaleAcquisizione
                     where (fp.descrizione == ta.descrizione)
                     select fp).FirstOrDefault() != null)
                {
                    return RedirectToAction("tipoCanaleAcquisizione", new
                    {
                        errorMessage = "Canale acquisizione " + ta.descrizione + " già censito"
                    });
                }
                else
                {
                    db.TipoCanaleAcquisizione.Add(ta);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("tipoCanaleAcquisizione", new { message = "inserimento campagna  : " + ta.descrizione + " avvenuto con successo" });
        }

        [HttpGet]

        public ActionResult cancellaTipoCanaleAcquisizione(int id)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
            {
                TipoCanaleAcquisizione tipoCanaleAcquisizione = db.TipoCanaleAcquisizione.Find(id);
                if (tipoCanaleAcquisizione == null)
                {
                    errorMessage = "impossibile eliminare il canale di acquisizione " + id + " in quanto non censito";
                }
                else
                {
                    db.TipoCanaleAcquisizione.Remove(tipoCanaleAcquisizione);
                    db.SaveChanges();
                    message = "canale acquisizione " + tipoCanaleAcquisizione.descrizione + " eliminato con successo";
                }
            }
            return RedirectToAction("TipoCanaleAcquisizione", new { errorMessage = errorMessage, message = message });
        }
        #endregion tipoCanaleAcquisizione
        #region tipoLuogoritrovo
        [HttpGet]

        public ActionResult tipoLuogoRitrovo(String errorMessage, String message)
        {

            ViewBag.errorMessage = errorMessage == null ? String.Empty : errorMessage;
            ViewBag.message = message == null ? String.Empty : message;
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            return View(
                (from tipoLuogoRitrovo in db.TipoLuogoRitrovo select tipoLuogoRitrovo).ToList()
                );
        }

        [HttpPost]

        public ActionResult tipoLuogoRitrovo(TipoLuogoRitrovo ta)
        {
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
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
            }
            return RedirectToAction("tipoLuogoRitrovo", new { message = "inserimento luogo ritrovo  : " + ta.descrizione + " avvenuto con successo" });
        }

        [HttpGet]

        public ActionResult cancellaTipoLuogoRitrovo(int id)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
            {
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
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            return View(
                (from tipoContatto in db.TipoContatto select tipoContatto).ToList()
                );
        }

        [HttpPost]

        public ActionResult tipoContatto(TipoContatto ta)
        {
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
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
            }
            return RedirectToAction("tipoContatto", new { message = "inserimento tipo contatto  : " + ta.descrizione + " avvenuto con successo" });
        }

        [HttpGet]

        public ActionResult cancellaTipoContatto(int id)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
            {
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
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            return View(
                (from tipoRiferimento in db.TipoRiferimento select tipoRiferimento).ToList()
                );
        }

        [HttpPost]

        public ActionResult tipoRiferimento(TipoRiferimento ta)
        {
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
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
            }
            return RedirectToAction("tipoRiferimento", new { message = "inserimento tipo riferimento  : " + ta.descrizione + " avvenuto con successo" });
        }

        [HttpGet]

        public ActionResult cancellaTipoRiferimento(int id)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
            {
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
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            return View(
                (from categoriaImpiego in db.TipoCategoriaImpiego select categoriaImpiego).ToList()
                );
        }

        [HttpPost]

        public ActionResult tipoCategoriaImpiego(TipoCategoriaImpiego ta)
        {
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
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
            }
            return RedirectToAction("tipoCategoriaImpiego", new { message = "inserimento Categoria impiego  : " + ta.descrizione + " avvenuto con successo" });
        }

        [HttpGet]

        public ActionResult cancellaTipoCategoriaImpiego(int id)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
            {
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
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            return View(
                (from tipoProdotto in db.TipoProdotto select tipoProdotto).ToList()
                );
        }

        [HttpPost]

        public ActionResult tipoProdotto(TipoProdotto ta)
        {
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
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
            }
            return RedirectToAction("tipoProdotto", new { message = "inserimento prodotto  : " + ta.descrizione + " avvenuto con successo" });
        }

        [HttpGet]

        public ActionResult cancellaTipoProdotto(int id)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
            {
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
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            return View(
                (from tipoCategoriaAmministrazione in db.TipoCategoriaAmministrazione select tipoCategoriaAmministrazione).ToList()
                );
        }

        [HttpPost]

        public ActionResult tipoCategoriaAmministrazione(TipoCategoriaAmministrazione ta)
        {
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
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
            }
            return RedirectToAction("tipoCategoriaAmministrazione", new { message = "inserimento categoria  : " + ta.descrizione + " avvenuto con successo" });
        }

        [HttpGet]

        public ActionResult cancellaTipoCategoriaAmministrazione(int id)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
            {
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
            }
            return RedirectToAction("TipoCategoriaAmministrazione", new { errorMessage = errorMessage, message = message });
        }
        #endregion tipoCategoriaAmministrazione
        #region tipoAssumibilitaAmministrazione
        [HttpGet]

        public ActionResult tipoAssumibilitaAmministrazione(String errorMessage, String message)
        {

            ViewBag.errorMessage = errorMessage == null ? String.Empty : errorMessage;
            ViewBag.message = message == null ? String.Empty : message;
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            return View(
                (from tipoAssumibilitaAmministrazione in db.TipoAssumibilitaAmministrazione select tipoAssumibilitaAmministrazione).ToList()
                );
        }

        [HttpPost]

        public ActionResult tipoAssumibilitaAmministrazione(TipoAssumibilitaAmministrazione ta)
        {
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
            {
                if ((from fp in db.TipoAssumibilitaAmministrazione
                     where (fp.descrizione == ta.descrizione)
                     select fp).FirstOrDefault() != null)
                {
                    return RedirectToAction("tipoAssumibilitaAmministrazione", new
                    {
                        errorMessage = "tipo assumibilita " + ta.descrizione + " già censita"
                    });
                }
                else
                {
                    db.TipoAssumibilitaAmministrazione.Add(ta);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("tipoAssumibilitaAmministrazione", new { message = "inserimento tipo assumibilita: " + ta.descrizione + " avvenuto con successo" });
        }

        [HttpGet]

        public ActionResult cancellaTipoAssumibilitaAmministrazione(int id)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
            {
                TipoAssumibilitaAmministrazione tipoAssumibilitaAmministrazione = db.TipoAssumibilitaAmministrazione.Find(id);
                if (tipoAssumibilitaAmministrazione == null)
                {
                    errorMessage = "impossibile eliminare il tipo assumibilita " + id + " in quanto non censito";
                }
                else
                {
                    db.TipoAssumibilitaAmministrazione.Remove(tipoAssumibilitaAmministrazione);
                    db.SaveChanges();
                    message = "tipo ammissibilita " + tipoAssumibilitaAmministrazione.descrizione + " eliminata con successo";
                }
            }
            return RedirectToAction("TipoAssumibilitaAmministrazione", new { errorMessage = errorMessage, message = message });
        }
        #endregion tipoAssumibilitaAmministrazione
        #region tipoErogazione
        [HttpGet]

        public ActionResult tipoErogazione(String errorMessage, String message)
        {

            ViewBag.errorMessage = errorMessage == null ? String.Empty : errorMessage;
            ViewBag.message = message == null ? String.Empty : message;
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            return View(db.TipoErogazione.ToList());
        }

        [HttpPost]

        public ActionResult tipoErogazione(TipoErogazione tipoErog)
        {
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
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
            }
            return RedirectToAction("tipoErogazione", new { message = "inserimento tipo erogazione: " + tipoErog.descrizione + " avvenuto con successo" });
        }

        [HttpGet]

        public ActionResult cancellaTipoErogazione(string sigla)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
            {
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
            }
            return RedirectToAction("TipoErogazione", new { errorMessage = errorMessage, message = message });
        }
        #endregion tipoErogazione
        #region tipoAgenzia
        [HttpGet]

        public ActionResult tipoAgenzia(String errorMessage, String message)
        {

            ViewBag.errorMessage = errorMessage == null ? String.Empty : errorMessage;
            ViewBag.message = message == null ? String.Empty : message;
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            return View(db.TipoAgenzia.ToList());
        }

        [HttpPost]

        public ActionResult tipoAgenzia(TipoAgenzia tipoAgenzia)
        {
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
            {
                if ((from fp in db.TipoAgenzia
                     where (fp.descrizione == tipoAgenzia.descrizione)
                     select fp).FirstOrDefault() != null)
                {
                    return RedirectToAction("tipoAgenzia", new
                    {
                        errorMessage = "tipo agenzia " + tipoAgenzia.descrizione + " già censita"
                    });
                }
                else
                {
                    db.TipoAgenzia.Add(tipoAgenzia);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("tipoAgenzia", new { message = "inserimento tipo agenzia: " + tipoAgenzia.descrizione + " avvenuto con successo" });
        }

        [HttpGet]

        public ActionResult cancellaTipoAgenzia(int id)
        {
            String errorMessage = string.Empty;
            string message = String.Empty;
            using (MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
            {
                TipoAgenzia tipoErogazione = db.TipoAgenzia.Find(id);
                if (tipoErogazione == null)
                {
                    errorMessage = "impossibile eliminare il tipo agenzia " + id + " in quanto non censito";
                }
                else
                {
                    db.TipoAgenzia.Remove(tipoErogazione);
                    db.SaveChanges();
                    message = "tipo agenzia " + tipoErogazione.descrizione + " eliminata con successo";
                }
            }
            return RedirectToAction("TipoAgenzia", new { errorMessage = errorMessage, message = message });
        }
        #endregion tipoErogazione
        #region Parametro
        [HttpGet]

        public ActionResult Parametro(String errorMessage, String message)
        {

            ViewBag.errorMessage = errorMessage == null ? String.Empty : errorMessage;
            ViewBag.message = message == null ? String.Empty : message;
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            return View(db.Parametri.ToList());
        }
        [HttpPost]

        public ActionResult Parametro(Parametro p)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            Parametro parDadb = db.Parametri.Find(p.id);
            parDadb.value = p.value;
            db.SaveChanges();
            ViewBag.message = "parametro salvato con successo";
            return View(db.Parametri.ToList());
        }
        #endregion
    }
}

