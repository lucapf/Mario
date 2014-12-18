using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mediatori.Controllers
{
    public class SimulazioneController : Controller
    {

        private BusinessModel.SimulazioneFinanziaria.SimulazioneManager manager = null;


        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            manager = new BusinessModel.SimulazioneFinanziaria.SimulazioneManager(System.Configuration.ConfigurationManager.AppSettings["pcc.url"]);


        }


        public ActionResult Index(BusinessModel.SimulazioneFinanziaria.SimulazioneModel model)
        {
            try
            {
                model.versione = manager.getVersion();

                model.agenzie = manager.getAgenzie();

#if DEBUG
                model.numeroRate = 24;
                model.sesso = "M";
                model.importoRata = 150;
                model.dataDiNascita = new DateTime(1975, 11, 7);
                model.dataAssunzione = new DateTime(2001, 8, 1);
#endif

            }
            catch (Exception ex)
            {
                TempData["Message"] = new MyMessage(ex);
            }
            finally
            {
                if (manager != null)
                {
                    manager.close();
                }
            }

            return View(model);
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult IndexPost(BusinessModel.SimulazioneFinanziaria.SimulazioneModel model)
        {

            if (!ModelState.IsValid)
            {
                model.versione = manager.getVersion();
                model.agenzie = manager.getAgenzie();

                if (model.agenziaId != null)
                {
                    model.prodotti = manager.getProdotti((long)model.agenziaId);
                }

                return View(model);
            }


            //simulazione



            manager.getAllPossiblePortafoglioCombinationFor(model);




            model.versione = manager.getVersion();
            model.agenzie = manager.getAgenzie();

            if (model.agenziaId != null)
            {
                model.prodotti = manager.getProdotti((long)model.agenziaId);
            }


            return View(model);
        }



        public ActionResult GetProdotti(int agenziaId)
        {
            List<MyManagerCSharp.Models.MyItem> prodotti = manager.getProdotti(agenziaId);

            return Json(prodotti, JsonRequestBehavior.AllowGet);
        }

    }
}