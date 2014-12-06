using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyManagerCSharp;


using System.Diagnostics;


namespace mediatori.Controllers
{
    public class UtentiController : MyBaseController
    {
        private MyUsers.UserManager manager = null;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (db != null)
            {
                manager = new MyUsers.UserManager(db.Database.Connection);
            }
        }

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

                //Aggiungo il profilo !!

                foreach (MyUsers.Models.MyUser u in model.Utenti)
                {

                    manager.setProfili(u);
                    manager.setGroups(u);
                }

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
                manager.setGroups(model.Utente);
                //manager.setRoles(model.Utente);

                //                MyUsers.CustomerManager c = new MyUsers.CustomerManager(manager.getConnection());
                //              c.set(model.Utente);


                //REPORTS
                // MyManagerCSharp.RGraph.Models.RGraphModel report;
                //MyUsers.Reports.ReportsUsers reportManager = new MyUsers.Reports.ReportsUsers(manager.getConnection());



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
                u.login = model.UserName.Trim();
                u.password = model.Password.Trim();
                u.isEnabled = true;

                bool esito;
                esito = MyManagerCSharp.RegularExpressionManager.isValidEmail(u.login);
                if (esito == false)
                {
                    string messaggio;
                    messaggio = "Inserire un indirizzo email valido.";
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, messaggio);
                    // ModelState.AddModelError("", messaggio);
                    model.ProfiliDisponibili = manager.getProfili();
                    return View(model);
                }

                string dominio;
                dominio = u.login.Split('@')[1];


                if ((Session["MySessionData"] as SessionData).Dominio != dominio)
                {
                    string messaggio;
                    messaggio = "L'email dell'utente deve appartenere al dominio: @" + (Session["MySessionData"] as SessionData).Dominio  ;
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, messaggio);
                    // ModelState.AddModelError("", messaggio);
                    model.ProfiliDisponibili = manager.getProfili();
                    return View(model);

                }

            
                long userId;
                try
                {
                    manager.openConnection(); 

                    userId = manager.getUserIdFromLogin(u.login);
                    if (userId != -1)
                    {
                        TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Spiacenti, risulta già presente un utente registrato con questo indizzo email.");
                        model.ProfiliDisponibili = manager.getProfili();
                        return View(model);
                    }


                    userId = manager.insert(u);

                    if (userId != -1)
                    {
                        /*** PROFILI **/
                        Debug.WriteLine("Profili: " + Request.Form["roles"]);
                        List<MyUsers.Models.MyProfile> lista = new List<MyUsers.Models.MyProfile>();
                        foreach (string id in Request["roles"].Split(','))
                        {
                            Debug.WriteLine("Profile: " + id);
                            lista.Add(new MyUsers.Models.MyProfile(id));
                        }
                        manager.updateProfili(lista, (long)userId);
                    }
                }
                finally
                {
                    manager.closeConnection();
                }

                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, "Registrazione utente " + u.login  + " conclusa con successo");
                return RedirectToAction("Index");
            }


            model.ProfiliDisponibili = manager.getProfili();
            // model.Password = "";
            model.ConfirmPassword = "";

            return View(model);
        }


        public ActionResult Delete(long id = 0)
        {
            MyUsers.Models.MyUser myuser = null;

            manager.openConnection();
            try
            {
                myuser = manager.getUser(id);
            }
            finally
            {
                manager.closeConnection();
            }


            if (myuser == null)
            {
                return HttpNotFound();
            }


            return View(myuser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            bool esito;
            manager.openConnection();
            try
            {
                esito = manager.delete((long)id);
            }
            finally
            {
                manager.closeConnection();
            }

            return RedirectToAction("Index");
        }



        public ActionResult Edit(long id)
        {
            MyWebApplication.Areas.Admin.Models.MyUserModel model = new MyWebApplication.Areas.Admin.Models.MyUserModel();

            //CustomerManager m1 = new CustomerManager(manager.getConnection());

            //List<MyGroup> listaGruppi = null;
            //List<MyCustomer> listaClienti = null;
            //List<MyProfile> listaProfili = null;

            manager.openConnection();
            try
            {
                model.Utente = manager.getUser(id);

                if (model.Utente == null)
                {
                    return HttpNotFound();
                }


                manager.setProfili(model.Utente);
                manager.setGroups(model.Utente);

                MyUsers.GroupManager groupManager = new MyUsers.GroupManager(manager.getConnection());
                model.Gruppi = groupManager.getList();

                //if (model.Utente.Gruppi != null)
                //{
                //    model.Gruppi = new MultiSelectList(groupManager.getList(), "gruppoId", "nome", model.Utente.Gruppi.Select(x => x.gruppoId).ToArray());
                //}
                //else
                //{
                //    model.Gruppi = new MultiSelectList(groupManager.getList(), "gruppoId", "nome", null);
                //}



                if (model.Utente.Profili != null && model.Utente.Profili.Count != 0)
                {
                    model.Profilo = new SelectList(manager.getProfili(), "profiloId", "nome", model.Utente.Profili[0].profiloId);
                }
                else
                {
                    model.Profilo = new SelectList(manager.getProfili(), "profiloId", "nome", null);
                }
            }
            finally
            {
                manager.closeConnection();
            }




            ////MultiSelectList sl = new MultiSelectList(listaGruppi.ToList(), "gruppoId", "nome", new int[] {2}  );


            //MultiSelectList sl = new MultiSelectList(listaGruppi.ToList(), "gruppoId", "nome", listaGruppi.ToList());
            //ViewBag.listaGruppi = sl;

            //ViewBag.listaGruppi = listaGruppi;

            //ViewBag.clienti = new SelectList(listaClienti.ToList(), "customerId", "ragioneSociale");

            //ViewBag.profili = new SelectList(listaProfili.ToList(), "profileId", "nome");


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MyWebApplication.Areas.Admin.Models.MyUserModel model)
        {
            //MyHelper.printRequest(Request);

            //if (myuser.Gruppi == null)
            //{
            //    ModelState.AddModelError(string.Empty, "Occorre selezionare almeno un Gruppo");
            //}

            //string gruppiSelected = Request.Form["gruppiIDs"];
            //Debug.WriteLine("gruppiIDs: " + gruppiSelected);

            //if (String.IsNullOrEmpty(gruppiSelected))
            //{
            //    ModelState.AddModelError(string.Empty, "Occorre selezionare almeno un Gruppo");
            //}



            if (ModelState.IsValid)
            {

                Debug.WriteLine("Nome: " + model.Utente.nome);

                bool esito;
                manager.openConnection();
                try
                {
                    //TODO: Rutigliano da modificare 02/02/2014
                    model.Utente.isEnabled = true;
                    //esito = manager.update(model.Utente);
                    esito = manager.updateEmail((long)model.Utente.userId, model.Utente.email);
                    //in questo caso gestiamo un solo profilo!
                    List<MyUsers.Models.MyProfile> listaProfili = new List<MyUsers.Models.MyProfile>();
                    Debug.WriteLine("ProfiloId: " + Request.Form["ProfiloId"]);
                    if (!String.IsNullOrEmpty(Request.Form["ProfiloId"]))
                    {
                        MyUsers.Models.MyProfile profilo = new MyUsers.Models.MyProfile(Request.Form["ProfiloId"]);
                        listaProfili.Add(profilo);
                    }

                    esito = manager.updateProfili(listaProfili, (long)model.Utente.userId);
                    if (esito)
                    {
                        string gruppiSelected = Request.Form["gruppiIDs"];
                        Debug.WriteLine("gruppiIDs: " + gruppiSelected);

                        List<MyUsers.Models.MyGroup> listaGruppi = new List<MyUsers.Models.MyGroup>();

                        if (!String.IsNullOrEmpty(gruppiSelected))
                        {
                            foreach (string id in gruppiSelected.Split(','))
                            {
                                listaGruppi.Add(new MyUsers.Models.MyGroup(int.Parse(id)));
                            }

                        }
                        model.Utente.Gruppi = listaGruppi;

                        MyUsers.GroupManager groupManager = new MyUsers.GroupManager(manager.getConnection());
                        groupManager.update(model.Utente.Gruppi, (long)model.Utente.userId);
                    }
                }
                finally
                {
                    manager.closeConnection();
                }
                return RedirectToAction("Details", new { id = model.Utente.userId });
            }





            var error = ModelState.SelectMany(x => x.Value.Errors);


            foreach (var value in ModelState.Values)
            {

                foreach (var merror in value.Errors)
                {

                    //throw new Exception(merror.ErrorMessage, merror.Exception);
                    Debug.WriteLine(merror.ErrorMessage + merror.Exception);

                }

            }

            //manager.openConnection();

            //populateComboBox(myuser);

            return View(model);
        }



        public ActionResult AutoCompleteLogin(string value)
        {
            Debug.WriteLine("AutoCompleteLogin: " + value);

            List<MyManagerCSharp.Models.MyItem> risultato;

            manager.openConnection();

            try
            {
                risultato = manager.getAutoCompleteLogin(value);
            }
            finally
            {
                manager.closeConnection();
            }

            return Json(risultato, JsonRequestBehavior.AllowGet);
        }

    }
}
