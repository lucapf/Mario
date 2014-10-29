using mediatori.Models.Anagrafiche;
using mediatori.Models.etc;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace mediatori.Controllers.Business.Anagrafiche
{
    public class AmministrazioneBusiness
    {
        internal static List<Models.Anagrafiche.Amministrazione> findByFilter(Filters.AmministrazioneFilter amministrazioneFilter, Models.MainDbContext db)
        {
            IQueryable<Amministrazione> amministrazioni = db.amministazioni.Include("soggettoGiuridico");
            if (amministrazioneFilter.partitaIva!=null && amministrazioneFilter.partitaIva != String.Empty){
                amministrazioni = amministrazioni.Where(a => a.partitaIva == amministrazioneFilter.partitaIva);
            }
            if (amministrazioneFilter.ragioneSociale != null && amministrazioneFilter.ragioneSociale != String.Empty)
            {
                amministrazioni = amministrazioni.Where(a => a.soggettoGiuridico.ragioneSociale == amministrazioneFilter.ragioneSociale);
            }
            return amministrazioni.ToList();
        }

        internal Amministrazione findByPK(int codiceAmministazione, Models.MainDbContext db)
        {
            AmministrazioneInclude<Amministrazione> amministrazioneInclude = new AmministrazioneInclude<Amministrazione>();
         return amministrazioneInclude.addIncludeStatement(db.amministazioni, "soggettoGiuridico").Where(a =>a.id==codiceAmministazione).FirstOrDefault();
        }
        internal Amministrazione copiaRiferimenti(Amministrazione amministrazioneOriginale, Amministrazione amministrazione, Models.MainDbContext db)
       {
           amministrazioneOriginale.tipoCategoria = db.TipoCategoriaAmministrazione.Find(amministrazione.tipoCategoria.id);
           amministrazioneOriginale.tipoNaturaGiuridica = db.tipoNaturaGiuridica.Find(amministrazione.tipoNaturaGiuridica.id) ;
           amministrazioneOriginale.stato = db.statiSegnalazione.Find(amministrazione.stato.id);
           amministrazioneOriginale.assumibilita = db.TipoAssumibilitaAmministrazione.Find(amministrazione.assumibilita.id) ;
         return amministrazioneOriginale;
       }


        internal static void valorizzaDati(Amministrazione amministrazione,string username ,Models.MainDbContext db)
        {
            amministrazione.assumibilita = db.TipoAssumibilitaAmministrazione.Find(amministrazione.assumibilita.id);
            amministrazione.tipoCategoria = db.TipoCategoriaAmministrazione.Find(amministrazione.tipoCategoria.id);
            amministrazione.tipoNaturaGiuridica = db.tipoNaturaGiuridica.Find(amministrazione.tipoNaturaGiuridica.id);
            amministrazione.stato = db.statiSegnalazione.Find(amministrazione.stato.id);
            amministrazione.soggettoGiuridico = new SoggettoGiuridicoBusiness().completaDati(amministrazione.soggettoGiuridico, username, db);
        }

        internal Amministrazione completaEVerifica(Amministrazione amministrazione, Models.MainDbContext db)
        {
            if (amministrazione.tipoCategoria == null)
            {
                amministrazione.tipoCategoria = new TipoCategoriaAmministrazione();
            }
            else
            {
                amministrazione.tipoCategoria = db.TipoCategoriaAmministrazione.Find(amministrazione.tipoCategoria.id);
            }
            if (amministrazione.tipoNaturaGiuridica == null)
            {
                amministrazione.tipoNaturaGiuridica = new TipoNaturaGiuridica();
            }
            else
            {
                amministrazione.tipoNaturaGiuridica = db.tipoNaturaGiuridica.Find(amministrazione.tipoNaturaGiuridica.id);
            }
            if (amministrazione.stato == null)
            {
                amministrazione.stato = new Stato();
            }
            else
            {
                amministrazione.stato = db.statiSegnalazione.Find(amministrazione.stato.id);
            }

            if (amministrazione.assumibilita == null)
            {
                amministrazione.assumibilita = new TipoAssumibilitaAmministrazione();
            }
            else
            {
                amministrazione.assumibilita = db.TipoAssumibilitaAmministrazione.Find(amministrazione.assumibilita.id);
            }
            return amministrazione;
        }
    }
    public class AmministrazioneInclude<T>
    {


        public DbQuery<T> addIncludeStatement(DbQuery<T> dbQuery, String prefisso)
        {

            prefisso = (prefisso == null || prefisso == String.Empty) ? "" : prefisso + ".";
          //  SoggettoGiuridicoInclude<T> soggettoGiuridicoInclude = new SoggettoGiuridicoInclude<T>();
          // return soggettoGiuridicoInclude.addIncludeStatement(dbQuery,prefisso+"soggettoGiuridico")
               return dbQuery.Include("soggettoGiuridico.riferimenti")
            .Include("soggettoGiuridico.riferimenti.tipoRiferimento")
            .Include("soggettoGiuridico.note")
            .Include("soggettoGiuridico.indirizzi.tipoIndirizzo")
            .Include("soggettoGiuridico.indirizzi.toponimo")
            .Include("soggettoGiuridico.indirizzi.provincia")
            .Include("soggettoGiuridico.indirizzi.comune")
            .Include("tipoNaturaGiuridica").Include("tipoCategoria").Include("assumibilita").Include("stato");
        }
    }
}