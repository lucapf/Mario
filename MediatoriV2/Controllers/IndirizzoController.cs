using mediatori.Controllers.Business;
using mediatori.Controllers.Business.Anagrafiche.Soggetto;
using mediatori.Controllers.CQS;
using mediatori.Models;
using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mediatori.Controllers
{
    public class IndirizzoController : MyBaseController
    {
        [ChildActionOnly]
        public ActionResult Cedente(int cedenteId)
        {
            Cedente cedente;
            cedente = db.Cedenti.Include("indirizzi").Where(p => p.id == cedenteId).First();
            if (cedente == null)
            {
                return HttpNotFound();
            }

            IndirizziModel model = new IndirizziModel();
            model.indirizzi = cedente.indirizzi.ToList<Indirizzo>();
            model.contattoId = cedenteId;

            valorizzaViewBag();

            return View("_Indirizzi", model);
        }

        [ChildActionOnly]
        public ActionResult SoggettoGiuridico(int soggettoGiuridicoId)
        {
            IndirizziModel model = new IndirizziModel();
            model.indirizzi = new List<Indirizzo>();

            if (soggettoGiuridicoId != -1)
            {
                SoggettoGiuridico soggettoGiuridico;
                soggettoGiuridico = db.SoggettiGiuridici.Include("indirizzi").Where(p => p.id == soggettoGiuridicoId).First();
                if (soggettoGiuridico == null)
                {
                    return HttpNotFound();
                }

                foreach (Indirizzo i in soggettoGiuridico.indirizzi)
                {
                    Debug.WriteLine("Indirizzo id: " + i.id + " Provicia: " + i.provincia.sigla);

                    model.indirizzi.Add(IndirizzoBusiness.findIndirizzo(i.id, db));
                }


                //                model.indirizzi = IndirizzoBusiness.valorizzaDatiPerInserimentoCancellazione(soggettoGiuridico.indirizzi, db).ToList<Indirizzo>();


                //model.indirizzi = soggettoGiuridico.indirizzi.ToList<Indirizzo>();


                model.soggettoGiuridicoId = soggettoGiuridicoId;
            }
            else
            {
                //CREATE
                model.soggettoGiuridicoId = -1;
                model.indirizzi = new List<Indirizzo>();
            }

            valorizzaViewBag();

            return View("_Indirizzi", model);
        }


        [HttpGet]
        public ActionResult IndirizzoPartialById(int id, EnumTipoAzione tipoAzione = EnumTipoAzione.MODIFICA)
        {
            Indirizzo indirizzo;
            indirizzo = IndirizzoBusiness.findIndirizzo(id, db);

            if (indirizzo == null)
            {
                return HttpNotFound();
            }

            if (tipoAzione == EnumTipoAzione.MODIFICA)
            {
                valorizzaViewBag(indirizzo);
                ViewData.TemplateInfo.HtmlFieldPrefix = "indirizzo";
                return View("IndirizzoEdit", indirizzo);
            }

            if (tipoAzione == EnumTipoAzione.VISUALIZZAZIONE)
            {
                return View("IndirizzoPartialDetail", indirizzo);
            }

            //  valorizzaViewBag();
            //return View("impiegoPartialEdit", impiego);
            throw new ApplicationException("Azione di inserimento che non si deve presentare");
        }

        // GET /Indirizzo/IndirizzoPartial
        [ChildActionOnly]
        public ActionResult IndirizzoPartial(Indirizzo indirizzo, EnumTipoAzione tipoAzione = EnumTipoAzione.MODIFICA)
        {

            if (indirizzo == null || indirizzo.id == 0)
            {
                IndirizzoBusiness.valorizzaDatiDefault(indirizzo);
            }
            switch (tipoAzione)
            {
                case EnumTipoAzione.INSERIMENTO:
                    valorizzaListeIndirizzoEdit(db, indirizzo);
                    return View("IndirizzoPartialInsert", indirizzo);
                //  return View("IndirizzoPartialEdit", indirizzo);
                case EnumTipoAzione.MODIFICA:
                    valorizzaListeIndirizzoEdit(db, indirizzo);
                    return View("IndirizzoPartialEdit", indirizzo);
                default:
                    return View("IndirizzoPartialDetail", indirizzo);
            }
        }

        private void valorizzaViewBag()
        {
            valorizzaViewBag(null);
        }

        private void valorizzaViewBag(Indirizzo indirizzo)
        {
            if (indirizzo == null)
            {
                ViewBag.listaProvincia = new SelectList(db.Province.ToList(), "denominazione", "denominazione");

                List<SelectListItem> lsli = new List<SelectListItem>();
                lsli.Add(new SelectListItem { Text = "", Value = "" });
                ViewBag.listaComuni = lsli;

                ViewBag.listaToponimo = new SelectList(db.Toponimi, "sigla", "sigla");
                ViewBag.listaTipoIndirizzo = new SelectList(db.TipoIndirizzo, "id", "descrizione");
            }
            else
            {
                ViewBag.listaProvincia = new SelectList(db.Province.ToList(), "denominazione", "denominazione", indirizzo.provincia.denominazione);

                if (String.IsNullOrEmpty(indirizzo.provincia.denominazione))
                {
                    List<SelectListItem> lsli = new List<SelectListItem>();
                    lsli.Add(new SelectListItem { Text = "", Value = "" });
                    ViewBag.listaComuni = lsli;

                }
                else
                {
                    // SelectList sli = new SelectList((from c in db.Comuni where c.denominazione == indirizzo.provincia.denominazione select c), "denominazione", "denominazione", indirizzo.comune.denominazione);
                    SelectList sli = new SelectList((from c in db.Comuni join p in db.Province on c.codiceProvincia equals p.id where p.denominazione == indirizzo.provincia.denominazione select c), "denominazione", "denominazione", indirizzo.comune.denominazione);

                    ViewBag.listaComuni = sli;
                }


                ViewBag.listaToponimo = new SelectList(db.Toponimi, "sigla", "sigla");
                ViewBag.listaTipoIndirizzo = new SelectList(db.TipoIndirizzo, "id", "descrizione");
            }
        }

        private void valorizzaListeIndirizzoEdit(MainDbContext db, Indirizzo indirizzo)
        {

            valorizzaListeIndirizzoDetails(db);
            ViewBag.listaProvincia = new SelectList(db.Province.ToList(), "denominazione", "denominazione");
            ViewBag.listaComuni = new SelectList((from c in db.Comuni where c.provincia.id == indirizzo.provincia.id select c), "denominazione", "denominazione");
        }


        public void valorizzaListeIndirizzoDetails(MainDbContext db)
        {
            List<SelectListItem> lsli = new List<SelectListItem>();
            lsli.Add(new SelectListItem { Text = "", Value = "" });
            ViewBag.listaToponimo = new SelectList(db.Toponimi, "sigla", "sigla");
            ViewBag.listaTipoIndirizzo = new SelectList(db.TipoIndirizzo, "id", "descrizione");
            ViewBag.listaProvincia = new SelectList(lsli, "Text", "Value");
            ViewBag.listaComuni = new SelectList(lsli, "Text", "Value");
        }

        public void eliminaElementiNonCaricati()
        {
            ModelState.Remove("tipoIndirizzo.descrizione");
            ModelState.Remove("provincia.sigla");
        }

        [HttpPost]
        public ActionResult Edit(Indirizzo indirizzo)
        {
            eliminaElementiNonCaricati();
            if (ModelState.IsValid)
            {
                Indirizzo indirizzoSalvato = IndirizzoBusiness.save(User.Identity.Name, indirizzo, db);
            }

            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }



        [ChildActionOnly]
        public ActionResult Create(Indirizzo indirizzo)
        {

#if DEBUG
            indirizzo.cap = "00100";
            indirizzo.numeroCivico = "20";
            indirizzo.recapito = "Giuseppe Verdi";
#endif

            valorizzaViewBag(indirizzo);

            ViewData.TemplateInfo.HtmlFieldPrefix = "indirizzo";
            return View("IndirizzoPartialEdit", indirizzo);
        }




        [HttpPost]
        public ActionResult CreateForPersonaFisica(Indirizzo indirizzo, int codiceId)
        {

            indirizzo.provincia = (from p in db.Province
                                   where p.denominazione == indirizzo.provincia.denominazione
                                   select p).First();

            indirizzo.comune = (from c in db.Comuni
                                where c.denominazione == indirizzo.comune.denominazione &&
                                      c.codiceProvincia == indirizzo.provincia.id
                                select c).First();
            ModelState.Clear();
            TryValidateModel(indirizzo);

            if (ModelState.IsValid)
            {
                indirizzo = IndirizzoBusiness.createBySegnalazione(User.Identity.Name, codiceId, indirizzo, db);
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare l'indirizzo, verificare i dati: " + Environment.NewLine + message);
            }
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        [HttpPost]
        public ActionResult CreateForCedente(Indirizzo indirizzo, int codiceCedente)
        {

            //  eliminaElementiNonCaricati();
            if (ModelState.IsValid)
            {
                indirizzo = IndirizzoBusiness.createBySegnalazione(User.Identity.Name, codiceCedente, indirizzo, db);
            }
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        [HttpPost]
        public ActionResult CreateForSoggettoGiuridico(Indirizzo indirizzo, int codiceSoggettoGiuridico)
        {
            indirizzo = IndirizzoBusiness.valorizzaDatiPerInserimentoCancellazione(indirizzo, db);
            ModelState.Clear();
            TryValidateModel(indirizzo);
            if (ModelState.IsValid)
            {
                indirizzo = IndirizzoBusiness.createBySoggettoGiuridico(User.Identity.Name, codiceSoggettoGiuridico, indirizzo, db);
            }
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }





        [HttpPost]
        public ActionResult UpdateForSoggettoGiuridico(Indirizzo indirizzo, int codiceSggettoGiuridico)
        {
            IndirizzoBusiness.valorizzaDatiPerInserimentoCancellazione(indirizzo, db);

            indirizzo.soggettoGiuridicoId = codiceSggettoGiuridico;

            ModelState.Clear();
            TryValidateModel(indirizzo);


            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                  .SelectMany(v => v.Errors)
                  .Select(e => e.ErrorMessage));
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare l'indirizzo, verificare i dati: " + Environment.NewLine + message);
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
                    IndirizzoBusiness.save(User.Identity.Name, indirizzo, db);

                    //Indirizzo original = IndirizzoBusiness.findIndirizzo(indirizzo.id, db);

                    // original.tipoIndirizzo = indirizzo.tipoIndirizzo;
                    //original.cap = indirizzo.cap

                    /////db.Indirizzi.Attach(original);
                    ////db.Entry(original).State = System.Data.Entity.EntityState.Modified;

                    ////indirizzo.soggettoGiuridico = soggettoGiuridico;


                    ////db.Entry(soggettoGiuridico).State = System.Data.Entity.EntityState.Unchanged;
                    //db.Indirizzi.app.Attach(indirizzo);
                    //db.Entry(indirizzo).State = System.Data.Entity.EntityState.Modified;

                    //db.TipoIndirizzo.Attach(indirizzo.tipoIndirizzo);
                    //db.Entry(indirizzo.tipoIndirizzo).State = System.Data.Entity.EntityState.Modified;




                    //db.SaveChanges();
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, "Indirizzo salvato con successo");
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    string messaggio;
                    messaggio = MyHelper.getDbEntityValidationException(ex);
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare l'indirizzo, verificare i dati: " + Environment.NewLine + messaggio);
                }
                catch (Exception ex)
                {
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare l'indirizzo, verificare i dati: " + Environment.NewLine + ex.Message);
                }


            }

            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);

        }

    }
}
