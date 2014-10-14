using mediatori.Controllers.Business;
using mediatori.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mediatori.Controllers
{
    public class NotaController : Controller
    {
        //
        // GET: /nota/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult notaPartial(Nota nota, EnumTipoAzione tipoAzione = EnumTipoAzione.MODIFICA)
        {
          
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            switch (tipoAzione)
            {
                case EnumTipoAzione.MODIFICA:
                    return View("NotaPartialEdit", nota);
                case EnumTipoAzione.INSERIMENTO:
                    return View("NotaPartialInsert", nota);
                default:
                    return View("NotaPartialDetail", nota);
            }  
        }
        [HttpPost]
        public ActionResult CreateForSegnalazione(Nota nota, int codiceSegnalazione)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            nota = new NotaBusiness().valorizzaDatiDefault(nota, User.Identity.Name);
            ModelState.Clear();
            TryValidateModel(nota);
            if (ModelState.IsValid)
            {
                nota = NotaBusiness.createBySegnalazione(User.Identity.Name, codiceSegnalazione, nota, db);
            }
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }
        [HttpPost]
        public ActionResult CreateForSoggettoGiuridico(Nota nota, int codiceSoggettoGiuridico)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            nota = new NotaBusiness().valorizzaDatiDefault(nota, User.Identity.Name);
            ModelState.Clear();
            TryValidateModel(nota);
            if (ModelState.IsValid)
            {
                nota = NotaBusiness.createBySoggettoGiuridico(User.Identity.Name, codiceSoggettoGiuridico, nota, db);
            }
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

    }
}
