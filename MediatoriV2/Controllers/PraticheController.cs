using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using mediatori.Controllers.Business;
using mediatori.Models.Pratica;


namespace mediatori.Controllers
{

    [MyAuthorize(Roles = new string[] { MyConstants.Profilo.DIPENDENTE, MyConstants.Profilo.AMMINISTRATORE, MyConstants.Profilo.COLLABORATORE })]
    public class PraticheController : MyBaseController
    {

        public ActionResult Index(Models.Pratica.SearchPratica model)
        {
            IQueryable<mediatori.Models.Pratica.Pratica> query = db.Pratiche.Include("cedente").Include("prodottoRichiesto").Include("stato");

            //model.Pratiche = db.Pratiche.Include("cedente").Include("prodottoRichiesto");

            Debug.WriteLine("Profilo: " + (Session["MySessionData"] as MyManagerCSharp.MySessionData).Profili);

            if ((Session["MySessionData"] as MyManagerCSharp.MySessionData).Profili.IndexOf(MyConstants.Profilo.COLLABORATORE.ToString()) > -1)
            {
                query = query.Where(p => p.utenteInserimento == User.Identity.Name);

            }
            model.Pratiche = query.ToList();

            return View(model);
        }

        public ActionResult Details(int id)
        {
            mediatori.Models.PraticaDetailsModel model = new Models.PraticaDetailsModel();
            //    model.pratica = db.Pratiche.Include("stato").Include("cedente").Include("preventivi").Include("note").Where(p => p.id == id).FirstOrDefault();

            model.pratica = db.Pratiche.Include("stato").Include("note").Where(p => p.id == id).FirstOrDefault();

            if (model.pratica == null)
            {
                return HttpNotFound();
            }

            model.listaStatiSuccessivi = new mediatori.Controllers.Business.CQS.StatoBusiness().getStatiSuccessivi(model.pratica.stato, db);

            return View(model);
        }


        [HttpGet]
        public ActionResult PraticaPartialById(int id, EnumTipoAzione tipoAzione = EnumTipoAzione.MODIFICA)
        {
            Pratica pratica;
            pratica = db.Pratiche.Where(p => p.id == id).FirstOrDefault();
            if (pratica == null)
            {
                return HttpNotFound();
            }

            if (tipoAzione == EnumTipoAzione.MODIFICA)
            {
                //valorizzaViewBag(indirizzo);

                return View("_PraticaEdit", pratica);
            }

            if (tipoAzione == EnumTipoAzione.VISUALIZZAZIONE)
            {
                return View("_PraticaPartialDetail", pratica);
            }

            //  valorizzaViewBag();
            //return View("impiegoPartialEdit", impiego);
            throw new ApplicationException("Azione di inserimento che non si deve presentare");
        }





        [HttpPost]
        public ActionResult Update(Pratica pratica)
        {
            Debug.WriteLine("ID: " + pratica.id);

            Pratica praticaOriginale;
          //  praticaOriginale = db.p

            praticaOriginale = db.Pratiche.Include("cedente").Where(p => p.id == pratica.id).FirstOrDefault();

            if (praticaOriginale == null)
            {
                return HttpNotFound();
            }

            praticaOriginale.dataDecorrenza = pratica.dataDecorrenza;
            praticaOriginale.dataRinnovo = pratica.dataRinnovo;
         
            try
            {
                db.SaveChanges();

                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, "Salvataggio concluso con successo");
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                string temp;
                temp = MyHelper.getDbEntityValidationException(ex);

                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, temp);
            }
            catch (Exception ex)
            {
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Failed, ex.Message);
            }

            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        public ActionResult GetListPraticheContatto(int id)
        {
            ICollection<Pratica> listPratiche = db.Pratiche.Include("stato").Include("prodottoRichiesto").Where(s => s.contattoId == id).ToList();
            return View("ListaPratiche", listPratiche);
        }

    }
}