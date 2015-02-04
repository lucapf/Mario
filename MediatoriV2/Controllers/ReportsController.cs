using mediatori.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mediatori.Controllers
{
    [MyAuthorize(Roles = new string[] { MyConstants.Profilo.AMMINISTRATORE })]
    public class ReportsController : MyBaseController
    {

        private Mediatori.Reports.ReportsManager manager = null;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (db != null)
            {
                manager = new Mediatori.Reports.ReportsManager(db.Database.Connection);
            }
        }


        public ActionResult Index()
        {

            List<MenuElement> model = null;

            if (MySessionData.IsInProfile(MyConstants.Profilo.AMMINISTRATORE))
            {

                model = new List<MenuElement>(){
               
                  new MenuElement(){display="Segnalazioni", ordinamento=1,livello=1,role="Amministratore",action="Segnalazioni", controller="Reports"},
                  new MenuElement(){display="Pratiche", ordinamento=1,livello=1,role="Amministratore",action="Pratiche", controller="Reports"},
                  new MenuElement(){display="Rinnovi", ordinamento=1,livello=1,role="Amministratore",action="Rinnovi", controller="Reports"}
                };

            }
            return View(model);
        }



        public ActionResult Segnalazioni(MyManagerCSharp.RGraph.Models.WidgetRGraph model)
        {

            Debug.WriteLine("DAYS: " + model.Days);
            //MyManagerCSharp.RGraph.Models.WidgetRGraph model = new MyManagerCSharp.RGraph.Models.WidgetRGraph();
            MyManagerCSharp.RGraph.Models.RGraphModel report;
            try
            {

                report = new MyManagerCSharp.RGraph.Models.RGraphModel();
                report.Id = "report01";
                report.Width = "600px";
                report.Height = "400px";
                // report.ShowFiltroData = true;

                manager.getSegnalazioniPratiche(report, model.Days, Models.etc.EnumEntitaAssociataStato.SEGNALAZIONE);
                model.RGraph.Add(report);
            }
            finally
            {
                manager.closeConnection();
            }

            return View(model);
        }

        public ActionResult Pratiche(MyManagerCSharp.RGraph.Models.WidgetRGraph model)
        {
            MyManagerCSharp.RGraph.Models.RGraphModel report;
            try
            {

                report = new MyManagerCSharp.RGraph.Models.RGraphModel();
                report.Id = "report01";
                report.Width = "600px";
                report.Height = "400px";
                // report.ShowFiltroData = true;

                manager.getSegnalazioniPratiche(report, model.Days, Models.etc.EnumEntitaAssociataStato.PRATICA);
                model.RGraph.Add(report);
            }
            finally
            {
                manager.closeConnection();
            }

            return View(model);
        }

        public ActionResult Rinnovi()
        {
            return View();
        }
    }
}