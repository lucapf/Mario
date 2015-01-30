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
using BusinessModel.Anagrafiche.Agenzia;
using mediatori.Controllers.Business;

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
        public ActionResult AgenziaPartialById(int id, EnumTipoAzione tipoAzione)
        {
            Agenzia agenzia = new AgenziaBusiness().findByPk(id, db);

            if (agenzia == null)
            {
                return HttpNotFound();
            }

            if (tipoAzione == EnumTipoAzione.MODIFICA)
            {
                valorizzaViewBag();

                return View("AgenziaEdit", agenzia);
            }

            if (tipoAzione == EnumTipoAzione.VISUALIZZAZIONE)
            {
                return View("AgenziaPartialDetail", agenzia);
            }

            throw new ApplicationException("Azione di inserimento che non si deve presentare");

        }


        [HttpGet]
        public ActionResult Create()
        {
            AgenziaCreateModel model = new AgenziaCreateModel();
            valorizzaViewBag();
            model = valorizzaDatiCreateModel(model, db);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(AgenziaCreateModel model)
        {
            //Verifico la presenza di un'agenzia con lo stresso nome
            List<SoggettoGiuridico> check;
            check = manager.getSoggettoGiuridicoByRagioneSociale(model.soggettoGiuridico.ragioneSociale);
            if (check != null)
            {
                valorizzaViewBag();
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Warning, "Attenzione, esiste già un'agenzia con lo stessa ragione sociale");
                return View(model);
            }

            check = manager.getSoggettoGiuridicoByCF(model.soggettoGiuridico.codiceFiscale);
            if (check != null)
            {
                valorizzaViewBag();
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Warning, "Attenzione, esiste già un'agenzia con lo stesso codice fiscale");
                return View(model);
            }


            List<Agenzia > check_2;
            check_2 = manager.getAgenziaByPIVA(model.agenzia.partitaIva);
            if (check_2 != null)
            {
                valorizzaViewBag();
                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Warning, "Attenzione, esiste già un'agenzia con la stessa partita iva");
                return View(model);
            }
            
            Agenzia agenzia;
            agenzia = model.agenzia;

            agenzia.soggettoGiuridico = model.soggettoGiuridico;
            agenzia.soggettoGiuridico.tipoSoggettoGiuridico = EnumEntitaRiferimento.AGENZIA.ToString();

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
            valorizzaViewBag();
            Agenzia a = new AgenziaBusiness().findByPk(id, db);
            return View(a);
        }


        private void valorizzaViewBag()
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
            model.agenzia.isEnabled = true;
            model.indirizzo = IndirizzoBusiness.valorizzaDatiDefault(new Indirizzo());
            model.riferimento = RiferimentoBusiness.valorizzaDatiDefault(new Riferimento());
            model.nota = new Nota();

            model.soggettoGiuridico = new SoggettoGiuridico();

#if DEBUG
            model.soggettoGiuridico.codiceFiscale = "GGGRRR55S66H406B";
            model.soggettoGiuridico.ragioneSociale = "Agenzia di prova";

            model.agenzia.partitaIva = "11111111111";
            model.agenzia.rea = "REA";
#endif

            return model;
        }


        [HttpPost]
        public ActionResult Edit(Agenzia agenzia)
        {
            AgenziaBusiness agenziaBusiness = new AgenziaBusiness();
            Agenzia agenziaOriginale = agenziaBusiness.findByPk(agenzia.id, db);

            if (agenziaOriginale == null)
            {
                return HttpNotFound();
            }


            agenziaOriginale.partitaIva = agenzia.partitaIva;
            agenziaOriginale.codiceOam = agenzia.codiceOam;
            agenziaOriginale.codiceRui = agenzia.codiceRui;
            agenziaOriginale.dataFineMandato = agenzia.dataFineMandato;
            agenziaOriginale.dataInizioMandato = agenzia.dataInizioMandato;
            agenziaOriginale.dataOam = agenzia.dataOam;
            agenziaOriginale.documentoPagamento = agenzia.documentoPagamento;
            agenziaOriginale.rea = agenzia.rea;

            agenziaOriginale.tipoAgenzia = db.TipoAgenzia.Find(agenzia.tipoAgenzia.id);
            agenziaOriginale.tipoNaturaGiuridica = db.tipoNaturaGiuridica.Find(agenzia.tipoNaturaGiuridica.id);


            agenziaOriginale.soggettoGiuridico.codiceFiscale = agenzia.soggettoGiuridico.codiceFiscale;
            agenziaOriginale.soggettoGiuridico.ragioneSociale = agenzia.soggettoGiuridico.ragioneSociale;
            agenziaOriginale.isEnabled = agenzia.isEnabled;

            ModelState.Clear();

            TryValidateModel(agenziaOriginale);
            if (ModelState.IsValid)
            {
                db.SaveChanges();
            }

            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }


    }
}
