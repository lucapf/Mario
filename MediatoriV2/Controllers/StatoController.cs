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
        //
        // GET: /Stato/
        [HttpGet]
        public String targets(int codiceStato,EnumEntitaAssociataStato entita)
        {
            StatoSearch statoSearch = new StatoSearch();
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            statoSearch.successiviDi = codiceStato;
            statoSearch.entita = entita;
            List<Stato> listaSati = new StatoBusiness().findByFilter(statoSearch,db);
            string strListaStati = JsonConvert.SerializeObject(listaSati);
            return strListaStati;
        }
        [HttpPost]
        public String update(int codiceStato, int codiceEntita, EnumEntitaAssociataStato entita)
        {
            Stato statoSegnalazione = new Stato();
            using (var db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri))
            {
                if (entita == EnumEntitaAssociataStato.SEGNALAZIONE)
                {
                    statoSegnalazione=db.StatiSegnalazione.Find(codiceStato);
                    db.Database.ExecuteSqlCommand("update Segnalazione set stato_id=" + codiceStato + " where id=" + codiceEntita);

                }
            }
            return statoSegnalazione.descrizione;
        }

    }
}
