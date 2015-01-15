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
using mediatori.Models;
using BusinessModel;
namespace mediatori.Controllers
{
    public class PreventivoController : MyBaseController
    {
        private PreventivoManager manager = null;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (db != null)
            {
                manager = new PreventivoManager(db.Database.Connection);
            }
        }

        public ActionResult Index(int id, EnumTipoAzione tipoAzione)
        {

            Preventivo preventivo = new Preventivo();
            preventivo.finanziaria = new SoggettoGiuridico();
            preventivo.assicurazioneVita = new SoggettoGiuridico();
            preventivo.assicurazioneImpiego = new SoggettoGiuridico();
            return dispatch(preventivo, tipoAzione, db);
        }




        [ChildActionOnly]
        public ActionResult DetailsFromPratica(int praticaId)
        {
            mediatori.Models.Pratica.Pratica pratica;
            pratica = db.Pratiche.Find(praticaId);

            if (pratica == null)
            {
                return HttpNotFound();
            }

            PreventiviModel model = new PreventiviModel();
            model.praticaId = praticaId;
            model.preventivoConfermato = db.Preventivi.Find(pratica.preventivoConfermatoId);

            model.simulazioneEnabled = !String.IsNullOrEmpty((Session["MySessionData"] as SessionData).UrlSimulazioneFinanziaria);

            valorizzaDatiViewBag();

            return View("_Preventivi", model);
        }


        [ChildActionOnly]
        public ActionResult DetailsFromSegnalazione(int segnalazioneId)
        {
            Segnalazione segnalazione;
            segnalazione = db.Segnalazioni.Include("preventivi").Where(p => p.id == segnalazioneId).First();
            if (segnalazione == null)
            {
                return HttpNotFound();
            }

            PreventiviModel model = new PreventiviModel();
            model.preventiviSmall = segnalazione.preventivi.ToList<PreventivoSmall>();
            model.segnalazioneId = segnalazioneId;

            model.simulazioneEnabled = !String.IsNullOrEmpty((Session["MySessionData"] as SessionData).UrlSimulazioneFinanziaria);

            if (model.simulazioneEnabled)
            {
                //   model.preventivi = db.Preventivi.Where(p => p.segnalazioneId == segnalazioneId).ToList();
                model.preventivi = manager.getPreventiviBySegnalazione(segnalazioneId);
                model.preventiviSmall = null;
            }
            //else
            //{
            PreventivoSmall preventivo = new PreventivoSmall();

            preventivo.importoRata = segnalazione.rataRichiesta;
            preventivo.durata = segnalazione.durataRichiesta;
            preventivo.montante = (decimal)(preventivo.durata * preventivo.durata);

            model.nuovoPreventivoSmall = preventivo;
            // }




#if DEBUG
            //preventivo.importoCoperturaVita = 1000;
            //preventivo.importoCoperturaImpego = 700;
            //preventivo.montante = 36000;
            //preventivo.nettoCliente = 18000;
            //preventivo.importoProvvigioni = 300;
            //preventivo.importoInteressi = 100;
            //preventivo.importoImpegniDaEstinguere = 0;
            //preventivo.nomeProdotto = "Prodotto XYZ";
            //preventivo.speseAttivazione = 0;
            //preventivo.speseIncasso = 2;
            //preventivo.taeg = 20;
            //preventivo.tan = 3;
            //preventivo.teg = 4;
            //preventivo.tabellaFinanziaria = "TAB";
            //preventivo.dataDecorrenza = DateTime.Now;
            //preventivo.oneriFiscali = 20;
            ////preventivo.assicurazioneImpiegoId = 1;
            ////preventivo.assicurazioneVitaId = 1;
            ////preventivo.finanziariaId = 2;

#endif



            valorizzaDatiViewBag();

            return View("_Preventivi", model);
        }







        [ChildActionOnly]
        public ActionResult Create(Segnalazione segnalazione)
        {
            PreventivoSmall preventivo = new PreventivoSmall();
            //preventivo = preventivo == null ? new Preventivo() : preventivo;
            if (segnalazione != null)
            {
                preventivo.importoRata = segnalazione.rataRichiesta;
                preventivo.durata = segnalazione.durataRichiesta;
                // preventivo.montante = (decimal)(segnalazione.rataRichiesta * (float)segnalazione.durataRichiesta);

            }


#if DEBUG
            // preventivo.importoCoperturaVita = 1000;
            //preventivo.importoCoperturaImpego = 700;
            preventivo.montante = 36000;
            preventivo.nettoCliente = 18000;
            //preventivo.importoProvvigioni = 300;
            //preventivo.importoInteressi = 100;
            //preventivo.importoImpegniDaEstinguere = 0;
            //preventivo.nomeProdotto = "Prodotto XYZ";
            //preventivo.speseAttivazione = 0;
            //preventivo.speseIncasso = 2;
            preventivo.taeg = 20;
            preventivo.tan = 3;
            //preventivo.teg = 4;
            //preventivo.tabellaFinanziaria = "TAB";
            //preventivo.dataDecorrenza = DateTime.Now;
            //preventivo.oneriFiscali = 20;
            ////preventivo.assicurazioneImpiegoId = 1;
            //preventivo.assicurazioneVitaId = 1;
            //preventivo.finanziariaId = 2;

#endif

            valorizzaDatiViewBag();
            return View("PreventivoPartialEditSmall", preventivo);
        }


