using mediatori.Controllers.Business;
using mediatori.Controllers.Business.Anagrafiche;
using mediatori.Controllers.CQS;
using mediatori.Filters;
using mediatori.Models;
using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mediatori.Controllers
{
    public class ContattoController : MyBaseController
    {
        //
        // GET: /Contatto/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public String findContattoByNomeCognome(String nome, String cognome)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            ICollection<Contatto> listaContatti = ContattoBusiness.findByFilter(new ContattoFilter() { nome = nome, cognome = cognome }, db);
            return ContattoBusiness.asHtml(listaContatti);
        }
        [HttpGet]
        public String findContattoByCodiceFiscale(String codiceFiscale)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            ICollection<Contatto> listaContatti = ContattoBusiness.findByFilter(new ContattoFilter() { codiceFiscale = codiceFiscale }, db);
            return ContattoBusiness.asHtml(listaContatti);
        }
       
       
    

        public ActionResult contattoPartialById(int id, EnumTipoAzione tipoAzione)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            return dispatch(db.Contatti.Find(id), tipoAzione);
        }
        [ChildActionOnly]
        public ActionResult contattoPartial(Contatto contatto, EnumTipoAzione tipoAzione)
        {
            return dispatch(contatto, tipoAzione);
        }
        [HttpPost]
        public ActionResult Edit(Contatto contatto)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            ContattoBusiness contattoBusiness= new ContattoBusiness();
            Contatto contattoOriginale = contattoBusiness.findByPK(contatto.id,db);
            contatto = contattoBusiness.copiaRiferimenti(contattoOriginale, contatto);
            LogEventi le = LogEventiManager.getEventoForUpdate(User.Identity.Name, contatto.id, EnumEntitaRiferimento.IMPIEGO, contattoOriginale, contatto);
            contattoOriginale = (Contatto)CopyObject.simpleCompy(contattoOriginale, contatto);

            LogEventiManager.save(le, db);
            db.SaveChanges();
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
