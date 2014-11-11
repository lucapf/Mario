using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyManagerCSharp;


using System.Diagnostics;


namespace mediatori.Controllers
{
    public class UtentiController : Controller
    {
        private MyUsers.UserManager manager = new MyUsers.UserManager("DefaultConnection");

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

                 manager.setProfili(model.Utente);
                //manager.setGroups(model.Utente);
                //manager.setRoles(model.Utente);

                 MyUsers.CustomerManager c = new MyUsers.CustomerManager(manager.getConnection());
                c.set(model.Utente);


                //REPORTS
                // MyManagerCSharp.RGraph.Models.RGraphModel report;
                //MyUsers.Reports.ReportsUsers reportManager = new MyUsers.Reports.ReportsUsers(manager.getConnection());

                model.Profili = manager.getProfili();


            }
            finally
            {
                manager.closeConnection();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProfilo(long? userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("userId");
            }

            Debug.WriteLine("UserId: " + userId);
            Debug.WriteLine("ProfiliIDs: " + Request["profiliIDs"]);

            manager.openConnection();
            try
            {

                List<MyUsers.Models.MyProfile> lista = new List<MyUsers.Models.MyProfile>();

                foreach (string id in Request["profiliIDs"].Split(','))
                {
                    Debug.WriteLine("Profile: " + id);
                    lista.Add(new MyUsers.Models.MyProfile(id));
                }
                manager.updateProfili(lista, (long)userId);


            }
            finally
            {
                manager.closeConnection();
            }


            return RedirectToAction("Details", new { id = userId });

            //return new RedirectResult(Url.Action("Details", new { id = userId }) + "#tabs-3");
        }


        public ActionResult Register()
        {
            Models.RegisterModel model = new Models.RegisterModel();

            model.ProfiliDisponibili = manager.getProfili();
            model.UserName = "";
            model.Password = "";
            model.ConfirmPassword = "";


            //model.roles 

            return View(model);
        }


        [HttpPost]
        //[Authorize(Roles = "Amministratore")]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Models.RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                MyUsers.Models.MyUser u = new MyUsers.Models.MyUser();
                u.login = model.UserName;
                u.password = model.Password;


                manager.openConnection();
                try
                {
                    //manager.insert(u);
                    //manager.updateProfili(u);
                }
                finally
                {
                    manager.closeConnection();
                }

                return RedirectToAction("Index");
            }


            model.ProfiliDisponibili = manager.getProfili();
            model.Password = "";
            model.ConfirmPassword = "";

            return View(model);
        }


    }
}
