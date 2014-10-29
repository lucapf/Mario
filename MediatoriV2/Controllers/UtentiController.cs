using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyManagerCSharp;
using MyUsers.Models;
using MyUsers;
using System.Diagnostics;


namespace mediatori.Controllers
{
    public class UtentiController : Controller
    {
        private UserManager manager = new UserManager("DefaultConnection");

        public ActionResult Index(MyUsers.Models.SearchUsers model)
        {
            if (model.Sort == "Login")
            {
                model.Sort = "my_login";
            }
            else if (model.Sort == "DateAdded")
            {
                model.Sort = "date_added";
            }
            else if (model.Sort == "DateLastLogin")
            {
                model.Sort = "date_last_login";
            }
            TryUpdateModel(model.filter, "filter");

            Debug.WriteLine(String.Format("Filtri di ricerca  Nome: {0} Email: {1}", model.filter.nome, model.filter.email));


            manager.openConnection();
            try
            {
                manager.getList(model);
            }
            finally
            {
                manager.closeConnection();
            }

            return View(model);
        }


        public ActionResult Details(long id = 0)
        {
            Models.MyUserModel model = new Models.MyUserModel();
            manager.openConnection();
            try
            {
                model.Utente = manager.getUser(id);

                if (model.Utente == null)
                {
                    return HttpNotFound();
                }

                //  manager.setProfilo(model.Utente);
                //manager.setGroups(model.Utente);
                manager.setRolesV2(model.Utente);

                CustomerManager c = new CustomerManager(manager.getConnection());
                c.set(model.Utente);


                //REPORTS
                // MyManagerCSharp.RGraph.Models.RGraphModel report;
                //MyUsers.Reports.ReportsUsers reportManager = new MyUsers.Reports.ReportsUsers(manager.getConnection());

                model.Ruoli = manager.getRoles();


            }
            finally
            {
                manager.closeConnection();
            }
            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRole(long? userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("userId");
            }

            Debug.WriteLine("UserId: " + userId);
            Debug.WriteLine("RuoliIDs: " + Request["ruoliIDs"]);

            manager.openConnection();
            try
            {

                List<MyRole> lista = new List<MyRole>();

                foreach (string id in Request["ruoliIDs"].Split(','))
                {
                    Debug.WriteLine("Ruolo: " + id);
                    lista.Add(new MyRole(id));
                }
                manager.updateRoles(lista, (long)userId);


            }
            finally
            {
                manager.closeConnection();
            }


            return RedirectToAction("Details", new { id = userId });

            //return new RedirectResult(Url.Action("Details", new { id = userId }) + "#tabs-3");
        }
    }
}
