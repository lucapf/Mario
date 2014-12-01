using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;


namespace mediatori.Controllers
{

      [MyAuthorize(Roles = new string [] {MyConstants.Profilo.FRONTOFFICE , MyConstants.Profilo.BACKOFFICE  , MyConstants.Profilo.ADMIN , MyConstants.Profilo.COLLABORATORE } )]
    public class PraticheController : MyBaseController
    {

        public ActionResult Index(Models.Pratica.SearchPratica model)
        {
            IQueryable<mediatori.Models.Pratica.Pratica> query = db.Pratiche.Include("cedente").Include("prodottoRichiesto");

            //model.Pratiche = db.Pratiche.Include("cedente").Include("prodottoRichiesto");

            Debug.WriteLine("Profilo: " + (Session["MySessionData"] as MyManagerCSharp.MySessionData).Profili);

            if ((Session["MySessionData"] as MyManagerCSharp.MySessionData).Profili.IndexOf(MyConstants.Profilo.COLLABORATORE.ToString()) > -1)
            {
                query = query.Where(p => p.utenteInserimento == User.Identity.Name);

            }
            model.Pratiche = query.ToList();

            return View(model);
        }

        public ActionResult Details(int id)
        {
            mediatori.Models.Pratica.Pratica model;
            model = db.Pratiche.Include("cedente").Include("preventivi").Include("note").Where(p => p.id == id).FirstOrDefault();

            if (model == null)
            {
                return HttpNotFound();
            }



            return View(model);
        }

    }
}