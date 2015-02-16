using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mediatori.Models.etc;
using mediatori.Controllers.Business.CQS;
using mediatori.Models;
using mediatori.Models.Anagrafiche;

namespace mediatori.Controllers
{
    public class StatoController : MyBaseController
    {
        private BusinessModel.Segnalazione.SegnalazioneManager manager = null;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (db != null)
            {
                manager = new BusinessModel.Segnalazione.SegnalazioneManager(db.Database.Connection);
            }
        }

        [HttpGet]
        public ActionResult targets(int codiceStato, EnumEntitaAssociataStato entita)
        {
            //StatoSearch statoSearch = new StatoSearch();
            // statoSearch.successiviDi = codiceStato;
            //statoSearch.entita = entita;
            //List<Stato> listaSati = new StatoBusiness().findByFilter(statoSearch, db);

            List<Stato> listaStati = new StatoBusiness().getStatiSuccessivi(codiceStato, db);

            //string strListaStati = JsonConvert.SerializeObject(listaStati);
            //return strListaStati;
            return Json(listaStati);
        }

        [HttpPost]
        public ActionResult Update(int codiceStato, int codiceEntita, EnumEntitaAssociataStato entita, DateTime? dataPromemoria)
        {
            EnumEntitaRiferimento entitaRiferimento = EnumEntitaRiferimento.STATO;
            Stato statoOriginale = null;

            if (entita == EnumEntitaAssociataStato.PRATICA)
            {

                Models.Pratica.Pratica pratica = db.Pratiche.Include("stato").Where(p => p.id == codiceEntita).FirstOrDefault();
                if (pratica == null)
                {
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, String.Format("Codice pratica non trovato: {0}", codiceEntita));
                    return RedirectToAction("Details", "Pratiche", new { id = codiceEntita });
                }

                statoOriginale = pratica.stato;
                //entitaRiferimento = EnumEntitaRiferimento.PRATICA;

                //Rel. 1.0.0.11 Bug 513 
                //verifico la presenza dei dati accessori del cedente 
                Models.Anagrafiche.Cedente cedente;
                cedente = db.Cedenti.Include("indirizzi").Include("impieghi").Include("documentiIdentita").Where(p => p.id == pratica.cedenteId).FirstOrDefault();
                if (cedente == null)
                {
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, String.Format("Codice cedente non trovato: {0}", pratica.cedenteId));
                    return RedirectToAction("Details", "Pratiche", new { id = codiceEntita });
                }

                if (cedente.indirizzi.Count == 0)
                {
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, String.Format("Attenzione, angrafica cedente incompleta. Per poter effettuare il passaggio di stato della pratica, occorre inserire almeno un indirizzo per il cedente"));
                    return RedirectToAction("Details", "Pratiche", new { id = codiceEntita });
                }

                if (cedente.impieghi.Count == 0)
                {
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, String.Format("Attenzione, angrafica cedente incompleta. Per poter effettuare il passaggio di stato della pratica, occorre inserire almeno un impiego per il cedente"));
                    return RedirectToAction("Details", "Pratiche", new { id = codiceEntita });
                }

                if (cedente.documentiIdentita.Count == 0)
                {
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, String.Format("Attenzione, angrafica cedente incompleta. Per poter effettuare il passaggio di stato della pratica, occorre inserire almeno un documento di identità per il cedente"));
                    return RedirectToAction("Details", "Pratiche", new { id = codiceEntita });
                }

            }


            if (entita == EnumEntitaAssociataStato.SEGNALAZIONE)
            {
                Models.Anagrafiche.Segnalazione segnalazione = db.Segnalazioni.Include("stato").Where(p => p.id == codiceEntita).FirstOrDefault();
                if (segnalazione == null)
                {
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, String.Format("Codice segnalzione non trovato: {0}", codiceEntita));
                    return RedirectToAction("Details", "Segnalazioni", new { id = codiceEntita });
                }

                statoOriginale = segnalazione.stato;
                //entitaRiferimento = EnumEntitaRiferimento.SEGNALAZIONE;

            }




            Stato statoNuovo = null;
            statoNuovo = db.StatiSegnalazione.Find(codiceStato);
            if (statoNuovo != null)
            {
                try
                {
                    manager.openConnection();
                    manager.updateStato(codiceStato, codiceEntita, dataPromemoria);

                    LogEventi le = BusinessModel.Log.LogEventiManager.getEventoForUpdate(User.Identity.Name, codiceEntita, entitaRiferimento, statoOriginale, statoNuovo);

                    BusinessModel.Log.LogEventiManager.save(le, db);
                }
                finally
                {
                    manager.closeConnection();
                }
            }


            if (entita == EnumEntitaAssociataStato.SEGNALAZIONE)
            {
                return RedirectToAction("Details", "Segnalazioni", new { id = codiceEntita });
            }


            // if (entita == EnumEntitaAssociataStato.PRATICA)
            //{
            return RedirectToAction("Details", "Pratiche", new { id = codiceEntita });
            //}

        }

    }
}
