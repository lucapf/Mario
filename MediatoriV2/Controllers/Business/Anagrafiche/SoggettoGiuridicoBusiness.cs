using mediatori.Controllers.Business.Anagrafiche;
using mediatori.Controllers.Business.Anagrafiche.Soggetto;
using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;

namespace mediatori.Controllers.Business
{
    class SoggettoGiuridicoBusiness
    {
        internal Models.Anagrafiche.SoggettoGiuridico findByPK(int id, Models.MainDbContext db)
        {
            SoggettoGiuridicoInclude<SoggettoGiuridico> soggettoGiuridicoInclude = new SoggettoGiuridicoInclude<SoggettoGiuridico>();
            SoggettoGiuridico soggetto = soggettoGiuridicoInclude.addIncludeStatement(db.SoggettiGiuridici, String.Empty).Where(c => c.id == id).FirstOrDefault();
            return soggetto;
        }

        internal Models.Anagrafiche.SoggettoGiuridico copiaRiferimenti(Models.Anagrafiche.SoggettoGiuridico soggettoOriginale, Models.Anagrafiche.SoggettoGiuridico soggettoGiuridico)
        {
            soggettoOriginale.indirizzi = soggettoGiuridico.indirizzi;
            soggettoOriginale.note = soggettoGiuridico.note;
            soggettoOriginale.riferimenti = soggettoGiuridico.riferimenti;
            return soggettoOriginale;
        }

        internal List<SoggettoGiuridico> findByFilter(Filters.SoggettoGiuridicoSearch soggettoGiuridicoSearch, Models.MainDbContext db)
        {
            IQueryable<SoggettoGiuridico> listaSoggetti = db.SoggettiGiuridici;
            
            if (soggettoGiuridicoSearch.codiceFiscale != null && soggettoGiuridicoSearch.codiceFiscale != String.Empty)
            {
                listaSoggetti=listaSoggetti.Where(s => s.codiceFiscale.Equals(soggettoGiuridicoSearch.codiceFiscale));
            }
            
            if (soggettoGiuridicoSearch.ragioneSociale != null && soggettoGiuridicoSearch.codiceFiscale!= String.Empty){
                listaSoggetti=listaSoggetti.Where(s => s.ragioneSociale.ToUpper().Contains(soggettoGiuridicoSearch.ragioneSociale.ToUpper()));
            }
            if (soggettoGiuridicoSearch.tipoSoggettoGiuridico != null)
            {
                listaSoggetti=listaSoggetti.Where(s=>s.tipoSoggettoGiuridico.Equals(soggettoGiuridicoSearch.tipoSoggettoGiuridico));
            }
            return listaSoggetti.ToList();

        }

        internal SoggettoGiuridico completaDati(SoggettoGiuridico soggettoGiuridico, string username, Models.MainDbContext db)
        {
            soggettoGiuridico.indirizzi = IndirizzoBusiness.completaDatiIndirizzo(soggettoGiuridico.indirizzi, db);
            soggettoGiuridico.riferimenti = RiferimentoBusiness.valorizzaDatiRiferimento(soggettoGiuridico.riferimenti, db);
            soggettoGiuridico.note = NotaBusiness.valorizzaInserimento(soggettoGiuridico.note,username );
            return soggettoGiuridico;
           
        }
    }
    public class SoggettoGiuridicoInclude<T>
    {


        public DbQuery<T> addIncludeStatement(DbQuery<T> dbQuery, String prefisso)
        {

            prefisso = (prefisso == null || prefisso == String.Empty)? "" : prefisso + ".";

            return dbQuery.Include(prefisso + "riferimenti")
            .Include(prefisso + "riferimenti.tipoRiferimento")
            .Include(prefisso + "note")
            .Include(prefisso + "indirizzi.tipoIndirizzo")
            .Include(prefisso + "indirizzi.toponimo")
            .Include(prefisso + "indirizzi.provincia")
            .Include(prefisso + "indirizzi.comune");
        }
    }
}
