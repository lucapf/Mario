using mediatori.Controllers.Business.Anagrafiche;
using mediatori.Controllers.Business.Anagrafiche.Soggetto;
using mediatori.Models;
using mediatori.Models.Anagrafiche;
using mediatori.Models.etc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mediatori.Controllers
{
    public class AgenziaController : MyBaseController
    {
        //
        // GET: /Agenzia/
        [HttpGet]
        public ActionResult Index(AgenziaFilter agenziaFilter)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);

            return View(AgenziaBusiness.findByFilter(agenziaFilter, db));
        }


        public ActionResult IndexV2(AgenziaFilter agenziaFilter)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);

            return View(AgenziaBusiness.findByFilter(agenziaFilter, db));
        }

        [HttpGet]
        public ActionResult Create()
        {
            var agenziaCreate = new AgenziaCreate();
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            valorizzaViewBag(db);
            agenziaCreate = valorizzaDatiAgenzia(agenziaCreate, db);
            return View(agenziaCreate);
        }

        [HttpPost]
        public ActionResult Create(AgenziaCreate agCreate)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);

            Agenzia agenzia = agCreate.agenzia;
            agenzia.soggettoGiuridico = agCreate.soggettoGiuridico;
            agenzia.soggettoGiuridico.tipoSoggettoGiuridico = "AMMINISTRAZIONE";
            agenzia.soggettoGiuridico.indirizzi = agCreate.indirizzi;
            agenzia.soggettoGiuridico.note = agCreate.note;
          
            AgenziaBusiness.valorizzaDati(agenzia, User.Identity.Name, db);
            ModelState.Clear();
            TryValidateModel(agenzia);
            if (ModelState.IsValid)
            {
                db.Agenzia.Add(agenzia);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            valorizzaViewBag(db);
            Agenzia a = AgenziaBusiness.findByPk(id, db);
            return View(a);
        }


        private void valorizzaViewBag(MainDbContext db)
        {
            ViewBag.listaTipoNaturaGiuridica = new SelectList(db.tipoNaturaGiuridica.OrderBy(p => p.descrizione), "id", "Descrizione");
            ViewBag.listaTipoCategoria = new SelectList(db.TipoCategoriaAmministrazione, "id", "Descrizione");
            ViewBag.listaTipoAssumibilita = new SelectList(db.TipoAssumibilitaAmministrazione, "id", "Descrizione");
            IQueryable<Stato> listaStati = db.statiSegnalazione.Where(m => m.entitaAssociata == EnumEntitaAssociataStato.AGENZIA);
            ViewBag.listaStati = new SelectList(listaStati, "id", "descrizione");
            ViewBag.listaTipoAgenzia = new SelectList(db.TipoAgenzia, "id", "Descrizione");

        }


        private AgenziaCreate valorizzaDatiAgenzia(AgenziaCreate agenziaCreate, MainDbContext db)
        {
            agenziaCreate.agenzia = new Agenzia();
            agenziaCreate.agenzia.soggettoGiuridico = new SoggettoGiuridico();
            agenziaCreate.agenzia = AgenziaBusiness.completaEVerifica(agenziaCreate.agenzia, db);
            agenziaCreate.indirizzi = new List<Indirizzo>();
            agenziaCreate.indirizzi.Add(IndirizzoBusiness.valorizzaDatiDefault(new Indirizzo()));
            agenziaCreate.riferimenti = new List<Riferimento>();
            agenziaCreate.riferimenti.Add(RiferimentoBusiness.valorizzaDatiDefault(new Riferimento()));
            agenziaCreate.note = new List<Nota>();
            agenziaCreate.note.Add(new Nota());

            return agenziaCreate;
        }

    }
}
