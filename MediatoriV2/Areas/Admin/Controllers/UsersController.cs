using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyManagerCSharp;
using MyUsers.Models;
using MyUsers;
using System.Diagnostics;

namespace MyWebApplication.Areas.Admin.Controllers
{
    [MyAuthorize(Roles = "ADMIN")]
    public class UsersController : MyBaseController
    {

        private UserManager manager = new UserManager("utenti");

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
                manager.setGroups(model.Utente);
                manager.setRoles(model.Utente);

                CustomerManager c = new CustomerManager(manager.getConnection());
                c.set(model.Utente);


                //REPORTS
                MyManagerCSharp.RGraph.Models.RGraphModel report;
                MyUsers.Reports.ReportsUsers reportManager = new MyUsers.Reports.ReportsUsers(manager.getConnection());


                //reportManager.EnableOnClick = false;
                //report = reportManager.getLoginSuccessAndFailure("report01", 10, ManagerDB.Days.Tutti);
                //report.Width = "400px";
                //report.Height = "400px";
                //model.Reports.Add(report);

            }
            finally
            {
                manager.closeConnection();
            }
            return View(model);
        }

        public ActionResult Create()
        {
            List<string> t = new List<string>  {
        "Brendan Enrick", 
        "Kevin Kuebler", 
        "Todd Ropog"
    };

            ViewBag.lista = new SelectList(t);

            //populateComboBox(null);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MyUsers.Models.MyUser myuser)
        {
            string t = Request.Form["Gruppi"];
            Debug.WriteLine("Gruppi: " + t);

            string gruppiSelected = Request.Form["gruppiIDs"];
            Debug.WriteLine("gruppiIDs: " + gruppiSelected);


            //http://www.hanselman.com/blog/ASPNETWireFormatForModelBindingToArraysListsCollectionsDictionaries.aspx

            MyHelper.printRequest(Request);


            //if (myuser.Gruppi == null)
            //{
            //    ModelState.AddModelError(string.Empty, "Occorre selezionare almeno un Gruppo");
            //}

            if (String.IsNullOrEmpty(gruppiSelected))
            {
                ModelState.AddModelError(string.Empty, "Occorre selezionare almeno un Gruppo");
            }



            if (ModelState.IsValid)
            {
                long newId;
                manager.openConnection();
                try
                {
                    newId = manager.insert(myuser);
                    if (newId != -1)
                    {
                        List<MyGroup> lista = new List<MyGroup>();

                        foreach (string id in gruppiSelected.Split(','))
                        {
                            lista.Add(new MyGroup(int.Parse(id)));
                        }

                        myuser.Gruppi = lista;


                        GroupManager groupManager = new GroupManager(manager.getConnection());
                        groupManager.addUser(myuser.Gruppi, newId);
                    }
                }
                finally
                {
                    manager.closeConnection();
                }

                return RedirectToAction("Index");
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
            return View(myuser);
        }

        public ActionResult Edit(long id)
        {
            Models.MyUserModel model = new Models.MyUserModel();

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


                //listaGruppi = m.getList();
                //listaClienti = m1.getList();
                //listaProfili = manager.getProfileList();
                GroupManager groupManager = new GroupManager(manager.getConnection());
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
        public ActionResult Edit(Models.MyUserModel model)
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
                    esito = manager.updateEmail((long)model.Utente.userId , model.Utente.email);
                    //in questo caso gestiamo un solo profilo!
                    List<MyUsers.Models.MyProfile> lista = new List<MyProfile>();


                    Debug.WriteLine("ProfiloId: " + Request.Form["ProfiloId"]);
                    

                    if (!String.IsNullOrEmpty(Request.Form["ProfiloId"]))
                    {
                        MyUsers.Models.MyProfile profilo = new MyProfile(Request.Form["ProfiloId"]);
                        lista.Add(profilo );
                    }

                    esito = manager.updateProfili(lista, (long) model.Utente.userId);
                    if (esito)
                    {

                        //List<MyGroup> lista = new List<MyGroup>();

                        //foreach (string id in gruppiSelected.Split(','))
                        //{
                        //    lista.Add(new MyGroup(int.Parse(id)));
                        //}

                        //model.Utente.Gruppi = lista;

                        //GroupManager groupManager = new GroupManager(manager.getConnection());
                        //groupManager.update(model.Utente.Gruppi, model.Utente.userId);
                    }
                }
                finally
                {
                    manager.closeConnection();
                }
                return RedirectToAction("Details", "Users", new { id = model.Utente.userId });
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

        public ActionResult Delete(long id = 0)
        {
            MyUser myuser = null;

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

        private void populateComboBoxOLD(MyUser myuser)
        {
            GroupManager m = new GroupManager(manager.getConnection());
            CustomerManager customerManager = new CustomerManager(manager.getConnection());

            m.openConnection();

            List<MyGroup> listaGruppi;
            List<MyCustomer> listaClienti;
            List<MyProfile> listaProfili;

            try
            {
                listaGruppi = m.getList();
                listaClienti = customerManager.getList();
                listaProfili = manager.getProfili();
            }
            finally
            {
                m.closeConnection();
            }


            //MultiSelectList sl = new MultiSelectList(listaGruppi.ToList(), "gruppoId", "nome");
            //ViewBag.listaGruppi = sl;
            //ViewBag.Gruppi = listaGruppi.ToList();
            //ViewBag.listaClienti = new SelectList(listaClienti.ToList(), "customerId", "ragioneSociale");

            if (myuser == null)
            {
                ViewBag.listaClienti = listaClienti.OrderBy(p => p.ragioneSociale).Select(p => new SelectListItem { Selected = false, Text = p.ragioneSociale, Value = p.customerId.ToString() });
                //ViewBag.customerId = new SelectList(listaClienti, "customerId", "ragioneSociale");
                ViewBag.Gruppi = new MultiSelectList(listaGruppi, "gruppoId", "nome");
            }
            else
            {
                ViewBag.listaClienti = listaClienti.OrderBy(p => p.ragioneSociale).Select(p => new SelectListItem { Selected = (p.customerId == myuser.customerId), Text = p.ragioneSociale, Value = p.customerId.ToString() });
                //ViewBag.customerId = new SelectList(db.Clienti, "customerId", "ragioneSociale", myuser.customerId);

                MultiSelectList msl;
                if (myuser.Gruppi != null)
                {
                    msl = new MultiSelectList(listaGruppi, "gruppoId", "nome", myuser.Gruppi.Select(x => x.gruppoId).ToArray());
                }
                else
                {
                    msl = new MultiSelectList(listaGruppi, "gruppoId", "nome", null);
                }

                ViewBag.Gruppi = msl;

                // ViewBag.Gruppi = new MultiSelectList(listaGruppi, "gruppoId", "nome", myuser.Gruppi);
            }
            ViewBag.listaProfili = new SelectList(listaProfili.ToList(), "profileId", "nome");

        }


        public ActionResult CSV()
        {

            byte[] content;

            manager.openConnection();
            try
            {
                content = manager.exportToCSV();
            }
            finally
            {
                manager.closeConnection();
            }


            return File(content, "text/csv", String.Format("Export_users_{0}.csv", DateTime.Now.ToString("yyyy-MM-dd")));
        }


        public ActionResult SID(string sid)
        {

            System.Security.Principal.SecurityIdentifier sidObj;
            sidObj = new System.Security.Principal.SecurityIdentifier(sid);

            MyUser user = null; ;
            try
            {
                ActiveDirectoryManager manager = new ActiveDirectoryManager();
                user = manager.getUser(sidObj);
            }
            catch (Exception ex)
            {
                //http://rachelappel.com/when-to-use-viewbag-viewdata-or-tempdata-in-asp.net-mvc-3-applications
                TempData["MyError"] = ex.Message;
                return RedirectToAction("Error", "Admin");
            }


            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddGroup(long? userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("userId");
            }

            Debug.WriteLine("UserId: " + userId);
            Debug.WriteLine("gruppiIDs: " + Request["gruppiIDs"]);

            manager.openConnection();
            try
            {

                List<MyGroup> lista = new List<MyGroup>();

                foreach (string id in Request["gruppiIDs"].Split(','))
                {
                    Debug.WriteLine("GruppoId: " + id);
                    lista.Add(new MyGroup(int.Parse(id)));
                }

                GroupManager groupManager = new GroupManager(manager.getConnection());
                groupManager.update(lista, (long)userId);

                //List<MyGroup> lista = new List<MyGroup>();

                //foreach (string id in gruppiSelected.Split(','))
                //{
                //    lista.Add(new MyGroup(int.Parse(id)));
                //}

                //model.Utente.Gruppi = lista;

                //GroupManager groupManager = new GroupManager(manager.getConnection());
                //groupManager.update(model.Utente.Gruppi, model.Utente.userId);

            }
            finally
            {
                manager.closeConnection();
            }


            // return RedirectToAction("Details", new { id = userId} , ) ;

            return new RedirectResult(Url.Action("Details", new { id = userId }) + "#tabs-3");
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



        #region "__REPORTS___"


        public ActionResult ReportLogin(MyWebApplication.Areas.Admin.Models.ReportsModel model)
        {

            Debug.WriteLine(String.Format("Width: {0} Heigh: {1}", Request.Browser.ScreenPixelsWidth, Request.Browser.ScreenPixelsHeight));


            MyManagerCSharp.RGraph.Models.RGraphModel report;

            MyUsers.Reports.ReportsUsers reportManager = new MyUsers.Reports.ReportsUsers("utenti");
            try
            {
                reportManager.openConnection();

                report = reportManager.getLoginByDate("report01", model.Days);
                report.Width = "700px";
                report.Height = "400px";
                model.Reports.Add(report);


                report = reportManager.getLoginTopByUser("report02", 10, model.Days, MyManagerCSharp.Log.LogUserManager.LogType.Login);
                report.Width = "700px";
                report.Height = "400px";
                model.Reports.Add(report);


                report = reportManager.getLoginTopByUser("report03", 10, model.Days, MyManagerCSharp.Log.LogUserManager.LogType.LoginMobile);
                report.Width = "700px";
                report.Height = "400px";
                model.Reports.Add(report);

            }
            finally
            {
                reportManager.closeConnection();
            }

            return View(model);
        }


        public ActionResult ReportLastLogin(MyWebApplication.Areas.Admin.Models.ReportsModel model)
        {

            Debug.WriteLine(String.Format("Width: {0} Heigh: {1}", Request.Browser.ScreenPixelsWidth, Request.Browser.ScreenPixelsHeight));


            MyUsers.Reports.ReportsUsers reportManager = new MyUsers.Reports.ReportsUsers("utenti");
            try
            {
                reportManager.openConnection();


                model.Table = reportManager.getLastLogin(model.Days);

            }
            finally
            {
                reportManager.closeConnection();
            }

            return View(model);
        }

        #endregion


    }
}