        public JsonResult Conferma(int id, Boolean statoConferma = true)
        {
            Models.JsonMessageModel model = new Models.JsonMessageModel();

            //  Preventivo preventivo = db.Preventivi.Include("segnalazione").Include("segnalazione.fontePubblicitaria").Include("segnalazione.altroPrestito").Include("segnalazione.contatto").First(d => d.id == id);
            // Preventivo preventivo = db.Preventivi.Include("segnalazione").Include("segnalazione.contatto").First(d => d.id == id);
            PreventivoSmall preventivo = db.PreventiviSmall.Include("segnalazione").Include("segnalazione.contatto").First(d => d.id == id);
            if (preventivo == null)
            {
                model.esito = Models.JsonMessageModel.Esito.Failed;
                model.messaggio = "Preventivo non trovato";
                return Json(model, JsonRequestBehavior.AllowGet);
            }

            //Verifico tutti i dati necessari per passare la Segnalazione in Pratica

            // preventivo.segnalazione.contatto.

            List<Impiego> impieghi;
            impieghi = db.Impieghi.Where(i => i.contattoId == preventivo.segnalazione.contatto.id).ToList<Impiego>();
            if (impieghi.Count == 0)
            {
                model.esito = Models.JsonMessageModel.Esito.Failed;
                model.messaggio = "Configurare almeno un impiego";
                return Json(model, JsonRequestBehavior.AllowGet);
            }

            if (impieghi.Count == 1 && impieghi[0].dataAssunzione == null)
            {
                model.esito = Models.JsonMessageModel.Esito.Failed;
                model.messaggio = "Configurare almeno un impiego";
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

                db.Database.ExecuteSqlCommand("UPDATE preventivo SET Tipo = 'Confermato'  where id = " + preventivo.id);

                //TODO: Confifurare lo stato base della PRATICA

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
            catch (Exception ex)
            {
                model.esito = Models.JsonMessageModel.Esito.Failed;
                model.messaggio = ex.Message;
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult preventivoPartial(Segnalazione segnalazione, EnumTipoAzione tipoAzione)
        //{
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
            return dispatch(preventivo, tipoAzione, db);
        }



        [HttpPost]
        public ActionResult createForSegnalazione(PreventivoSmall p, int idSegnalazione)
        {
            //Segnalazione s = new SegnalazioneBusiness().findByPk(idSegnalazione, db);


            Segnalazione s = db.Segnalazioni.Include("preventivi").Where(t => t.id == idSegnalazione).First();


            if (s == null)
            {
                return HttpNotFound();
            }


            if (s.preventivi == null)
            {
                s.preventivi = new List<PreventivoSmall>();
            }

            // p.id = 0;
            p.progressivo = s.preventivi.Count() + 1;

            //int idAssicurazioneVita = p.assicurazioneVita.id;
            //int idAssicurazioneImpiego = p.assicurazioneImpiego.id;
            //int idFinanziari = p.finanziaria.id;

            //p.assicurazioneVita = db.SoggettiGiuridici.Where(aa => aa.id == idAssicurazioneVita).FirstOrDefault();
            //p.assicurazioneImpiego = db.SoggettiGiuridici.Where(aa => aa.id == idAssicurazioneImpiego).FirstOrDefault();
            //p.finanziaria = db.SoggettiGiuridici.Where(aa => aa.id == idFinanziari).FirstOrDefault();

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
                catch (Exception ex)
                {
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare il preventivo, verificare i dati: " + Environment.NewLine + ex.Message);
                }
            }

            return RedirectToAction("Details", "Segnalazioni", new { id = idSegnalazione });
        }




        [HttpPost]
        public ActionResult Update(Preventivo p, int praticaId)
        {
            //mediatori.Models.Pratica.Pratica  pratica;

            //pratica = db.Pratiche.Find(praticaId);

            //if (pratica == null)
            //{
            //    return HttpNotFound(); 
            //}

            //pratica.preventivi.


            // p.id = 0;
            // p.progressivo = s.preventivi.Count() + 1;

            int idAssicurazioneVita = p.assicurazioneVita.id;
            int idAssicurazioneImpiego = p.assicurazioneImpiego.id;
            int idFinanziari = p.finanziaria.id;

            p.assicurazioneVita = db.SoggettiGiuridici.Where(aa => aa.id == idAssicurazioneVita).FirstOrDefault();
            p.assicurazioneImpiego = db.SoggettiGiuridici.Where(aa => aa.id == idAssicurazioneImpiego).FirstOrDefault();
            p.finanziaria = db.SoggettiGiuridici.Where(aa => aa.id == idFinanziari).FirstOrDefault();


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
                catch (Exception ex)
                {
                    TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare il preventivo, verificare i dati: " + Environment.NewLine + ex.Message);
                }
            }

            return RedirectToAction("Details", "Pratiche", new { id = praticaId });
        }



        private void valorizzaDatiViewBag()
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
                    valorizzaDatiViewBag();
                    return View("PreventivoPartialEdit", p);
                case EnumTipoAzione.VISUALIZZAZIONE:
                    return View("PreventivoPartialDetail", p);
            }
            return View("PreventivoPartialDetail", p);
        }

    }
}
