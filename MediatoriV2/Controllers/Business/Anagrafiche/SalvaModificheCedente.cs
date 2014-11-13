using mediatori.Controllers.CQS;
using mediatori.Models;
using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Controllers.Business.Anagrafiche.Soggetto
{
    public static class SalvaModificheCedente
    {
        public static Cedente salvaModificheDatiGenerali(String username,Cedente cedente, MainDbContext db)
        {
            cedente.provinciaNascita = (from p in db.Province
                                        where p.denominazione == cedente.provinciaNascita.denominazione
                                        select p).First();
            cedente.comuneNascita = (from c in db.Comuni
                                     where c.denominazione == cedente.comuneNascita.denominazione &&
                                           c.provincia.id == cedente.provinciaNascita.id
                                     select c).First();
            Cedente cedenteOriginale = (from ced in db.Cedenti.Include("impieghi").Include("documentoIdentita").Include("indirizzi")
                                        where ced.id == cedente.id
                                        select ced).FirstOrDefault();
            cedente.impieghi = cedenteOriginale.impieghi;
            cedente.indirizzi = cedenteOriginale.indirizzi;
            cedente.documentiIdentita = cedenteOriginale.documentiIdentita;
            LogEventi le = LogEventiManager.getEventoForUpdate(username, cedente.id, EnumEntitaRiferimento.CEDENTE, cedenteOriginale, cedente);
            cedenteOriginale = (Cedente)CopyObject.simpleCompy(cedenteOriginale, cedente);
              LogEventiManager.save(le, db);
            db.SaveChanges();
            return cedenteOriginale;
        }
    }
}