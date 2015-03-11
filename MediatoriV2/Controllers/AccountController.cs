using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using mediatori.Filters;
using mediatori.Models.Anagrafiche;
using System.Data.SqlClient;
using System.Diagnostics;
using mediatori.Models;

namespace mediatori.Controllers
{

    public class AccountController : MyBaseController
    {
        public const bool MY_CUSTOM_IDENTITY = true;

        private MyUsers.UserManager manager = null;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (db != null)
            {
                manager = new MyUsers.UserManager(db.Database.Connection);
            }
        }



        [Authorize]
        public ActionResult ChangePassword()
        {
            ChangePasswordModel model = new ChangePasswordModel();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("", "Login o email errati");
                var message = string.Join(" | ", ModelState.Values
                 .SelectMany(v => v.Errors)
                 .Select(e => e.ErrorMessage));
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile modificare la password, verificare i dati: " + Environment.NewLine + message);
           
                return View(model);
            }


            manager.openConnection();

            try
            {
                long test;
                test = manager.isAuthenticated(User.Identity.Name , model.OldPassword);

                if (test != MySessionData.UserId)
                {
                    //ModelState.AddModelError("", "Verificare la password inserita");
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Verificare la password inserita");
                    return View(model);
                }
                
                manager.updatePassword(MySessionData.UserId, model.NewPassword);
            }
            catch (MyManagerCSharp.MyException ex)
            {
                if (ex.ErrorCode == MyManagerCSharp.MyException.ErrorNumber.LoginPasswordErrati)
                {
                   // ModelState.AddModelError("", "Verificare la password inserita");
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Verificare la password inserita");
                }
                else
                {
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, ex.Message);
//                    ModelState.AddModelError("", ex.Message);
                }


