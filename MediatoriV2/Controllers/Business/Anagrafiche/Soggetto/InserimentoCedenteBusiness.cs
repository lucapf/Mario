using mediatori.Controllers.CQS;
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
                foreach (DocumentoIdentita di in cedente.documentoIdentita)
                {
                    di.provinciaEnte = (from p in db.Province where p.denominazione == di.provinciaEnte.denominazione select p).FirstOrDefault();
                    di.comuneEnte = (from c in db.Comuni
                                     where c.denominazione == di.comuneEnte.denominazione &&
                                         c.provincia.sigla == di.provinciaEnte.sigla
                                     select c).FirstOrDefault();
                    di.enteRilascio = (from e in db.TipoEnteRilascio where e.id == di.enteRilascio.id select e).FirstOrDefault();
                }
                foreach (Indirizzo i in cedente.indirizzi)
                {
                    i.provincia = (from p in db.Province 
                                    where p.denominazione == i.provincia.denominazione 
                                   select p).FirstOrDefault();
                    i.comune = (from c in db.Comuni
                                where c.denominazione == i.comune.denominazione &&
                                    c.provincia.sigla == i.provincia.sigla
                                select c).FirstOrDefault();
                   
                    i.tipoIndirizzo = (from ti in db.TipoIndirizzo where ti.id == i.tipoIndirizzo.id select ti).FirstOrDefault();
                    i.toponimo = db.Toponimi.Find(i.toponimo.sigla);
                }
                foreach (Impiego i in cedente.impieghi)
                {

                    i.tipoImpiego = db.TipoContrattoImpiego.Find(i.tipoImpiego.id);
                    if (i.dataLicenziamento.Year == 1) i.dataLicenziamento = new DateTime(3000, 01, 01);
                }

                db.Cedenti.Add(cedente);
                db.SaveChanges();
                LogEventiManager.save(
                    LogEventiManager.getEventoForCreate(username, cedente.id, EnumEntitaRiferimento.CEDENTE), db);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                log4net.ILog logger = log4net.LogManager.GetLogger("errorLogger");
                logger.Error(ex.EntityValidationErrors.ToString());
                foreach (System.Data.Entity.Validation.DbEntityValidationResult dbevr in ex.EntityValidationErrors)
                {
                    foreach (System.Data.Entity.Validation.DbValidationError ve in dbevr.ValidationErrors)
                    {
                        logger.Error("errore validazione proprietà : " + ve.PropertyName + " message: " + ve.ErrorMessage);
                    }
                }
            }
            return cedente;
        }
    }
}