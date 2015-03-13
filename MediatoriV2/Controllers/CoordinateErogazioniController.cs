using BusinessModel.Log;
using mediatori.Controllers.Business;
using mediatori.Controllers.Business.Anagrafiche.Soggetto;
using mediatori.Models;
using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mediatori.Controllers.Business.Anagrafiche
{
    public class CoordinateErogazioniController : MyBaseController
    {
       


        [ChildActionOnly]
        public ActionResult Details(int cedenteId)
        {
            Cedente cedente;
            cedente = db.Cedenti.Include("coordinateErogazioni").Where(p => p.id == cedenteId).First();
            if (cedente == null)
            {
                return HttpNotFound();
            }


            CoordinateErogazioneModel model = new CoordinateErogazioneModel();
            model.coordinateErogazione = new List<CoordinateErogazione>();

            foreach (CoordinateErogazione i in cedente.coordinateErogazione)
            {
                model.coordinateErogazione.Add(i);
            }

            model.cedenteId = cedenteId;
            
                valorizzaViewBag();
                return View("_CoordinateErogazione", model);
                       
        }
        private void valorizzaViewBag()
        {
            ViewBag.listaTipoCoordinataErogazione = new SelectList(db.TipoCoordinataErogazione.OrderBy(p => p.descrizione), "sigla", "descrizione");
        }
    }
}