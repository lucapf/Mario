using BusinessModel.Log;
using mediatori.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace mediatori.Controllers
{
    public class LogController : MyBaseController
    {
        
        public ActionResult Index()
        {
            return View();
        }

        public String getEventoInserimento(int idEntita, EnumEntitaRiferimento entitaRiferimento)
        {
            LogEventi logEventi = LogEventiManager.getEventoCreazione(idEntita, entitaRiferimento, db);
            return new JavaScriptSerializer().Serialize(logEventi);
        }


        public ActionResult Details(int idEntita, EnumEntitaRiferimento entitaRiferimento, mediatori.Models.etc.EnumEntitaAssociataStato? entitaAssociataStato = null)
        {
            LogEventiModel model = new LogEventiModel();
            model.idEntita = idEntita;
            model.entitaRiferimento = entitaRiferimento;
            model.entitaAssociataStato = entitaAssociataStato;


            model.listaEventi = db.LogsEventi.Where(e => e.idEntita == idEntita && e.tipoEntitaRiferimento == entitaRiferimento).OrderBy( e => e.dataInserimento).ToList();

            return View(model);
        }
    }
}
