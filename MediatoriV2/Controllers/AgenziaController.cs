using BusinessModel.Anagrafiche;
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
using System.Diagnostics;

namespace mediatori.Controllers
{
    public class AgenziaController : MyBaseController
    {
        private AgenziaManager manager = null;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (db != null)
            {
                manager = new AgenziaManager(db.Database.Connection);
            }
        }

        public ActionResult Index(SearchAgenzia model)
        {
            manager.openConnection();
            try
            {
                manager.getList(model);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                manager.closeConnection();
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            AgenziaCreateModel model = new AgenziaCreateModel();
            valorizzaViewBag(db);
            model = valorizzaDatiCreateModel(model, db);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(AgenziaCreateModel model)
        {
            Agenzia agenzia = model.agenzia;

            agenzia.soggettoGiuridico = model.soggettoGiuridico;
            agenzia.soggettoGiuridico.tipoSoggettoGiuridico = EnumEntitaRiferimento.AMMINISTRAZIONE.ToString();

            agenzia.soggettoGiuridico.indirizzi = new List<Indirizzo>();
            agenzia.soggettoGiuridico.indirizzi.Add(model.indirizzo);

            agenzia.soggettoGiuridico.note = new List<Nota>();
            agenzia.soggettoGiuridico.note.Add(model.nota);

            agenzia.soggettoGiuridico.riferimenti = new List<Riferimento>();
            agenzia.soggettoGiuridico.riferimenti.Add(model.riferimento);

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
            valorizzaViewBag(db);
            Agenzia a = AgenziaBusiness.findByPk(id, db);
            return View(a);
        }


        private void valorizzaViewBag(MainDbContext db)
        {
            ViewBag.listaTipoNaturaGiuridica = new SelectList(db.tipoNaturaGiuridica.OrderBy(p => p.descrizione), "id", "Descrizione");
            ViewBag.listaTipoCategoria = new SelectList(db.TipoCategoriaAmministrazione, "id", "Descrizione");
            ViewBag.listaTipoAssumibilita = new SelectList(db.TipoAssumibilitaAmministrazione, "id", "Descrizione");
            //IQueryable<Stato> listaStati = db.StatiSegnalazione.Where(m => m.entitaAssociata == EnumEntitaAssociataStato.AGENZIA);
            //ViewBag.listaStati = new SelectList(listaStati, "id", "descrizione");
            ViewBag.listaTipoAgenzia = new SelectList(db.TipoAgenzia, "id", "Descrizione");

        }


        private AgenziaCreateModel valorizzaDatiCreateModel(AgenziaCreateModel model, MainDbContext db)
        {
            model.agenzia = new Agenzia();
            model.agenzia = AgenziaBusiness.completaEVerifica(model.agenzia, db);

            model.indirizzo = new Indirizzo();
            model.riferimento = new Riferimento();
            model.nota = new Nota();

            model.soggettoGiuridico = new SoggettoGiuridico();

#if DEBUG
            model.soggettoGiuridico.codiceFiscale = "GGGRRR55S66H406B";
            model.soggettoGiuridico.ragioneSociale = "Agenzia di prova";

            model.agenzia.partitaIva = "1111111111111111";
            model.agenzia.rea = "REA";
#endif

            return model;
        }

    }
}
