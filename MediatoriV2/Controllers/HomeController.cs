using mediatori.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;


namespace mediatori.Controllers
{
    public class HomeController : MyBaseController
    {
        public ActionResult Index()
        {
            List<MenuElement> model = new List<MenuElement>(){
                 // new MenuElement(){display="Home", ordinamento=1,livello=1,role="Amministratore",action="Index", controller="Home"},
                  new MenuElement(){display="Anagrafiche", ordinamento=1,livello=1,role="Amministratore",action="Anagrafiche", controller="Home"},
                  new MenuElement(){display="Sicurezza", ordinamento=1,livello=1,role="Amministratore",action="Sicurezza", controller="Home"},
                  new MenuElement(){display="Configurazioni", ordinamento=1,livello=1,role="Amministratore",action="Index", controller="Configurazioni"}
                };
            return View(model);
        }


        public ActionResult Whatsnew()
        {

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);


            ViewData["VersioneCorrente"] = fvi.FileVersion;
            return View();
        }


        public ActionResult Anagrafiche()
        {
            List<MenuElement> model = new List<MenuElement>(){
                  //new MenuElement(){display="Home", ordinamento=1,livello=1,role="Amministratore",action="Index", controller="Home"},
                  //new MenuElement(){display="Anagrafiche", ordinamento=1,livello=1,role="Amministratore",action="Anagrafiche", controller="Home"},
                  new MenuElement(){display="Agenzie", ordinamento=1,livello=1,role="Amministratore",action="Index",controller="Agenzia"},
                  new MenuElement(){display="Amministrazioni", ordinamento=1,livello=1,role="Amministratore",action="Index",controller="Amministrazione"},
                  new MenuElement(){display="Società", ordinamento=1,livello=1,role="Amministratore",action="Index",controller="SoggettoGiuridico"},
                  new MenuElement(){display="Cedenti", ordinamento=1,livello=1,role="Amministratore",action="Index",controller="Cedente"}
                };

            return View(model);
        }


        [MyAuthorize(Roles = new string[] { MyConstants.Profilo.AMMINISTRATORE })]
        public ActionResult Sicurezza()
        {
            List<MenuElement> model = new List<MenuElement>(){
                  //new MenuElement(){display="Home", ordinamento=1,livello=1,role="Amministratore",action="Index", controller="Home"},
                  //new MenuElement(){display="Sicurezza", ordinamento=1,livello=1,role="Amministratore",action="Sicurezza", controller="Home"},
                  new MenuElement(){display="Utenti", ordinamento=1,livello=1,role="Amministratore",action="Index", controller="Utenti"},
                  new MenuElement(){display="Gruppi", ordinamento=1,livello=1,role="Amministratore",action="Index",controller="Groups"}
                  //new MenuElement(){display="Nuovo utente", ordinamento=1,livello=1,role="Amministratore",action="Register",controller="Utenti"},
                  //new MenuElement(){display="Modifica password", ordinamento=1,livello=1,role="Amministratore",action="ChangePassword",controller="Account"},
                  //new MenuElement(){display="Logout", ordinamento=1,livello=1,role="Amministratore",action="LogOff",controller="Account"}
                };
            return View(model);
        }



        public ActionResult AccessDenied()
        {
            return View();
        }



        public ActionResult Calendar()
        {
            return View();
        }

