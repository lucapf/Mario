using mediatori.Controllers.CQS;
using mediatori.Models;
using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Controllers.Business.Anagrafiche.Soggetto
{
    public static class IndirizzoBusiness
    {
        public static Indirizzo findIndirizzo(int id, MainDbContext db)
        {
            return (from ind in db.Indirizzi.Include("tipoIndirizzo").Include("toponimo")
                                            .Include("provincia").Include("comune")
                    where ind.id == id
                    select ind).First();
        }
        public static Indirizzo valorizzaDatiPerInserimentoCancellazione(Indirizzo indirizzo, MainDbContext db)
        {
            indirizzo.tipoIndirizzo = (from ti in db.TipoIndirizzo
                                       where ti.id == indirizzo.tipoIndirizzo.id
                                       select ti).First();
            indirizzo.toponimo = (from t in db.Toponimi where t.sigla == indirizzo.toponimo.sigla select t).First();
            indirizzo.provincia = (from p in db.Province
                                   where p.denominazione == indirizzo.provincia.denominazione
                                   select p).First();
            indirizzo.comune = (from c in db.Comuni
                                where c.denominazione == indirizzo.comune.denominazione &&
                                      c.provincia.id == indirizzo.provincia.id
                                select c).First();
            return indirizzo;

        }
        public static Indirizzo save(String username, Indirizzo indirizzo, MainDbContext db)
        {
            indirizzo = valorizzaDatiPerInserimentoCancellazione(indirizzo, db);
            Indirizzo indirizzoCorrente = findIndirizzo(indirizzo.id, db);
            LogEventi le = LogEventiManager.getEventoForUpdate(username, indirizzoCorrente.id, EnumEntitaRiferimento.INDIRIZZO, indirizzoCorrente, indirizzo);
            indirizzoCorrente = (Indirizzo)CopyObject.simpleCompy(indirizzoCorrente, indirizzo);
            LogEventiManager.save(le, db);
            db.SaveChanges();
            return indirizzoCorrente;
          
        }
        public static Indirizzo create(String username, Indirizzo indirizzo, MainDbContext db)
        {
            indirizzo = valorizzaDatiPerInserimentoCancellazione(indirizzo, db);
            db.Indirizzi.Add(indirizzo);
            LogEventi le =  LogEventiManager.getEventoForCreate(username, indirizzo.id, EnumEntitaRiferimento.INDIRIZZO);
            LogEventiManager.save(le, db);
            db.SaveChanges();
            return indirizzo;
        }
        public static Indirizzo createByCedente(String username, int codiceCedente, Indirizzo indirizzo, MainDbContext db)
        {
            indirizzo = valorizzaDatiPerInserimentoCancellazione(indirizzo, db);
            Cedente cedente = RicercaCedenteBusiness.find(codiceCedente,db);
            cedente.indirizzi.Add(indirizzo);
            LogEventi le = LogEventiManager.getEventoForCreate(username, indirizzo.id, EnumEntitaRiferimento.INDIRIZZO);
            LogEventiManager.save(le, db);
            db.SaveChanges();
            return indirizzo;
        }

    }

}