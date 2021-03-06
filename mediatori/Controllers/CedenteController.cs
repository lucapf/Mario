﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mediatori.Models.Anagrafiche;
using mediatori.Models;
using mediatori.Controllers.Business.Anagrafiche.Soggetto;
using mediatori.Controllers.Business;

namespace mediatori.Controllers.CQS
{
    public class CedenteController : Controller
    {
    

        //
        // GET: /Cedente/

        public ActionResult Index()
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            return View(db.Cedenti.ToList());
        }

        //
        // GET: /Cedente/Details/5

        public ActionResult Details(int id = 0)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            Cedente cedente = RicercaCedenteBusiness.find(id, db);
            if (cedente == null)
            {
                return HttpNotFound();
            }
            return View(cedente);
        }


 
        //
        // GET: /Cedente/Create

        public ActionResult Create()
        {

            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            Cedente cedente = new Cedente();
            valorizzaListe(db);

            cedente.indirizzi = new List<Indirizzo>();
            cedente.indirizzi.Add(new Indirizzo());

            cedente.impieghi = new List<Impiego>();
            cedente.impieghi.Add(new Impiego());

            cedente.documentoIdentita = new List<DocumentoIdentita>();
            cedente.documentoIdentita.Add(new DocumentoIdentita());
           
            
            
            return View(cedente);
            
            
        }

        private void valorizzaListe(MainDbContext db)
        {
            ViewBag.listaTipoIndirizzo = new SelectList(db.TipoIndirizzo.ToList(),"id","descrizione");
            ViewBag.listaToponimo = new SelectList(db.Toponimi.ToList(), "sigla", "sigla");
            //ViewBag.listaProvincia = new SelectList(db.Province.ToList(), "sigla", "denominazione");
            ViewBag.listaStatoCivile = DecodeStatoCivile.getSelectListValues();
            ViewBag.listaSesso = DecodeSesso.getSelectListItems(); ;

            List<SelectListItem> lsli = new List<SelectListItem>();
            lsli.Add(new SelectListItem { Text = "", Value = "" });
            ViewBag.listaProvincia = new SelectList(db.Province.ToList(), "denominazione", "denominazione");
            ViewBag.listaComuniNascita = lsli;
        }

        //
        // POST: /Cedente/Create

        [HttpPost]
        public ActionResult Create(Cedente cedente)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            
            //elimino i componenti che non devono essere controllati
            ModelState.Remove("provinciaNascita.sigla");
            //concurrentModificationException
            List<String> lstKeys = new List<string>();
            foreach (String key in ModelState.Keys) lstKeys.Add(key);
            foreach (String key in lstKeys)
            {
                if ( key.EndsWith("].provincia.sigla")
                  || key.EndsWith("].tipoIndirizzo.descrizione")
                  || key.EndsWith("].provinciaEnte.sigla")
                  || key.EndsWith("].enteRilascio.descrizione")
                  || key.EndsWith("].tipoImpiego.descrizione")) ModelState.Remove(key);

            }
            if (ModelState.IsValid)
            {   
                Cedente cedenteSalvato = InserimentoCedenteBusiness.inserisci(cedente, db, User.Identity.Name);
                //List<SelectListItem> sl = new List<SelectListItem>();
                //ViewBag.listaProvincia = new SelectList(db.Province.ToList(), "denominazione", "denominazione");
                //ViewBag.listaComuniNascita = new SelectList((from c in db.Comuni where c.provincia.id==cedenteSalvato.provinciaNascita.id select c).ToList(), "denominazione", "denominazione");
                ViewBag.message = String.Format("Cedente {0} {1} inserito con successo", cedenteSalvato.nome, cedenteSalvato.cognome);
                return View("Details",  cedenteSalvato);
            }
            valorizzaListe(db);
            return View(cedente);
        }

        //
        // GET: /Cedente/Edit/5

        public ActionResult Edit(int id = 0)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            Cedente cedente = RicercaCedenteBusiness.find(id, db);
            if (cedente == null)
            {
                return HttpNotFound();
            }
            return View(cedente);
        }

        //
        // POST: /Cedente/Edit/5

        [HttpPost]
        public ActionResult Edit(Cedente cedente)
        {
            ModelState.Remove("Indirizzi");
            ModelState.Remove("documentoIdentita");
            ModelState.Remove("impieghi");
            ModelState.Remove("provinciaNascita.sigla");
            if (ModelState.IsValid)
            {
                MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
                Cedente cedentesalvato = SalvaModificheCedente.salvaModificheDatiGenerali(User.Identity.Name, cedente, db);
                return RedirectToAction("Index");
            }
            return View(cedente);
        }

        //
        // GET: /Cedente/Delete/5

        public ActionResult Delete(int id = 0)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            Cedente cedente = RicercaCedenteBusiness.find(id, db);
            if (cedente == null)
            {
                return HttpNotFound();
            }
            return View(cedente);
        }

        //
        // POST: /Cedente/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            Cedente cedente = RicercaCedenteBusiness.find(id, db);
           // db.Database.ExecuteSqlCommand("delete from indirizzo where Cedente_id=" + id);
           // db.Database.ExecuteSqlCommand("delete from documento_identita where Cedente_id=" + id);
           // db.Database.ExecuteSqlCommand("delete from impiego where Cedente_id=" + id);
            db.Cedenti.Remove(cedente);

            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [ChildActionOnly]
        public ActionResult DatiGeneraliPartialDetails(Cedente cedente)
        {
            return View(cedente);
        }
        [ChildActionOnly]
        public ActionResult DatiGeneraliPartialEdit(Cedente cedente)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            ViewBag.listaProvincia = new SelectList(db.Province.ToList(), "denominazione", "denominazione");
            ViewBag.listaComuniNascita = new SelectList((from c in db.Comuni where c.provincia.id==cedente.provinciaNascita.id select c).ToList(), "denominazione", "denominazione");
                
            return View(cedente);
        }
     
        public ActionResult DatiGeneraliPartialById(int id, EnumTipoAzione tipoAzione=EnumTipoAzione.MODIFICA)
        { 

            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            Cedente cedente = RicercaCedenteBusiness.findDatiGenerali(id, db);
           if (tipoAzione == EnumTipoAzione.MODIFICA)
            {
                valorizzaListe(db);
               // ViewBag.listaProvincia = new SelectList(db.Province.ToList(), "denominazione", "denominazione");
                ViewBag.listaComuniNascita = new SelectList((from c in db.Comuni where c.provincia.id == cedente.provinciaNascita.id select c).ToList(), "denominazione", "denominazione");
                return View("DatiGeneraliPartialInsert", cedente);
            }
           else if (tipoAzione == EnumTipoAzione.INSERIMENTO)
           {
               return View("DatiGeneraliPartialDetails", cedente);
           }else
            {
                return View("DatiGeneraliPartialDetails", cedente);
            }
        }

        protected override void Dispose(bool disposing)
        {
            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}