using BusinessModel.Log;
using mediatori.Controllers.Business;
using mediatori.Controllers.Business.Anagrafiche;
using mediatori.Controllers.Business.Anagrafiche.Soggetto;
using mediatori.Models;
using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace mediatori.Controllers
{
    public class ErogazioneController : MyBaseController
    {

        [ChildActionOnly]
        public ActionResult Cedente(int cedenteId)
        {
            ErogazioniModel model = new ErogazioniModel();
            model.erogazioni = db.Erogazione.Include("coordinataErogazione").Include("tipoErogazione").Where(p => p.coordinataErogazione.cedenteId == cedenteId).ToList();
            model.cedenteId = cedenteId;
            valorizzaViewBag();
            return View("_Erogazioni", model);
        }


        private void valorizzaViewBag()
        {
            valorizzaViewBag(null);
        }

        private void valorizzaViewBag(Erogazione erogazione)
        {
            if (erogazione == null)
            {
                ViewBag.listaTipoErogazioni = new SelectList(db.TipoErogazione.OrderBy(p => p.descrizione), "id", "descrizione");
                ViewBag.listaTipoCoordinateErogazioni = new SelectList(db.TipoCoordinataErogazione.OrderBy(p => p.descrizione), "sigla", "descrizione");

            }
            else
            {
                ViewBag.listaTipoErogazioni = new SelectList(db.TipoErogazione.OrderBy(p => p.descrizione), "id", "descrizione");
                ViewBag.listaTipoCoordinateErogazioni = new SelectList(db.TipoCoordinataErogazione.OrderBy(p => p.descrizione), "sigla", "descrizione");
            }
        }

        //private void valorizzaListeErogazioniEdit(MainDbContext db, Erogazione erogazione)
        //{

        //    valorizzaListeErogazioniDetails(db);
        //    ViewBag.listaCoordinateErogazioni = new SelectList(db.CoordinataErogazione.ToList(), "coordinateErogazioni", "coordinateErogazioni");
        //}


        //public void valorizzaListeErogazioniDetails(MainDbContext db)
        //{
        //    List<SelectListItem> lsli = new List<SelectListItem>();
        //    lsli.Add(new SelectListItem { Text = "", Value = "" });
        //    ViewBag.listaCoordinateErogazioni = new SelectList(db.CoordinataErogazione.ToList(), "coordinateErogazioni", "coordinateErogazioni");
        //}


        //[ChildActionOnly]
        //public ActionResult Create(Erogazione erogazione)
        //{

        //    valorizzaViewBag(erogazione);

        //    ViewData.TemplateInfo.HtmlFieldPrefix = "erogazione";
        //    return View("ErogazionePartialEdit", erogazione);
        //}



        //[HttpPost]
        //public ActionResult CreateForCedente(Erogazione erogazione, int codiceCedente)
        //{
        ////    if (ModelState.IsValid)
        ////    {
        ////        erogazione = ErogazioneBusiness.createBySegnalazione(User.Identity.Name, codiceCedente, erogazione, db);
        ////    }
        //    return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        //}

        [HttpPost]
        public ActionResult CreateForPersonaFisica(Erogazione erogazione, int codiceCedente)
        {

            erogazione.tipoErogazione = (from ti in db.TipoErogazione
                                         where ti.descrizione == erogazione.tipoErogazione.descrizione
                                         select ti).First();

            erogazione.coordinataErogazione = (from t in db.CoordinataErogazione where t.tipoCoordinataErogazione.descrizione == erogazione.coordinataErogazione.tipoCoordinataErogazione.descrizione select t).First();

            ModelState.Clear();
            TryValidateModel(erogazione);


            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                  .SelectMany(v => v.Errors)
                  .Select(e => e.ErrorMessage));
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare l'impiego, verificare i dati: " + Environment.NewLine + message);
                return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
            }

            
              //  erogazione = ErogazioneBusiness.createBySegnalazione(User.Identity.Name, codiceCedente, erogazione, db);
            
           
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

    }
}