                return View(model);
            }
            finally
            {
                manager.closeConnection();
            }

            TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, "La password è stata aggiornata correttamente");

            return RedirectToAction("Manage");
        }

        public ActionResult ChangeEmail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreditoLab(mediatori.Models.ManageModel model)
        {
            //Verifico le credenziali
            string temp;
            BusinessModel.SimulazioneFinanziaria.SimulazioneManager manager = new BusinessModel.SimulazioneFinanziaria.SimulazioneManager(null, MySessionData.Istituto.url, model.Login, model.Password);

            temp = manager.Login(model.Login, model.Password);

            Debug.WriteLine(temp);

            if (!String.IsNullOrEmpty(temp))
            {
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, temp);
                return View("Manage", model);
            }


            BusinessModel.Configurazione.IstitutoManager istitutoManager = new BusinessModel.Configurazione.IstitutoManager(db.Database.Connection);

            try
            {
                istitutoManager.openConnection();

                if (MyConstants.CREDITOLAB_ENABLED)
                {
                    istitutoManager.insertCredenziali(new MyUsers.Models.MyCredenziali(model.Login, model.Password), MySessionData.UserId, MySessionData.Istituto.id);
                    _initSessionData(MySessionData);

                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, "Account CreditoLab configurato con successo");
                }
                else
                {
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Attenzione la configurazione verso CreditoLab è disabilitata");
                }

            }
            finally
            {
                istitutoManager.closeConnection();
            }


            return RedirectToAction("Manage");
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreditoLabDelete(mediatori.Models.ManageModel model)
        {
            BusinessModel.Configurazione.IstitutoManager istitutoManager = new BusinessModel.Configurazione.IstitutoManager(db.Database.Connection);

            try
            {
                istitutoManager.openConnection();

                istitutoManager.deleteCredenziali(MySessionData.UserId, MySessionData.Istituto.id);

                _initSessionData(MySessionData);

                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, "Account CreditoLab elimanto con successo");
            }
            finally
            {
                istitutoManager.closeConnection();
            }

            return RedirectToAction("Manage");
        }






        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            Models.LogOnModel model = new Models.LogOnModel();
            model.Password = "";
            return View(model);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Models.LogOnModel model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            //  if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            if (ModelState.IsValid)
            {
                string messaggioDiErrore;
                long userId;


                bool esito;
                esito = MyManagerCSharp.RegularExpressionManager.isValidEmail(model.UserName);
                if (esito == false)
                {
                    string messaggio;
                    messaggio = "Inserire un indirizzo email valido.";
                    //  TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, messaggio);
                    ModelState.AddModelError("", messaggio);
                    return View(model);
                }

                string dominio;
                dominio = model.UserName.Split('@')[1];

                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                connectionString = connectionString.Replace("database=mediatori", String.Format("database={0}", dominio));
                //"Data Source=tcp:khzappcv1w.database.windows.net,1433;Initial Catalog=mediatori;User ID=mediatori@khzappcv1w;Password=M3d14t0r1"
                connectionString = connectionString.Replace("initial catalog=mediatori", String.Format("initial catalog={0}", dominio));
                connectionString = connectionString.Replace("Initial Catalog=mediatori", String.Format("initial catalog={0}", dominio));

                if (manager == null)
                {
                    manager = new MyUsers.UserManager("DefaultConnection");
                }

                manager.changeConnectionString(connectionString);

                try
                {
                    manager.openConnection();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception: " + ex.Message);
                    string messaggio;
                    messaggio = "Dominio non riconosciuto. Verificare l'indirizzo email e se l'errore persiste, contattare l'amministratore di sistema.";
                    //TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, messaggio);
                    ModelState.AddModelError("", messaggio);
                    return View(model);
                }



                //   manager.openConnection();

                try
                {
                    string ip = HttpContext.Request.UserHostAddress;
                    userId = manager.isAuthenticated(model.UserName.Trim(), model.Password.Trim(), ip);

                    if (userId != -1)
                    {

                        if (MY_CUSTOM_IDENTITY == true)
                        {
                            string userDataString;
                            userDataString = userId + ";" + model.UserName.Trim() + ";";
                            HttpCookie authCookie = FormsAuthentication.GetAuthCookie(model.UserName, model.RememberMe);
                            //Get the FormsAuthenticationTicket out of the encrypted cookie
                            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                            //Create a new FormsAuthenticationTicket that includes our custom User Data
                            FormsAuthenticationTicket newTicket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration, ticket.IsPersistent, userDataString);

                            //Update the authCookie's Value to use the encrypted version of newTicket
                            authCookie.Value = FormsAuthentication.Encrypt(newTicket);

                            //Manually add the authCookie to the Cookies collection
                            Response.Cookies.Add(authCookie);
                        }
                        else
                        {
                            FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                        }

                        MyManagerCSharp.Log.LogUserManager log = new MyManagerCSharp.Log.LogUserManager(manager.getConnection());

                        if (TempData["AREA"] != null && TempData["AREA"].ToString() == "Mobile")
                        {
                            log.insert(userId, MyManagerCSharp.Log.LogUserManager.LogType.LoginMobile, System.Net.IPAddress.Parse(ip));
                        }
                        else
                        {
                            log.insert(userId, MyManagerCSharp.Log.LogUserManager.LogType.Login, System.Net.IPAddress.Parse(ip));
                        }

                        mediatori.SessionData session = new mediatori.SessionData(userId, dominio, connectionString);

                        /*** SESSIONE ***/
                        _initSessionData(session);

                        Session["MySessionData"] = session;

                        string temp;
                        temp = FormsAuthentication.GetRedirectUrl(model.UserName, model.RememberMe);

                        Debug.WriteLine("FormsAuthentication.GetRedirectUrl " + temp);
                    }
                }
                catch (MyManagerCSharp.MyException ex)
                {
                    if (ex.ErrorCode == MyManagerCSharp.MyException.ErrorNumber.LoginPasswordErrati)
                    {
                        messaggioDiErrore = ex.Message;
                    }
                    else if (ex.ErrorCode == MyManagerCSharp.MyException.ErrorNumber.UtenteDisabilitato)
                    {
                        messaggioDiErrore = ex.Message;
                        MyManagerCSharp.MailManager.send(ex);
                    }
                    else
                    {
                        //errore non gestito!!
                        messaggioDiErrore = "Errore durante la procedura di login. Contattare l'amministratore di sistema.";
                        MyManagerCSharp.MailManager.send(ex);
                    }

                    ModelState.AddModelError("", messaggioDiErrore);
                    return View(model);
                }
                finally
                {
                    manager.closeConnection();
                }

                //  if (Url.IsLocalUrl(returnUrl))
                if (IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    if (TempData["AREA"] == "Mobile")
                    {
                        return RedirectToAction("Index", "Mobile", new { area = "Mobile" });
                    }

                    if (TempData["AREA"] == "Admin")
                    {
                        return RedirectToAction("Index", "Admin", new { area = "Admin" });
                    }

                    return RedirectToAction("Index", "Home");
                }
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);

        }



        private void _initSessionData(mediatori.SessionData session)
        {
            long userId = session.UserId;


            session.Roles = manager.getRoles(userId);
            session.Profili = manager.getProfili(userId);
            session.Groups = manager.getGroupSmall(userId);

            /*** ISTITUTO ***/
            BusinessModel.Configurazione.IstitutoManager managerIstituto = new BusinessModel.Configurazione.IstitutoManager(manager.getConnection());
            session.Istituto = managerIstituto.getIstitutoByUserId(userId);
            if (session.Istituto != null)
            {
                session.CredenzialiCreditoLab = managerIstituto.getCredenziali(userId, session.Istituto.id);
            }

            //return session;
        }


        public ActionResult Manage()
        {
            mediatori.Models.ManageModel model = new mediatori.Models.ManageModel();
            manager.openConnection();

            long userId = -1;

            try
            {
                if (User.Identity is System.Security.Principal.WindowsIdentity)
                {
                    userId = manager.getUserIdFromSID(new System.Security.Principal.SecurityIdentifier((User.Identity as System.Security.Principal.WindowsIdentity).User.Value));
                }
                else if (User.Identity is MyUsers.MyCustomIdentity)
                {
                    userId = (User.Identity as MyUsers.MyCustomIdentity).UserId;
                }
                else if (User.Identity is System.Web.Security.FormsIdentity)
                {
                    userId = (Session["MySessionData"] as MyManagerCSharp.MySessionData).UserId;
                }

                //Non carico nulla perchè visualizzo solo i dati che sono in SESSIONE
                //setUserProfileModel(model, userId);

                MyUsers.Models.MyUser utente = manager.getUser(userId);
                if (utente != null)
                {
                    model.Nome = utente.nome;
                    model.Cognome = utente.cognome;

                    model.datePreviousLogin = utente.datePreviousLogin;
                }

            }
            finally
            {
                manager.closeConnection();
            }

            return View(model);
        }





        public ActionResult Refresh()
        {
            Debug.WriteLine("Area: " + Request.RequestContext.RouteData.DataTokens["area"]);

            if (Session["MySessionData"] == null)
            {
                return HttpNotFound();
            }


            long userId = -1;

            try
            {
                manager.openConnection();

                if (User.Identity is System.Security.Principal.WindowsIdentity)
                {
                    userId = manager.getUserIdFromSID(new System.Security.Principal.SecurityIdentifier((User.Identity as System.Security.Principal.WindowsIdentity).User.Value));
                }
                else if (User.Identity is MyUsers.MyCustomIdentity)
                {
                    userId = (User.Identity as MyUsers.MyCustomIdentity).UserId;
                }
                else if (User.Identity is System.Web.Security.FormsIdentity)
                {
                    userId = (Session["MySessionData"] as MyManagerCSharp.MySessionData).UserId;
                }

                //MyManagerCSharp.MySessionData sessionData = (Session["MySessionData"] as MyManagerCSharp.MySessionData);

                //if ((User.Identity.IsAuthenticated) && (User.Identity is MyUsers.MyCustomIdentity) && sessionData.UserId == -1)
                //{
                //    userId = (User.Identity as MyUsers.MyCustomIdentity).UserId;
                //}

                /*** SESSIONE ***/
                _initSessionData(MySessionData);

                //sessionData.Roles = manager.getRoles(userId);
                //sessionData.Profili = manager.getProfili(userId);
                //sessionData.Groups = manager.getGroupSmall(userId);
            }
            finally
            {
                manager.closeConnection();
            }



            if (TempData["AREA"] != null && TempData["AREA"].ToString() == "Mobile")
            {
                return RedirectToAction("Manage", new { area = "Mobile" });
            }

            return RedirectToAction("Manage");
        }


        //
        // GET: /Account/Register

        //Authorize(Roles = "Amministratore")]
        //public ActionResult Register()
        //{
        //    List<SelectListItem> selItems = new List<SelectListItem>();
        //    foreach (String ruolo in Roles.Provider.GetAllRoles())
        //    {
        //        selItems.Add(new SelectListItem() { Value = ruolo, Text = ruolo });
        //    }
        //    ViewBag.roles = selItems;
        //    return View();
        //}

        // [HttpGet]
        ////Authorize(Roles = "Amministratore")]
        // public ActionResult ListUsers(String message)
        // {
        //     if (message != null) ViewBag.message = message;
        //     List<String> users = db.Database.SqlQuery<String>("select UserName from dbo.UserProfile").ToList();
        //     ViewBag.utenti = users;
        //     return View();
        // }
        //
        // POST: /Account/Register

        //[HttpPost]
        ////[Authorize(Roles = "Amministratore")]
        //[ValidateAntiForgeryToken]
        //public ActionResult Register(RegisterModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Tentare di registrare l'utente
        //        try
        //        {

        //            WebSecurity.CreateUserAndAccount(model.UserName, model.Password);

        //            //  WebSecurity.Login(model.UserName, model.Password);
        //            SimpleRoleProvider srp = (SimpleRoleProvider)Roles.Provider;
        //            String[] usernames = { model.UserName };
        //            srp.AddUsersToRoles(usernames, model.roles);
        //            String message = "utente " + model.UserName + " registrato con successo!";
        //            return RedirectToAction("ListUsers", "Account", new { message = message });
        //        }
        //        catch (MembershipCreateUserException e)
        //        {
        //            ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
        //            List<SelectListItem> selItems = new List<SelectListItem>();
        //            foreach (String ruolo in Roles.Provider.GetAllRoles())
        //            {
        //                selItems.Add(new SelectListItem() { Value = ruolo, Text = ruolo });
        //            }
        //            ViewBag.roles = selItems;
        //        }
        //    }


        //    // Se si arriva a questo punto, significa che si è verificato un errore, rivisualizzare il form
        //    return View(model);
        //}


        //
        // GET: /Account/Disassociate





        [AllowAnonymous]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            MyManagerCSharp.Log.LogUserManager log = new MyManagerCSharp.Log.LogUserManager(db.Database.Connection);
            log.openConnection();
            try
            {
                string ip = Request.UserHostAddress;
                log.insert((Session["MySessionData"] as MyManagerCSharp.MySessionData).UserId, MyManagerCSharp.Log.LogUserManager.LogType.Logout, ip);
            }
            catch (Exception ex)
            {
                //potrebbe esserci un errore in quanto in fase di sviluppo si memorizza un id di sessione diverso
                //il problema si presenta ad esemoio se si cambia il puntamento al DB

                //ignoro l'errore
                Debug.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                log.closeConnection();
            }


            if (Session["MySessionData"] != null)
            {
                (Session["MySessionData"] as MyManagerCSharp.MySessionData).LogOff();
            }


            if (TempData["AREA"] == "Mobile")
            {
                return RedirectToAction("Index", "Mobile", new { area = "Mobile" });
            }

            if (TempData["AREA"] == "Admin")
            {
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }


            return RedirectToAction("Index", "Home");
        }


        //
        // GET: /Account/Manage
        //[AllowAnonymous]
        //public ActionResult Manage(string user, ManageMessageId? message)
        //{
        //    ViewBag.StatusMessage =
        //        message == ManageMessageId.ChangePasswordSuccess ? "Cambiamento password completato."
        //        : message == ManageMessageId.SetPasswordSuccess ? "Impostazione password completata."
        //        : message == ManageMessageId.RemoveLoginSuccess ? "L'account di accesso esterno è stato rimosso."
        //        : "";
        //    string currentUser = user;
        //    if (currentUser == null) currentUser = User.Identity.Name;
        //    ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(currentUser));
        //    ViewBag.ReturnUrl = Url.Action("Manage");
        //    LocalPasswordModel lpm = new LocalPasswordModel { user = currentUser, NewPassword = "pwd" };
        //    return View(lpm);
        //}

        //
        // POST: /Account/Manage

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[AllowAnonymous]
        //public ActionResult Manage(LocalPasswordModel model)
        //{
        //    if (model.user == null)
        //    {
        //        return View(model);
        //    }
        //    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(model.user));
        //    ViewBag.HasLocalPassword = hasLocalAccount;
        //    ViewBag.ReturnUrl = Url.Action("Manage");
        //    if (hasLocalAccount)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            // ChangePassword genererà un'eccezione anziché restituire false in alcuni scenari di errore.
        //            bool changePasswordSucceeded;
        //            try
        //            {
        //                changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
        //            }
        //            catch (Exception)
        //            {
        //                changePasswordSucceeded = false;
        //            }

        //            if (changePasswordSucceeded)
        //            {
        //                return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", "La password corrente non è corretta o la nuova password non è valida.");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        // L'utente non dispone di una password locale. Rimuovere quindi gli eventuali errori di convalida dovuti a un
        //        // campo OldPassword mancante
        //        ModelState state = ModelState["OldPassword"];
        //        if (state != null)
        //        {
        //            state.Errors.Clear();
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            try
        //            {
        //                WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
        //                return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
        //            }
        //            catch (Exception e)
        //            {
        //                ModelState.AddModelError("", e);
        //            }
        //        }
        //    }

        //    // Se si arriva a questo punto, significa che si è verificato un errore, rivisualizzare il form
        //    return View(model);
        //}

        //Get /Account/GestisciRuoli gestione ruoli utente. punto di partenza la schermata ListUsers
        //     nella schermata corrente viene permesso di aggiungere o rimuovere ruoli per uno specifico utente

        //
        // POST: /Account/ExternalLogin
        /*
                [HttpPost]
                [AllowAnonymous]
                [ValidateAntiForgeryToken]
                public ActionResult ExternalLogin(string provider, string returnUrl)
                {
                    return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
                }

                //
                // GET: /Account/ExternalLoginCallback

                [AllowAnonymous]
                public ActionResult ExternalLoginCallback(string returnUrl)
                {
                    AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
                    if (!result.IsSuccessful)
                    {
                        return RedirectToAction("ExternalLoginFailure");
                    }

                    if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
                    {
                        return RedirectToLocal(returnUrl);
                    }

                    if (User.Identity.IsAuthenticated)
                    {
                        // Se l'utente corrente ha eseguito l'accesso, aggiungere il nuovo account
                        OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        // L'utente è nuovo, chiedere di specificare il nome di appartenenza desiderato
                        string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                        ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                        ViewBag.ReturnUrl = returnUrl;
                        return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
                    }
                }

                //
                // POST: /Account/ExternalLoginConfirmation

                [HttpPost]
                [AllowAnonymous]
                [ValidateAntiForgeryToken]
                public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
                {
                    string provider = null;
                    string providerUserId = null;

                    if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
                    {
                        return RedirectToAction("Manage");
                    }

                    if (ModelState.IsValid)
                    {
                        // Inserisce un nuovo utente nel database
                        using (MainDbContext db = new MainDbContext())
                        {
                            UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                            // Verifica se l'utente esiste già
                            if (user == null)
                            {
                                // Inserire il nome nella tabella dei profili
                                db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                                db.SaveChanges();

                                OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                                OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                                return RedirectToLocal(returnUrl);
                            }
                            else
                            {
                                ModelState.AddModelError("UserName", "Il nome utente esiste già. Immettere un nome utente differente.");
                            }
                        }
                    }

                    ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
                    ViewBag.ReturnUrl = returnUrl;
                    return View(model);
                }

                //
                // GET: /Account/ExternalLoginFailure

                [AllowAnonymous]
                public ActionResult ExternalLoginFailure()
                {
                    return View();
                }

                [AllowAnonymous]
                [ChildActionOnly]
                public ActionResult ExternalLoginsList(string returnUrl)
                {
                    ViewBag.ReturnUrl = returnUrl;
                    return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
                }

                [ChildActionOnly]
                public ActionResult RemoveExternalLogins()
                {
                    ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
                    List<ExternalLogin> externalLogins = new List<ExternalLogin>();
                    foreach (OAuthAccount account in accounts)
                    {
                        AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                        externalLogins.Add(new ExternalLogin
                        {
                            Provider = account.Provider,
                            ProviderDisplayName = clientData.DisplayName,
                            ProviderUserId = account.ProviderUserId,
                        });
                    }

                    ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    return PartialView("_RemoveExternalLoginsPartial", externalLogins);
                }
                */



        private bool IsLocalUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }
            else
            {
                return ((url[0] == '/' && (url.Length == 1 ||
                        (url[1] != '/' && url[1] != '\\'))) ||   // "/" or "/foo" but not "//" or "/\"
                        (url.Length > 1 &&
                         url[0] == '~' && url[1] == '/'));   // "~/" or "~/foo"
            }
        }
        #region Helper
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }



        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // Vedere http://go.microsoft.com/fwlink/?LinkID=177550 per
            // un elenco completo di codici di stato.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Il nome utente esiste già. Immettere un nome utente differente.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Un nome utente per l'indirizzo di posta elettronica esiste già. Immettere un nome utente differente.";

                case MembershipCreateStatus.InvalidPassword:
                    return "La password fornita non è valida. Immettere un valore valido per la password.";

                case MembershipCreateStatus.InvalidEmail:
                    return "L'indirizzo di posta elettronica fornito non è valido. Controllare il valore e riprovare.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "La risposa fornita per il recupero della password non è valida. Controllare il valore e riprovare.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "La domanda fornita per il recupero della password non è valida. Controllare il valore e riprovare.";

                case MembershipCreateStatus.InvalidUserName:
                    return "Il nome utente fornito non è valido. Controllare il valore e riprovare.";

                case MembershipCreateStatus.ProviderError:
                    return "Il provider di autenticazione ha restituito un errore. Verificare l'immissione e riprovare. Se il problema persiste, contattare l'amministratore di sistema.";

                case MembershipCreateStatus.UserRejected:
                    return "La richiesta di creazione dell'utente è stata annullata. Verificare l'immissione e riprovare. Se il problema persiste, contattare l'amministratore di sistema.";

                default:
                    return "Si è verificato un errore sconosciuto. Verificare l'immissione e riprovare. Se il problema persiste, contattare l'amministratore di sistema.";
            }
        }
        #endregion
    }
}
