using BusinessModel.Log;
using mediatori.Models;
using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace mediatori.Controllers.Business.Anagrafiche.Soggetto
{
    public static class InserimentoCedenteBusiness
    {
        public static Cedente inserisci(Cedente cedente, MainDbContext db, String username)
        {

            cedente.provinciaNascita = (from p in db.Province 
                                        where p.denominazione == cedente.provinciaNascita.denominazione 
                                       select p).FirstOrDefault();
            cedente.comuneNascita = (from com in db.Comuni
                                     where com.provincia.id == cedente.provinciaNascita.id &&
                                         com.denominazione == cedente.comuneNascita.denominazione
                                     select com).FirstOrDefault();
            try
            {
               
                IndirizzoBusiness.valorizzaDatiPerInserimentoCancellazione(cedente.indirizzi, db);
                ImpiegoBusiness.valorizzaDatiImpiego(cedente.impieghi,db);

                db.Cedenti.Add(cedente);
                db.SaveChanges();
                LogEventiManager.save(
                    LogEventiManager.getEventoForCreate(username, cedente.id, EnumEntitaRiferimento.CEDENTE), db);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
               foreach (System.Data.Entity.Validation.DbEntityValidationResult dbevr in ex.EntityValidationErrors)
                {
                    foreach (System.Data.Entity.Validation.DbValidationError ve in dbevr.ValidationErrors)
                    {
                        //logger.Error("errore validazione proprietà : " + ve.PropertyName + " message: " + ve.ErrorMessage);
                    }
                }
            }
            return cedente;
        }
    }
}