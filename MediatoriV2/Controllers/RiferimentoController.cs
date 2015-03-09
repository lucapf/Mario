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
    public class RiferimentoController : MyBaseController
    {


        //public ActionResult Index()
        //{
        //    valorizzaViewBag();
        //    return View(new Riferimento());
        //}


        [ChildActionOnly]
        public ActionResult Details(int contattoId, bool edit = true)
        {
            Contatto contatto;
            contatto = db.Contatti.Include("riferimenti").Include("riferimenti.tipoRiferimento").Where(p => p.id == contattoId).First();
            if (contatto == null)
            {
                return HttpNotFound();
            }

            RiferimentiModel model = new RiferimentiModel();
            model.riferimenti = contatto.riferimenti.ToList<Riferimento>();
            model.contattoId = contattoId;

            if (edit == true)
            {
                valorizzaViewBag();
                return View("_Riferimenti", model);
            }

            return View("_RiferimentiView", model);
        }



        [ChildActionOnly]
        public ActionResult SoggettoGiuridico(int soggettoGiuridicoId)
        {
            RiferimentiModel model = new RiferimentiModel();

            if (soggettoGiuridicoId != -1)
            {
                SoggettoGiuridico soggettoGiuridico;
                soggettoGiuridico = db.SoggettiGiuridici.Include("riferimenti").Include("riferimenti.tipoRiferimento").Where(p => p.id == soggettoGiuridicoId).First();
                if (soggettoGiuridico == null)
                {
                    return HttpNotFound();
                }

                model.riferimenti = soggettoGiuridico.riferimenti.ToList<Riferimento>();
                model.soggettoGiuridicoId = soggettoGiuridicoId;
            }
            else
            {
                //CREATE
                model.soggettoGiuridicoId = -1;
                model.riferimenti = new List<Riferimento>();
            }

            valorizzaViewBag();

            return View("_Riferimenti", model);
        }





        [ChildActionOnly]
        public ActionResult Create(Riferimento riferimento)
        {
#if DEBUG
            riferimento.valore = "Prova";
#endif
            valorizzaViewBag();
            ViewData.TemplateInfo.HtmlFieldPrefix = "riferimento";
            return View("RiferimentoPartialEdit", riferimento);
        }




        [HttpGet]
        public ActionResult RiferimentoPartialById(int id, EnumTipoAzione tipoAzione)
        {
            Riferimento riferimento = RiferimentoBusiness.findByPk(id, db);

            if (riferimento == null)
            {
                return HttpNotFound();
            }


            if (tipoAzione == EnumTipoAzione.MODIFICA)
            {
                valorizzaViewBag(riferimento);
                //  ViewData.TemplateInfo.HtmlFieldPrefix = "indirizzo";
                return View("RiferimentoEdit", riferimento);
            }

            if (tipoAzione == EnumTipoAzione.VISUALIZZAZIONE)
            {
                return View("RiferimentoPartialDetail", riferimento);
            }

            throw new ApplicationException("Azione di inserimento che non si deve presentare");
        }

        /*
        [ChildActionOnly]
        public ActionResult riferimentoPartial(Riferimento riferimento, EnumTipoAzione tipoAzione = EnumTipoAzione.MODIFICA)
        {
            return riferimentoDispatcher(riferimento, tipoAzione, db);
        }

        private ActionResult riferimentoDispatcher(Riferimento riferimento, EnumTipoAzione tipoAzione, MainDbContext db)
        {
            switch (tipoAzione)
            {
                case EnumTipoAzione.MODIFICA:
                    valorizzaViewBag();
                    return View("RiferimentoPartialEdit", riferimento);
                case EnumTipoAzione.INSERIMENTO:
                    valorizzaViewBag();
                    return View("RiferimentoPartialInsert", riferimento);
                default:
                    return View("RiferimentoPartialDetail", riferimento);
            }
        }*/

        private void valorizzaViewBag()
        {
            valorizzaViewBag(null);
        }

        private void valorizzaViewBag(Riferimento riferimento)
        {
            if (riferimento == null)
            {
                ViewBag.listaTipoRiferimento = new SelectList(db.TipoRiferimento, "id", "descrizione");
            }
            else
            {
                ViewBag.listaTipoRiferimento = new SelectList(db.TipoRiferimento, "id", "descrizione", riferimento.tipoRiferimento.id);
            }
        }



        [HttpPost]
        public ActionResult CreateForContatto(Riferimento riferimento, int codiceContatto)
        {
            Contatto contatto = db.Contatti.Include("riferimenti").Where(c => c.id == codiceContatto).First();
            if (contatto == null)
            {
                return HttpNotFound();
            }

            riferimento.tipoRiferimento = db.TipoRiferimento.Find(riferimento.tipoRiferimento.id);

            ModelState.Clear();
            TryValidateModel(riferimento);

            contatto.riferimenti.Add(riferimento);

            try
            {
                db.SaveChanges();
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, "Riferimento creato con successo");
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                string messaggio;
                messaggio = MyHelper.getDbEntityValidationException(ex);
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile creare un nuovo riferimento: " + Environment.NewLine + messaggio);
            }

            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        /*
              [HttpPost]
              public ActionResult CreateForSegnalazione(Riferimento riferimento, int codiceSegnalazione)
              {

                  riferimento = RiferimentoBusiness.valorizzaDatiRiferimento(riferimento, db);
                  Segnalazione s = new SegnalazioneBusiness().findByPk(codiceSegnalazione, db);
                  ModelState.Clear();
                  TryValidateModel(riferimento);
                  s.contatto.riferimenti.Add(riferimento);
                  db.SaveChanges();
                  return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
              }
         */
        [HttpPost]
        public ActionResult CreateForSoggettoGiuridico(Riferimento riferimento, int codiceSoggettoGiuridico)
        {
            riferimento = RiferimentoBusiness.valorizzaDatiRiferimento(riferimento, db);
            SoggettoGiuridico s = new SoggettoGiuridicoBusiness().findByPK(codiceSoggettoGiuridico, db);
            ModelState.Clear();
            TryValidateModel(riferimento);
            s.riferimenti.Add(riferimento);
            db.SaveChanges();
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        /*
        [HttpPost]
        public ActionResult Edit(Riferimento riferimento)
        {
            riferimento = RiferimentoBusiness.valorizzaDatiRiferimento(riferimento, db);
            ModelState.Clear();

            TryValidateModel(riferimento);
            if (ModelState.IsValid)
            {
                Riferimento riferimentoSalvato = RiferimentoBusiness.save(User.Identity.Name, riferimento, db);
            }

            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }*/


        /*
        [HttpPost]
        public ActionResult UpdateForSoggettoGiuridico(Riferimento riferimento, int codiceSggettoGiuridico)
        {
            RiferimentoBusiness.valorizzaDatiRiferimento(riferimento, db);
            riferimento.soggettoGiuridicoId = codiceSggettoGiuridico;

            ModelState.Clear();
            TryValidateModel(riferimento);


            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                  .SelectMany(v => v.Errors)
                  .Select(e => e.ErrorMessage));
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare il riferimento, verificare i dati: " + Environment.NewLine + message);
                return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
            }

            SoggettoGiuridico soggettoGiuridico = null;
            soggettoGiuridico = db.SoggettiGiuridici.Find(codiceSggettoGiuridico);

            if (soggettoGiuridico == null)
            {
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Soggetto giuridico non trovato");

            }
            else
            {

                try
                {
                    riferimento.soggettoGiuridico = soggettoGiuridico;

                    RiferimentoBusiness.save(User.Identity.Name, riferimento, db);

                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, "Riferimento salvato con successo");
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    string messaggio;
                    messaggio = MyHelper.getDbEntityValidationException(ex);
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare il riferimento, verificare i dati: " + Environment.NewLine + messaggio);
                }
                catch (Exception ex)
                {
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare l'indirizzo, verificare i dati: " + Environment.NewLine + ex.Message);
                }


            }

            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);

        }
        */
        /*

        [HttpPost]
        public ActionResult UpdateForContatto(Riferimento riferimento, int codiceContatto)
        {
            RiferimentoBusiness.valorizzaDatiRiferimento(riferimento, db);
            riferimento.contattoId = codiceContatto;

            ModelState.Clear();
            TryValidateModel(riferimento);


            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                  .SelectMany(v => v.Errors)
                  .Select(e => e.ErrorMessage));
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare il riferimento, verificare i dati: " + Environment.NewLine + message);
                return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
            }

            PersonaFisica persona = null;
            persona = db.Per.Find(codiceSggettoGiuridico);

            if (soggettoGiuridico == null)
            {
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Soggetto giuridico non trovato");

            }
            else
            {

                try
                {
                    riferimento.soggettoGiuridico = soggettoGiuridico;

                    RiferimentoBusiness.save(User.Identity.Name, riferimento, db);

                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, "Riferimento salvato con successo");
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    string messaggio;
                    messaggio = MyHelper.getDbEntityValidationException(ex);
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare il riferimento, verificare i dati: " + Environment.NewLine + messaggio);
                }
                catch (Exception ex)
                {
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare l'indirizzo, verificare i dati: " + Environment.NewLine + ex.Message);
                }


            }

            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);

        }*/





        [HttpPost]
        public ActionResult Update(Riferimento riferimento, int codiceRiferimento)
        {
            if (codiceRiferimento == 0)
            {
                throw new ApplicationException("Codice Riferimento NON valorizzato");
            }

            Riferimento riferimentoCorrente = db.Riferimento.Include("TipoRiferimento").Where(p => p.id == codiceRiferimento).FirstOrDefault();
            //Riferimento riferimentoCorrente = db.Riferimento.Where(p => p.id == codiceRiferimento).FirstOrDefault();
            if (riferimentoCorrente == null)
            {
                throw new ApplicationException("Riferimento NON trovato: " + codiceRiferimento);
            }


            //riferimentoCorrente.tipoRiferimento.id = riferimento.tipoRiferimento.id;
            riferimentoCorrente.tipoRiferimento = db.TipoRiferimento.Find(riferimento.tipoRiferimento.id);
            riferimentoCorrente.valore = riferimento.valore;

            //RiferimentoBusiness.valorizzaDatiRiferimento(riferimentoCorrente, db);

            ModelState.Clear();
            TryValidateModel(riferimentoCorrente);


            try
            {
                //LogEventi le = BusinessModel.Log.LogEventiManager.getEventoForUpdate(User.Identity.Name, riferimentoCorrente.id, EnumEntitaRiferimento.RIFERIMENTO, riferimentoCorrente, riferimento);
                //BusinessModel.Log.LogEventiManager.save(le, db);
                db.Riferimento.Attach(riferimentoCorrente);
                db.Entry(riferimentoCorrente).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, "Riferimento salvato con successo");
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                string messaggio;
                messaggio = MyHelper.getDbEntityValidationException(ex);
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare il riferimento, verificare i dati: " + Environment.NewLine + messaggio);
            }
            catch (Exception ex)
            {
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare il riferimento, verificare i dati: " + Environment.NewLine + ex.Message);
            }


            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

    }
}
