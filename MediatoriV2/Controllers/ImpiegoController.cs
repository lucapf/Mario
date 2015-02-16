using BusinessModel.Log;
using mediatori.Controllers.Business;
using mediatori.Controllers.Business.Anagrafiche.Soggetto;
using mediatori.Models;
using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mediatori.Controllers
{
    public class ImpiegoController : MyBaseController
    {

        public ActionResult Index()
        {
            valorizzaViewBag();
            return View(new ImpiegoBusiness());
        }


        [ChildActionOnly]
        public ActionResult Details(int contattoId, bool edit = true)
        {
            Contatto contatto;
            contatto = db.Contatti.Include("impieghi").Where(p => p.id == contattoId).First();
            if (contatto == null)
            {
                return HttpNotFound();
            }

            ImpieghiModel model = new ImpieghiModel();
            model.impieghi = new List<Impiego>(); 

            foreach (Impiego i in contatto.impieghi)
            {
                //aggiungo il soggetto giuridico
                i.amministrazione.soggettoGiuridico = db.SoggettiGiuridici.Find(i.amministrazione.soggettoGiuridicoId);
                model.impieghi.Add(i);
            }

            //model.impieghi = contatto.impieghi.ToList<Impiego>();
            model.contattoId = contattoId;
                     
            if (edit == true)
            {
                valorizzaViewBag();
                return View("_Impieghi", model);
            }

            return View("_ImpieghiView", model);
        }



        [HttpGet]
        public ActionResult ImpiegoPartialById(int id, EnumTipoAzione tipoAzione = EnumTipoAzione.VISUALIZZAZIONE)
        {
            Impiego impiego;
            impiego = (from i in db.Impieghi.Include("tipoImpiego").Include("categoriaImpiego") where i.id == id select i).First();

            if (impiego == null)
            {
                return HttpNotFound();
            }

            impiego.amministrazione.soggettoGiuridico = db.SoggettiGiuridici.Find(impiego.amministrazione.soggettoGiuridicoId);

            if (tipoAzione == EnumTipoAzione.MODIFICA)
            {
                valorizzaViewBag();
                return View("ImpiegoEdit", impiego);
            }

            if (tipoAzione == EnumTipoAzione.VISUALIZZAZIONE)
            {
                return View("ImpiegoPartialDetail", impiego);
            }

            //  valorizzaViewBag();
            throw new ApplicationException("Azione di inserimento che non si deve presentare");
        }



        [ChildActionOnly]
        public ActionResult Create(Impiego impiego)
        {
            //  Impiego impiego = new Impiego();

#if DEBUG
            //impiego.azienda = "Azienda";
            //impiego.aziendaSedeLavoro = "Sede lavoro";
            ////impiego.dataAssunzione = new DateTime(2000, 8, 1);
            //impiego.mansione = "Impiegato";
            //impiego.mensilita = 14;
            //impiego.stipendioLordoAnnuo = 20000;
            //impiego.stipendioLordoMensile = 1200;
            //impiego.stipendioNettoMensile = 900;
#endif


            valorizzaViewBag();

            ViewData.TemplateInfo.HtmlFieldPrefix = "impiego";


            return View("ImpiegoPartialEdit", impiego);
        }


        [ChildActionOnly]
        public ActionResult impiegoPartial(Impiego impiego, EnumTipoAzione tipoAzione = EnumTipoAzione.MODIFICA)
        {

            switch (tipoAzione)
            {
                case EnumTipoAzione.MODIFICA:
                    valorizzaViewBag();
                    return View("ImpiegoPartialEdit", impiego);
                case EnumTipoAzione.INSERIMENTO:
                    valorizzaViewBag();
                    return View("ImpiegoPartialInsert", impiego);
                default:
                    return View("ImpiegoPartialDetail", impiego);
            }
        }
        [HttpPost]
        public ActionResult Edit(Impiego impiego)
        {

            ImpiegoBusiness.valorizzaDatiImpiego(impiego, db);
            Impiego impiegoOriginale = (from i in db.Impieghi.Include("tipoImpiego") where i.id == impiego.id select i).First();

            LogEventi le = LogEventiManager.getEventoForUpdate(User.Identity.Name, impiego.id, EnumEntitaRiferimento.IMPIEGO, impiegoOriginale, impiego);
            impiegoOriginale = (Impiego)CopyObject.simpleCompy(impiegoOriginale, impiego);

            LogEventiManager.save(le, db);
            db.SaveChanges();

            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        //private Impiego completaDatiImpiegoFromRequest(Impiego impiego)
        //{
        //    // impiego.dataLicenziamento = impiego.dataLicenziamento.Year == 01 ? new DateTime(2050, 12, 31) : impiego.dataLicenziamento;

        //    impiego.tipoImpiego = db.TipoContrattoImpiego.Find(impiego.tipoImpiego.id);
        //    impiego.categoriaImpiego = db.TipoCategoriaImpiego.Find(impiego.categoriaImpiego.id);
        //    return impiego;
        //}



        [HttpPost]
        public ActionResult UpdateForContatto(Impiego impiego, int codiceContatto)
        {
            // impiego = completaDatiImpiegoFromRequest(impiego);
            ImpiegoBusiness.valorizzaDatiImpiego(impiego, db);
            impiego.contattoId = codiceContatto;
            //ModelState.Remove("tipoImpiego.descrizione");
            //ModelState.Remove("categoriaImpiego.descrizione");
            ModelState.Clear();
            TryValidateModel(impiego);


            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                  .SelectMany(v => v.Errors)
                  .Select(e => e.ErrorMessage));
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare l'impiego, verificare i dati: " + Environment.NewLine + message);
                return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
            }

            Contatto contatto = null;
            contatto = db.Contatti.Find(codiceContatto);

            if (contatto == null)
            {
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Codice contatto non trovato");

            }
            else
            {

                try
                {
                    // contatto.impieghi.Add(impiego);
                    // LogEventiManager.save(LogEventiManager.getEventoForCreate(User.Identity.Name, impiego.id, EnumEntitaRiferimento.IMPIEGO), db);
                    db.Impieghi.Attach(impiego);
                    db.Entry(impiego).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, "Impiego salvato con successo");
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    string messaggio;
                    messaggio = MyHelper.getDbEntityValidationException(ex);
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare l'impiego, verificare i dati: " + Environment.NewLine + messaggio);
                }
                catch (Exception ex)
                {
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare l'impiego, verificare i dati: " + Environment.NewLine + ex.Message);
                }
            }

            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        [HttpPost]
        public ActionResult CreateForContatto(Impiego impiego, int codiceContatto)
        {

            ImpiegoBusiness.valorizzaDatiImpiego(impiego, db);

            ModelState.Clear();
            TryValidateModel(impiego);


            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                  .SelectMany(v => v.Errors)
                  .Select(e => e.ErrorMessage));
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare l'impiego, verificare i dati: " + Environment.NewLine + message);
                return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
            }


            //Cedente cedente = RicercaCedenteBusiness.find(codiceCedente, db);
            //cedente.impieghi.Add(impiego);

            Contatto contatto = db.Contatti.Include("impieghi").Where(p => p.id == codiceContatto).First();
            if (contatto == null)
            {
                return HttpNotFound();
            }


            try
            {
                contatto.impieghi.Add(impiego);

                LogEventiManager.save(LogEventiManager.getEventoForCreate(User.Identity.Name, impiego.id, EnumEntitaRiferimento.IMPIEGO), db);
                db.SaveChanges();

                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, "Impiego salvato con successo");
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                string messaggio;
                messaggio = MyHelper.getDbEntityValidationException(ex);
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare l'impiego, verificare i dati: " + Environment.NewLine + messaggio);
            }
            catch (Exception ex)
            {
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare l'impiego, verificare i dati: " + Environment.NewLine + ex.Message);
            }
            // return View("Details","Segnalazioni", );
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        [HttpPost]
        public ActionResult CreateForSegnalazione(Impiego impiego, int codiceSegnalazione)
        {
            //  impiego = completaDatiImpiegoFromRequest(impiego);
            impiego = ImpiegoBusiness.valorizzaDatiImpiego(impiego, db);
            ModelState.Clear();
            TryValidateModel(impiego);
            if (ModelState.IsValid)
            {
                Segnalazione segnalazione = new SegnalazioneBusiness().findByPk(codiceSegnalazione, db);
                segnalazione.contatto.impieghi.Add(impiego);
                LogEventiManager.save(LogEventiManager.getEventoForCreate(User.Identity.Name, impiego.id, EnumEntitaRiferimento.IMPIEGO), db);
                db.SaveChanges();
            }
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        private void valorizzaViewBag()
        {
            ViewBag.listaTipoImpiego = new SelectList(db.TipoContrattoImpiego.OrderBy(p=> p.descrizione) , "id", "descrizione");
            ViewBag.listaCategoriaImpiego = new SelectList(db.TipoCategoriaImpiego.OrderBy(p=> p.descrizione), "id", "descrizione");

            ViewBag.listaAmministrazioni = new SelectList(db.Amministazioni.Include("soggettoGiuridico"), "id", "soggettoGiuridico.ragioneSociale"); 
            //ViewBag.oggi = System.DateTime.Now.Year;
            //ViewBag.inizioValiditaImpiego = System.DateTime.Now.AddYears(-40).Year;

        }
    }
}
