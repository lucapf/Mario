using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mediatori.Models
{
    public class LogEventiController : Controller
    {
        //
        // GET: /LogEventi/

        public ActionResult Index(int idEntita, EnumEntitaRiferimento e)
        {
              MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
              List<LogEventi> logEventi = (from le in db.LogsEventi where le.idEntita == idEntita && le.tipoEntitaRiferimento == e select le).ToList<LogEventi>();
            return View(logEventi);
        }

    }
}
