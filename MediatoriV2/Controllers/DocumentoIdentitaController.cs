using mediatori.Controllers.Business;
using mediatori.Controllers.Business.Anagrafiche;
using mediatori.Controllers.Business.Anagrafiche.Soggetto;
using mediatori.Controllers.CQS;
using mediatori.Models;
using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mediatori.Controllers
{
    public class DocumentoIdentitaController : MyBaseController
    {
        //
        // GET: /DocumentoIdentita/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DocumentoIdentitaPartialInsert(DocumentoIdentita di)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            valorizzaDatiDocumentoIdentita(db);
            ViewBag.provinciaRilascio = new SelectList(db.Province.ToList(), "denominazione", "denominazione");
            // ViewBag.comuneRilascio = new SelectList((from c in db.Comuni where c.provincia.id == di.provinciaEnte.id select c).ToList(), "denominazione", "denominazione");

            if (di.provinciaEnte != null)
            {
                ViewBag.comuneRilascio = new SelectList((db.Comuni.ToList().Where(c => c.provincia.id == di.provinciaEnte.id)), "denominazione", "denominazione");
            }
            else
            {
                List<SelectListItem> lsli = new List<SelectListItem>();
                lsli.Add(new SelectListItem { Text = "", Value = "" });
                ViewBag.comuneRilascio = new SelectList(lsli, "Text", "Value");

            }
            return View("DocumentoIdentitaPartialInsert", di);
        }

        public ActionResult DocumentoIdentitaPartial(DocumentoIdentita di)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            valorizzaDatiDocumentoIdentita(db);
            ViewBag.provinciaRilascio = new SelectList(db.Province.ToList(), "denominazione", "denominazione");
            // ViewBag.comuneRilascio = new SelectList((from c in db.Comuni where c.provincia.id == di.provinciaEnte.id select c).ToList(), "denominazione", "denominazione");

            if (di.provinciaEnte != null)
            {
                ViewBag.comuneRilascio = new SelectList((db.Comuni.ToList().Where(c => c.provincia.id == di.provinciaEnte.id)), "denominazione", "denominazione");
            }
            else
            {
                List<SelectListItem> lsli = new List<SelectListItem>();
                lsli.Add(new SelectListItem { Text = "", Value = "" });
                ViewBag.comuneRilascio = new SelectList(lsli, "Text", "Value");

            }
            return View("DocumentoIdentitaPartialEdit", di);
        }

        public ActionResult DocumentoIdentitaPartialById(int id, EnumTipoAzione tipoAzione = EnumTipoAzione.VISUALIZZAZIONE)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            DocumentoIdentita docI = new DocumentoIdentita();
            if (id != 0)
            {
                docI = (from di in db.DocumentiIdentita.Include("enteRilascio")
                                                        .Include("provinciaEnte")
                                                        .Include("comuneEnte")
                        where di.id == id
                        select di).First();
            }
            if (tipoAzione == EnumTipoAzione.MODIFICA)
            {
                valorizzaDatiDocumentoIdentita(db);
                PopolaDropDownListAnagrafiche pddla = new PopolaDropDownListAnagrafiche();
                String defaultValue = String.Empty;
                if (docI.provinciaEnte != null)
                {
                    defaultValue = docI.provinciaEnte.denominazione;
                    ViewBag.comuneRilascio = new SelectList((from c in db.Comuni
                                                             where c.provincia.id == docI.provinciaEnte.id
                                                             select c).ToList(),
                                      "denominazione", "denominazione", docI.comuneEnte.denominazione);

                }

                ViewBag.provinciaRilascio = new SelectList(db.Province.ToList(),
                                        "denominazione", "denominazione", defaultValue);


                return View("DocumentoIdentitaPartialEdit", docI);
            }
            else
            {
                return View("DocumentoIdentitaPartialDetail", docI);
            }
        }
        public ActionResult DocumentoIdentitaPartialDetail(DocumentoIdentita di)
        {
            return View(di);
        }


        [HttpPost]
        public ActionResult Edit(DocumentoIdentita di, String contestoRedirect, String oggettoRedirect, String idRedirect)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);

            valorizzaDatiDaRequest(di, db);
            DocumentoIdentita diOriginale = (from d in db.DocumentiIdentita.Include("enteRilascio")
                                                                           .Include("provinciaEnte")
                                                                           .Include("comuneEnte")
                                             where d.id == di.id
                                             select d).First();
            LogEventi le = LogEventiManager.getEventoForUpdate(User.Identity.Name, di.id, EnumEntitaRiferimento.DOCUMENTO_IDENTITA, diOriginale, di);
            diOriginale = (DocumentoIdentita)CopyObject.simpleCompy(diOriginale, di);
            LogEventiManager.save(le, db);
            db.SaveChanges();
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
            //return View("DocumentoIdentitaPartialDetail", di);
        }

        private static DocumentoIdentita valorizzaDatiDaRequest(DocumentoIdentita di, MainDbContext db)
        {
            di.enteRilascio = db.TipoEnteRilascio.Find(di.enteRilascio.id);
            di.provinciaEnte = (from p in db.Province where p.denominazione == di.provinciaEnte.denominazione select p).First();
            di.comuneEnte = (from c in db.Comuni
                             where
                                 c.denominazione == di.comuneEnte.denominazione && c.provincia.id == di.provinciaEnte.id
                             select c).First();
            return di;
        }
        [HttpPost]
        public ActionResult CreateForCedente(DocumentoIdentita di, int codiceCedente)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            di = valorizzaDatiDaRequest(di, db);

            ModelState.Remove("enteRilascio.descrizione");
            ModelState.Remove("provinciaEnte.sigla");
            ModelState.Remove("comuneEnte.denominazione");
            if (ModelState.IsValid)
            {
                Cedente cedente = RicercaCedenteBusiness.find(codiceCedente, db);
                cedente.documentoIdentita.Add(di);
                LogEventiManager.save(LogEventiManager.getEventoForCreate(User.Identity.Name, di.id, EnumEntitaRiferimento.DOCUMENTO_IDENTITA), db);
                db.SaveChanges();
            }

            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        private void valorizzaDatiDocumentoIdentita(MainDbContext db)
        {
            ViewBag.inizioValidita = System.DateTime.Now.Year - 15;
            ViewBag.oggi = System.DateTime.Now.Year;
            ViewBag.fineValidita = System.DateTime.Now.Year + 15;
            ViewBag.listaEnteRilascio = new SelectList(db.TipoEnteRilascio.ToList(), "id", "descrizione");
            List<SelectListItem> lsli = new List<SelectListItem>();
            lsli.Add(new SelectListItem { Text = "", Value = "" });
            ViewBag.provinciaRilascio = new SelectList(lsli, "Text", "Value");
            ViewBag.comuneRilascio = new SelectList(lsli, "Text", "Value");
        }

    }
}
