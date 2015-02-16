using mediatori.Controllers.Business;
using mediatori.Controllers.Business.Anagrafiche;
using mediatori.Filters;
using mediatori.Models;
using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using BusinessModel.Anagrafiche.PersonaFisica;
using BusinessModel.Anagrafiche.Contatto;
using BusinessModel.Log;

namespace mediatori.Controllers
{
    public class ContattoController : MyBaseController
    {
        private PersonaFisicaManager manager = null;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (db != null)
            {
                manager = new PersonaFisicaManager(db.Database.Connection);
            }
        }

        private void valorizzaViewBag()
        {
            valorizzaViewBag(null);
        }

        private void valorizzaViewBag(Contatto contatto)
        {
            //  ViewBag.listaSesso = new SelectList(new List<SelectListItem> { new SelectListItem { Text = "M", Value = "M" }, new SelectListItem { Text = "F", Value = "F" } }, null);

            ViewBag.listaProvincia = new SelectList(db.Province.OrderBy(p => p.denominazione).ToList(), "denominazione", "denominazione");

            if (contatto != null && contatto.provinciaNascita != null && !String.IsNullOrEmpty(contatto.provinciaNascita.denominazione))
            {

                ViewBag.listaComuni = new SelectList(db.Comuni.Where(c => c.codiceProvincia == contatto.provinciaNascita.id).OrderBy(p => p.denominazione).ToList(), "denominazione", "denominazione");


                Debug.WriteLine((ViewBag.listaComuni as SelectList).Count());

                //List<SelectListItem> lsli = new List<SelectListItem>();
                //lsli.Add(new SelectListItem { Text = "", Value = "" });

                //ViewBag.listaComuni = lsli;
            }
            else
            {

                List<SelectListItem> lsli = new List<SelectListItem>();
                lsli.Add(new SelectListItem { Text = "", Value = "" });

                ViewBag.listaComuni = lsli;
            }


            //if (contatto != null && contatto.comuneNascita != null && !String.IsNullOrEmpty(contatto.comuneNascita.denominazione))
            //{
            //    ViewBag.listaComuni = new SelectList(db.Comuni.Where(c => c.denominazione == contatto.comuneNascita.denominazione && c.codiceProvincia == contatto.comuneNascita.provincia.id).OrderBy(p => p.denominazione).ToList(), contatto.comuneNascita.denominazione, "denominazione", "denominazione");
            //}
            //else
            //{
            //    List<SelectListItem> lsli = new List<SelectListItem>();
            //    lsli.Add(new SelectListItem { Text = "", Value = "" });
            //    ViewBag.listaComuni = lsli;
            //}



        }

        [ChildActionOnly]
        public ActionResult Create(Contatto contatto)
        {
            //in fase di creazine di una segnalazione posso trovare un contatto già esistente
            valorizzaViewBag(contatto);

            ViewData.TemplateInfo.HtmlFieldPrefix = "contatto";
            return View("ContattoPartialEdit", contatto);
        }


        [HttpGet]
        public ActionResult Details(int id)
        {
            //valorizzaViewBag();
            Contatto c = ContattoManager.findByPK(id, db);

            if (c == null)
            {
                return HttpNotFound();
            }

            Models.ContattoDetailsModel model = new ContattoDetailsModel();
            model.contatto = c;

            return View(model);
        }



        [ChildActionOnly]
        public ActionResult DetailsV2(int id, bool isCedente = false)
        {
            //valorizzaViewBag();
            Contatto contatto = ContattoManager.findByPK(id, db);

            if (contatto == null)
            {
                return HttpNotFound();
            }

            valorizzaViewBag(contatto);

            Models.ContattoDetailsModel model = new ContattoDetailsModel();
            model.contatto = contatto;
            model.isCedente = isCedente;

            return View("_Contatto", model);
        }




        public ActionResult Index(SearchPersonaFisica model)
        {
            model.tipoPersonaFisica = PersonaFisicaManager.TipoPersonaFisica.Contatto;
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

        [HttpGet]
        public String findContattoByNomeCognome(String nome, String cognome)
        {
            Debug.WriteLine("findContattoByNomeCognome");
            ICollection<Contatto> listaContatti = ContattoBusiness.findByFilter(new ContattoFilter() { nome = nome, cognome = cognome }, db);
            return ContattoBusiness.asHtml(listaContatti);
        }

        [HttpGet]
        public String findContattoByCodiceFiscale(String codiceFiscale)
        {
            ICollection<Contatto> listaContatti = ContattoBusiness.findByFilter(new ContattoFilter() { codiceFiscale = codiceFiscale }, db);
            return ContattoBusiness.asHtml(listaContatti);
        }



        public ActionResult ContattoPartialById(int id, EnumTipoAzione tipoAzione)
        {
            Contatto contatto;
            contatto = ContattoManager.findByPK(id, db);

            if (contatto == null)
            {
                return HttpNotFound();
            }

            if (tipoAzione == EnumTipoAzione.MODIFICA)
            {
                valorizzaViewBag(contatto);
                ViewData.TemplateInfo.HtmlFieldPrefix = "contatto";
                return View("ContattoEdit", contatto);
            }

            if (tipoAzione == EnumTipoAzione.VISUALIZZAZIONE)
            {
                return View("ContattoPartialDetail", contatto);
            }

            //  valorizzaViewBag();
            throw new ApplicationException("Azione di inserimento che non si deve presentare");
        }

        [ChildActionOnly]
        public ActionResult contattoPartial(Contatto contatto, EnumTipoAzione tipoAzione)
        {
            return dispatch(contatto, tipoAzione);
        }
        [HttpPost]
        public ActionResult Edit(Contatto contatto)
        {
            ContattoManager.valorizzaDati(contatto, db);

            ModelState.Clear();
            TryValidateModel(contatto);


            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                  .SelectMany(v => v.Errors)
                  .Select(e => e.ErrorMessage));
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare il contatto, verificare i dati: " + Environment.NewLine + message);
                return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
            }



            ContattoBusiness contattoBusiness = new ContattoBusiness();
            Contatto contattoOriginale = ContattoManager.findByPK(contatto.id, db);

            contatto = contattoBusiness.copiaRiferimenti(contattoOriginale, contatto);

            LogEventi le = LogEventiManager.getEventoForUpdate(User.Identity.Name, contatto.id, EnumEntitaRiferimento.CONTATTO, contattoOriginale, contatto);
            contattoOriginale = (Contatto)CopyObject.simpleCompy(contattoOriginale, contatto);

            try
            {
                LogEventiManager.save(le, db);
                db.SaveChanges();
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, "Contatto salvato con successo");
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                string messaggio;
                messaggio = MyHelper.getDbEntityValidationException(ex);
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare il contatto, verificare i dati: " + Environment.NewLine + messaggio);
            }
            catch (Exception ex)
            {
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, "Impossibile salvare il contatto, verificare i dati: " + Environment.NewLine + ex.Message);
            }
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }


        private ActionResult dispatch(Contatto contatto, EnumTipoAzione tipoAzione)
        {
            switch (tipoAzione)
            {
                case EnumTipoAzione.VISUALIZZAZIONE:
                    return View("ContattoPartialDetail", contatto);
                case EnumTipoAzione.MODIFICA:
                    return View("ContattoPartialEdit", contatto);
            }
            return View("ContattoPartialDetail", contatto);
        }
    }
}
