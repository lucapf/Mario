using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mediatori.Models.etc;
using mediatori.Controllers.Business.CQS;
using mediatori.Models;
using Newtonsoft.Json;
using mediatori.Models.Anagrafiche;

namespace mediatori.Controllers
{
    public class StatoController : MyBaseController
    {

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
        public ActionResult Update(int codiceStato, int codiceEntita, EnumEntitaAssociataStato entita)
        {
            Stato statoSegnalazione = new Stato();
            if (entita == EnumEntitaAssociataStato.SEGNALAZIONE)
            {
                statoSegnalazione = db.StatiSegnalazione.Find(codiceStato);
                db.Database.ExecuteSqlCommand("update Segnalazione set stato_id=" + codiceStato + " where id=" + codiceEntita);
            }

            return RedirectToAction("Details", "Segnalazioni", new { id = codiceEntita });

            //return statoSegnalazione.descrizione;
        }

    }
}
