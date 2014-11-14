﻿using mediatori.Controllers.Business;
using mediatori.Filters;
using mediatori.Models.Anagrafiche;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using mediatori.Models;
namespace mediatori.Controllers
{
    public class PreventivoController : MyBaseController
    {
        private MainDbContext db = new MainDbContext();

        public ActionResult Index(int id, EnumTipoAzione tipoAzione)
        {
            //MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            Preventivo preventivo = new Preventivo();
            preventivo.finanziaria = new SoggettoGiuridico();
            preventivo.assicurazioneVita = new SoggettoGiuridico();
            preventivo.assicurazioneImpiego = new SoggettoGiuridico();
            return dispatch(preventivo, tipoAzione, db);
        }

        [ChildActionOnly]
        public ActionResult Create(Segnalazione segnalazione)
        {
            Preventivo preventivo = new Preventivo();
            //preventivo = preventivo == null ? new Preventivo() : preventivo;
            if (segnalazione != null)
            {
                preventivo.importoRata = segnalazione.rataRichiesta;
                preventivo.durata = segnalazione.durataRichiesta;
                // preventivo.montante = (decimal)(segnalazione.rataRichiesta * (float)segnalazione.durataRichiesta);

            }


#if DEBUG
            preventivo.importoCoperturaVita = 1000;
            preventivo.importoCoperturaImpego = 700;
            preventivo.montante = 36000;
            preventivo.nettoCliente = 18000;
            preventivo.importoProvvigioni = 300;
            preventivo.importoInteressi = 100;
            preventivo.importoImpegniDaEstinguere = 0;
            preventivo.nomeProdotto = "Prodotto XYZ";
            preventivo.speseAttivazione = 0;
            preventivo.speseIncasso = 2;
            preventivo.taeg = 20;
            preventivo.tan = 3;
            preventivo.teg = 4;
            preventivo.tabellaFinanziaria = "TAB";
            preventivo.dataDecorrenza = DateTime.Now;
            preventivo.oneriFiscali = 20;
            //preventivo.assicurazioneImpiegoId = 1;
            //preventivo.assicurazioneVitaId = 1;
            //preventivo.finanziariaId = 2;

#endif

            valorizzaViewBag(db);
            return View("PreventivoPartialEdit", preventivo);
        }


        public JsonResult Conferma(int id, Boolean statoConferma = true)
        {
            Models.JsonMessageModel model = new Models.JsonMessageModel();

            Preventivo preventivo = db.preventivi.Include("segnalazione").Include("segnalazione.fontePubblicitaria").Include("segnalazione.altroPrestito").Include("segnalazione.contatto").First(d => d.id == id);
            if (preventivo == null)
            {
                model.esito = Models.JsonMessageModel.Esito.Failed;
                model.messaggio = "Preventivo non trovato";
                return Json(model, JsonRequestBehavior.AllowGet);
            }

            if (statoConferma == true)
            {
                preventivo.dataConferma = System.DateTime.Now;
                preventivo.operatoreConferma = User.Identity.Name;
            }
            else
            {
                preventivo.dataConferma = null;
                preventivo.operatoreConferma = null;
            }

            preventivo.segnalazione.preventivoConfermatoId = preventivo.id;

            try
            {
                db.SaveChanges();

                db.Database.ExecuteSqlCommand("UPDATE Segnalazione SET Discriminator = 'Pratica'  where id = " + preventivo.segnalazione.id);

                db.Database.ExecuteSqlCommand("UPDATE Segnalazione SET cedente_id = contatto_id  where id = " + preventivo.segnalazione.id);

                db.Database.ExecuteSqlCommand("UPDATE persona_fisica SET tipoPersonaFisica = 'Cedente'  where id = " + preventivo.segnalazione.contatto.id);

                model.referenceId = preventivo.segnalazione.id.ToString();
                model.esito = Models.JsonMessageModel.Esito.Succes;
                model.messaggio = "Operazione conlusa con successo";

            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                string temp;
                temp = MyHelper.getDbEntityValidationException(ex);

                Debug.WriteLine("DbEntityValidationException: " + temp);
                model.esito = Models.JsonMessageModel.Esito.Failed;
                model.messaggio = temp;
            }
            catch (Exception ex) {
                model.esito = Models.JsonMessageModel.Esito.Failed;
                model.messaggio = ex.Message ;
            }

            return Json(model, JsonRequestBehavior.AllowGet);
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



        public ActionResult Partial(Preventivo preventivo, EnumTipoAzione tipoAzione, Segnalazione segnalazione)
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



        [HttpPost]
        public ActionResult createForSegnalazione(Preventivo p, int idSegnalazione)
        {
            Segnalazione s = new SegnalazioneBusiness().findByPk(idSegnalazione, db);

            if (s == null)
            {
                return HttpNotFound();
            }


            if (s.preventivi == null)
            {
                s.preventivi = new List<Preventivo>();
            }

            // p.id = 0;
            p.progressivo = s.preventivi.Count() + 1;

            int idAssicurazioneVita = p.assicurazioneVita.id;
            int idAssicurazioneImpiego = p.assicurazioneImpiego.id;
            int idFinanziari = p.finanziaria.id;

            p.assicurazioneVita = db.SoggettiGiuridici.Where(aa => aa.id == idAssicurazioneVita).FirstOrDefault();
            p.assicurazioneImpiego = db.SoggettiGiuridici.Where(aa => aa.id == idAssicurazioneImpiego).FirstOrDefault();
            p.finanziaria = db.SoggettiGiuridici.Where(aa => aa.id == idFinanziari).FirstOrDefault();

            p.dataInserimento = DateTime.Now;
            p.operatoreInserimento = User.Identity.Name;

            s.preventivi.Add(p);

            ModelState.Clear();
            TryValidateModel(p);

            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare il preventivo, verificare i dati: " + Environment.NewLine + message);
            }
            else
            {
                try
                {
                    db.SaveChanges();
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, "Preventivo salvato con successo");
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    string messaggio;
                    messaggio = MyHelper.getDbEntityValidationException(ex);
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare il preventivo, verificare i dati: " + Environment.NewLine + messaggio);
                }
            }

            return RedirectToAction("Details", "Segnalazioni", new { id = idSegnalazione });
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