        public ActionResult Assegnazioni(mediatori.Models.AssegnazioniModel model)
        {
            Debug.WriteLine("Assegnazioni");
            // model.DaAssegnare = db.Segnalazioni.Include("contatto").Include("prodottoRichiesto").ToList();

            //List<mediatori.Models.etc.GruppoLavorazione> gruppi = db.gruppiLavorazione.Where ( p => p.utenti.Contains (""User.Identity.Name

            //model.DaAssegnare = (from s in db.Segnalazioni.Include("stato").Include("contatto").Include("prodottoRichiesto")
            //                     where !(
            //                            from a in db.Assegnazioni where a.segnalazioneId == s.id && a.statoId == s.stato.id select a.segnalazioneId
            //                        ).Contains(s.id) && s.stato.gruppoLavorazione.utenti.Contains(";" + User.Identity.Name + ";")
            //                     select s).ToList();


            //Debug.WriteLine("GROUPS : " + (Session["MySessionData"] as MyManagerCSharp.MySessionData).Groups.Select(p => p.gruppoId == 8).First());

            //var test = (Session["MySessionData"] as MyManagerCSharp.MySessionData).Groups.Select(p => p.gruppoId == 8).First();


            List<long> listGruppiIds = (Session["MySessionData"] as MyManagerCSharp.MySessionData).Groups.Select(g => g.gruppoId).ToList();
            string temp = String.Join(";", listGruppiIds);
            temp = ";" + temp + ";";





            //model.DaAssegnare = (from s in db.Segnalazioni.Include("stato").Include("contatto").Include("prodottoRichiesto")
            //                     where !(
            //                            from a in db.Assegnazioni where a.segnalazioneId == s.id && a.statoId == s.stato.id select a.segnalazioneId
            //                        ).Contains(s.id) && temp.Contains(";" + s.stato.gruppoId + ";")
            //                     select s).ToList();


            IQueryable<mediatori.Models.Anagrafiche.Segnalazione> querySegnalazioni = (from s in db.Segnalazioni.Include("stato").Include("contatto").Include("prodottoRichiesto")
                                                                                       where !(
                                                                                              from a in db.Assegnazioni where a.segnalazioneId == s.id && a.statoId == s.stato.id select a.segnalazioneId
                                                                                          ).Contains(s.id) && temp.Contains(";" + s.stato.gruppoId + ";")
                                                                                       select s);


            //  Debug.WriteLine("Profilo: " + (Session["MySessionData"] as MyManagerCSharp.MySessionData).Profili);
            if ((Session["MySessionData"] as MyManagerCSharp.MySessionData).Profili.IndexOf(MyConstants.Profilo.COLLABORATORE.ToString()) > -1)
            {
                querySegnalazioni = querySegnalazioni.Where(p => p.utenteInserimento == User.Identity.Name);
            }


            model.DaAssegnare = querySegnalazioni.ToList();


            //  model.Assegnate = db.Assegnazioni.ToList();

            //model.Assegnate = (from s in db.Segnalazioni.Include("stato")
            //                   join a in db.Assegnazioni.Include("Segnalazione").Include("Segnalazione.contatto") on s.id equals a.segnalazioneId
            //                  where s.stato.id == a.statoId
            //                  select a).ToList();

            //model.Assegnate = (from a in db.Assegnazioni.Include("Segnalazione").Include("Segnalazione.contatto").Include("Segnalazione.stato").Include("Segnalazione.prodottoRichiesto")
            //                   where a.segnalazione.stato.id == a.statoId && a.stato.gruppoLavorazione.utenti.Contains(";" + User.Identity.Name + ";")
            //                   select a).ToList();


            IQueryable<mediatori.Models.etc.Assegnazione> queryAssegnazioni;
            queryAssegnazioni = (from a in db.Assegnazioni.Include("Segnalazione").Include("Segnalazione.contatto").Include("Segnalazione.stato").Include("Segnalazione.prodottoRichiesto")
                                 where a.segnalazione.stato.id == a.statoId && temp.Contains(";" + a.stato.gruppoId + ";")
                                 select a);


            model.Assegnate = queryAssegnazioni.ToList();
            // model.Assegnate = (from a in db.Assegnazioni.Include("Segnalazione").Include("Segnalazione.contatto").Include("Segnalazione.stato").Include("Segnalazione.prodottoRichiesto")
            //                    where a.segnalazione.stato.id == a.statoId && temp.Contains(";" + a.stato.gruppoId + ";")
            //                    select a).ToList();


            model.NumeroScadute = model.Assegnate.Where(p => (p.segnalazione.dataPromemoria != null && p.segnalazione.dataPromemoria < DateTime.Now)).Count();

            model.NumeroScadute = model.NumeroScadute + model.DaAssegnare.Where(p => (p.dataPromemoria != null && p.dataPromemoria < DateTime.Now)).Count();

            //model.NumeroScadute = model.DaAssegnare.Where(p => (p.dataPromemoria == null)).Count();
            //model.NumeroScadute = model.DaAssegnare.Where(p => (p.dataPromemoria != null)).Count();

            if (Request.IsAjaxRequest())
            {
                List<mediatori.Models.MyItem> risultato = new List<MyItem>();

                if (model.DaAssegnare == null)
                {
                    risultato.Add(new MyItem("0", "Da_assegnare"));
                }
                else
                {
                    risultato.Add(new MyItem(model.DaAssegnare.Count.ToString(), "Da_assegnare"));
                }



                if (model.Assegnate == null)
                {
                    risultato.Add(new MyItem("0", "Assegnate"));
                }
                else
                {
                    risultato.Add(new MyItem(model.Assegnate.Count.ToString(), "Assegnate"));
                }


                risultato.Add(new MyItem(model.NumeroScadute.ToString(), "Scadute"));

                return Json(risultato, JsonRequestBehavior.AllowGet);

            }

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Pagina di descrizione dell'app.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Pagina di contatto.";

            return View();
        }




        [HttpGet]
        public String popolaDropDownlistProvince()
        {
            return new mediatori.Controllers.Business.Anagrafiche.PopolaDropDownListAnagrafiche().popolaDropDownListProvince(db);
        }

        [HttpGet]
        public String popolaDropDownlistComuni(String comboComunElementId, String denominazioneProvincia)
        {
            return new mediatori.Controllers.Business.Anagrafiche.PopolaDropDownListAnagrafiche().popolaDropDownListComuniJSON(comboComunElementId, denominazioneProvincia, db);
        }
    }
}
