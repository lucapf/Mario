using mediatori.Controllers.Business;
using mediatori.Filters;
using mediatori.Models.Anagrafiche;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace mediatori.Models
{
    public class PreventivoController : Controller
    {
        //
        // GET: /Preventivo/
        
        public ActionResult Index(int id, EnumTipoAzione tipoAzione)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            Preventivo preventivo = new Preventivo();
            preventivo.finanziaria = new SoggettoGiuridico();
            preventivo.assicurazioneVita = new SoggettoGiuridico();
            preventivo.assicurazioneImpiego = new SoggettoGiuridico();
            return dispatch(preventivo, tipoAzione, db);
        }
        public String  confermaPreventivo(int codicePreventivo, Boolean statoConferma = true)
        {
             MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
             Preventivo p = db.preventivi.Find(codicePreventivo);
             if (statoConferma == true)
             {
                 p.dataConferma = System.DateTime.Now;
                 p.operatoreConferma = User.Identity.Name;
             }
             else {
                 p.dataConferma = null;
                 p.operatoreConferma = null;
             }
             db.SaveChanges();
             return JsonConvert.SerializeObject(p);
        }
        //public ActionResult preventivoPartial(Segnalazione segnalazione, EnumTipoAzione tipoAzione)
        //{
        //    MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
        //    Preventivo preventivo = new Preventivo();
        //    preventivo.importoRata = segnalazione.rataRichiesta;
        //    preventivo.durata = segnalazione.durataRichiesta;
        //    preventivo.montante = (decimal)(segnalazione.rataRichiesta * (float) segnalazione.durataRichiesta);
        //    return dispatch(preventivo, tipoAzione, db);
        //}
        public ActionResult preventivoPartial(Preventivo preventivo, EnumTipoAzione tipoAzione, Segnalazione segnalazione)
        {
            preventivo = preventivo == null ? new Preventivo() : preventivo;
            if (segnalazione != null)
            {
                preventivo.importoRata = segnalazione.rataRichiesta;
                preventivo.durata = segnalazione.durataRichiesta;
               // preventivo.montante = (decimal)(segnalazione.rataRichiesta * (float)segnalazione.durataRichiesta);
           
            }
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            return dispatch(preventivo, tipoAzione, db);
        }
        
        public ActionResult createForSegnalazione(Preventivo p, int idSegnalazione)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            Segnalazione s = new SegnalazioneBusiness().findByPk(idSegnalazione, db);
            if (s.preventivi == null)
            {
                s.preventivi = new List<Preventivo>();
            }

            p.id = 0;

            p.progressivo = s.preventivi.Count() + 1;
            s.preventivi.Add(p);
            int idAssicurazioneVita = p.assicurazioneVita.id;
            int idAssicurazioneImpiego= p.assicurazioneImpiego.id;
            int idFinanziari = p.finanziaria.id;
            p.assicurazioneVita = db.SoggettiGiuridici.Where(aa => aa.id == idAssicurazioneVita).FirstOrDefault();
            p.assicurazioneImpiego= db.SoggettiGiuridici.Where(aa => aa.id == idAssicurazioneImpiego).FirstOrDefault();
            p.finanziaria = db.SoggettiGiuridici.Where(aa => aa.id == idFinanziari).FirstOrDefault();            
            ModelState.Clear();
            TryValidateModel(p);
            if (ModelState.IsValid)
            {
                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();

                    foreach (var failure in ex.EntityValidationErrors)
                    {
                        sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                        foreach (var error in failure.ValidationErrors)
                        {
                            sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                            sb.AppendLine();
                        }
                    }
                    Debug.WriteLine("EntityValidationErrors:" + sb.ToString());

                }
                ViewBag.message = "Preventivo salvato con successo";
                return RedirectToAction("Details", "GestioneSegnalazioni", idSegnalazione);
            }
            else
            {
                ViewBag.erroMessage = "Impossibile salvare il preventivo, verificare i dati";
                return View(p);
            }


        }
        private void valorizzaViewBag(MainDbContext db)
        {
            SoggettoGiuridicoBusiness sgb = new SoggettoGiuridicoBusiness();
            SoggettoGiuridicoSearch sgs = new SoggettoGiuridicoSearch();
            sgs.tipoSoggettoGiuridico = EnumTipoSoggettoGiuridico.FINANZIARIA.ToString();
            ViewBag.listaFinanziarie = new SelectList(sgb.findByFilter(sgs, db), "id", "ragioneSociale");
            sgs.tipoSoggettoGiuridico = EnumTipoSoggettoGiuridico.ASSICURAZIONE.ToString();
            ViewBag.listaCompagnieAssicurative = new SelectList(sgb.findByFilter(sgs, db), "id", "ragioneSociale");
        }
        private ActionResult dispatch(Preventivo p, EnumTipoAzione tipoAzione, MainDbContext db)
        {
            switch (tipoAzione)
            {
                case EnumTipoAzione.INSERIMENTO:
                case EnumTipoAzione.MODIFICA:
                    valorizzaViewBag(db);
                    return View("PreventivoPartialEdit", p);
                case EnumTipoAzione.VISUALIZZAZIONE:
                    return View("PreventivoPartialDetail", p);
            }
            return View("PreventivoPartialDetail", p);
        }

    }
}
