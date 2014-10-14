using mediatori.Controllers.CQS;
using mediatori.Models;
using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Controllers.Business.Anagrafiche.Soggetto
{
    public class RiferimentoBusiness
    {
        public static  ICollection<Riferimento> valorizzaDatiRiferimento(ICollection<Riferimento> listaRiferimenti, MainDbContext db)
        {
            if (listaRiferimenti != null)
            {
                foreach (Riferimento r in listaRiferimenti)
                {
                    valorizzaDatiRiferimento(r, db);
                }
            }
            return listaRiferimenti;
        }

        public static Riferimento valorizzaDatiRiferimento(Riferimento r, MainDbContext db)
        {
          
                r.tipoRiferimento = db.TipoRiferimento.Find(r.tipoRiferimento.id);
                return r;
                  
        }

       
        internal static Riferimento findByPk(int id, MainDbContext db)
        {
            return db.Riferimento.Include("tipoRiferimento").Where(r => r.id == id).FirstOrDefault();
        }



        internal static Riferimento save(string username, Riferimento riferimento, MainDbContext db)
        {
            Riferimento riferimentoCorrente = findByPk(riferimento.id, db);
            LogEventi le = LogEventiManager.getEventoForUpdate(username, riferimentoCorrente.id, EnumEntitaRiferimento.RIFERIMENTO, riferimentoCorrente, riferimento);
            riferimentoCorrente = (Riferimento)CopyObject.simpleCompy(riferimentoCorrente, riferimento);
            LogEventiManager.save(le, db);
            db.SaveChanges();
            return riferimentoCorrente;
        }

        internal static Riferimento valorizzaDatiDefault(Riferimento riferimento)
        {
            riferimento.tipoRiferimento = new TipoRiferimento();
            return riferimento;
        }
    }
}