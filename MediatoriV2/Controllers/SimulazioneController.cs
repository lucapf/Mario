using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mediatori.Controllers
{
    public class SimulazioneController : MyBaseController
    {

        private BusinessModel.SimulazioneFinanziaria.SimulazioneManager manager = null;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (db != null)
            {
                manager = new BusinessModel.SimulazioneFinanziaria.SimulazioneManager(db.Database.Connection, System.Configuration.ConfigurationManager.AppSettings["pcc.url"]);


            }
        }

        public ActionResult Index(BusinessModel.SimulazioneFinanziaria.SimulazioneModel model)
        {
            try
            {
                model.versione = manager.getVersion();
                model.agenzie = manager.getAgenzie();

#if DEBUG
                //model.numeroRate = 120;
                //model.sesso = "M";
                //model.importoRata = 150;
                //model.dataDiNascita = new DateTime(1975, 11, 7);
                //model.dataAssunzione = new DateTime(2001, 8, 1);
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

        [HttpGet]
        public ActionResult Select(BusinessModel.SimulazioneFinanziaria.SimulazioneModel model)
        {
            if (Session[MyConstants.MySessionData.ProdottiSimulazioneFinanziaria.ToString()] == null)
            {
                return View("Index");
            }

            return View(model);
        }



        [HttpPost]
        [ActionName("Select")]
        public ActionResult SelectPost(BusinessModel.SimulazioneFinanziaria.SimulazioneModel model)
        {
            if (!ModelState.IsValid)
            {
                model.versione = manager.getVersion();
                model.agenzie = manager.getAgenzie();

                if (model.agenziaId != null)
                {
                    model.prodotti = manager.getProdotti((long)model.agenziaId);
                }

                return View("Index", model);
            }

            BusinessModel.PccWS.importiPraticaVO[] temp;
            temp  = manager.getAllPossiblePortafoglioCombinationFor(model);

            Debug.WriteLine("COUNT : " + temp.Length);

            Session[MyConstants.MySessionData.ProdottiSimulazioneFinanziaria.ToString()] = temp;

            //model.versione = manager.getVersion();
            //model.agenzie = manager.getAgenzie();

            //if (model.agenziaId != null)
            //{
            //    model.prodotti = manager.getProdotti((long)model.agenziaId);
            //}


            return View(model);
        }




        public ActionResult createForSegnalazione(int codiceSegnalazione)
        {
            if (!ModelState.IsValid)
            {
                //model.versione = manager.getVersion();
                //model.agenzie = manager.getAgenzie();

                //if (model.agenziaId != null)
                //{
                //    model.prodotti = manager.getProdotti((long)model.agenziaId);
                //}

                return RedirectToAction("Details", "Segnalazioni", new { id = codiceSegnalazione });
            }


            Segnalazione segnalazione;
            segnalazione = db.Segnalazioni.Include("preventivi").Include("contatto").Where(p => p.id == codiceSegnalazione).First();
            if (segnalazione == null)
            {
                return HttpNotFound();
            }



            BusinessModel.SimulazioneFinanziaria.SimulazioneModel model = new BusinessModel.SimulazioneFinanziaria.SimulazioneModel();
            model.segnalazioneId = codiceSegnalazione;

            model.numeroRate = segnalazione.durataRichiesta;

            if (segnalazione.contatto.sesso == EnumSesso.MASCHIO)
            {
                model.sesso = "M";
            }
            else
            {
                model.sesso = "F";
            }

            model.importoRata = segnalazione.rataRichiesta;
            model.dataDiNascita = segnalazione.contatto.dataNascita;
            if (segnalazione.contatto.impieghi.ToList<Impiego>()[0] != null)
            {
                model.dataAssunzione = segnalazione.contatto.impieghi.ToList<Impiego>()[0].dataAssunzione;
            }


            // Session[MyConstants.MySessionData.ProdottiSimulazioneFinanziaria.ToString()] = manager.getAllPossiblePortafoglioCombinationFor(model);

            try
            {
                model.versione = manager.getVersion();
                model.agenzie = manager.getAgenzie();
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



            return View("Index", model);
        }



        public ActionResult GetProdotti(int agenziaId)
        {
            List<MyManagerCSharp.Models.MyItem> prodotti = manager.getProdotti(agenziaId);

            return Json(prodotti, JsonRequestBehavior.AllowGet);
        }




        public ActionResult Details(int id, int? segnalazioneId)
        {
            if (Session[MyConstants.MySessionData.ProdottiSimulazioneFinanziaria.ToString()] == null)
            {
                return View("Index");
            }

            BusinessModel.SimulazioneFinanziaria.SimulazioneModel model = new BusinessModel.SimulazioneFinanziaria.SimulazioneModel();
            model.segnalazioneId = segnalazioneId;
            model.indice = id;

            return View(model);
        }


        public ActionResult Confirm(int indice, int segnalazioneId)
        {
            if (Session[MyConstants.MySessionData.ProdottiSimulazioneFinanziaria.ToString()] == null)
            {
                return View("Index");
            }

            BusinessModel.PccWS.importiPraticaVO prodotto;
            prodotto = (Session[MyConstants.MySessionData.ProdottiSimulazioneFinanziaria.ToString()] as BusinessModel.PccWS.importiPraticaVO[])[indice];

            //SALVO il prodotto selezionato nel preventivo!
            try
            {
                manager.openConnection(); 
                manager.insert(prodotto, segnalazioneId);
            }
            finally
            {
                manager.closeConnection(); 
            }

            return RedirectToAction("Details", "Segnalazioni", new { id = segnalazioneId });
        }
    }
}