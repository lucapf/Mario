using mediatori.Controllers.CQS;
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
        //
        // GET: /Log/

        public ActionResult Index()
        {
            return View();
        }

        public String getEventoInserimento(int idEntita, EnumEntitaRiferimento entitaRiferimento)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            LogEventi logEventi = LogEventiManager.getEventoCreazione(idEntita, entitaRiferimento, db);
            return new JavaScriptSerializer().Serialize(logEventi);
        }
    }
}
