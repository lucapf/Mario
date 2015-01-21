using mediatori.Models;
using mediatori.Models.Anagrafiche;
using mediatori.Models.etc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace mediatori.Controllers.Business.Anagrafiche
{
    public class AgenziaBusiness
    {
        public static List<Agenzia> findByFilter(Agenzia filtro, MainDbContext db)
        {
            DbQuery<Agenzia> dbSetAgenzie = db.Agenzia.Include("soggettoGiuridico");
            if (filtro == null)
            {
                return dbSetAgenzie.ToList();
            }
            if (filtro.partitaIva != String.Empty)
            {
                dbSetAgenzie.Where(a => a.partitaIva == filtro.partitaIva);
            }
            if (filtro.soggettoGiuridico.ragioneSociale != String.Empty)
            {
                dbSetAgenzie.Where(a => a.soggettoGiuridico.ragioneSociale == filtro.soggettoGiuridico.ragioneSociale);
            }
            return dbSetAgenzie.ToList();
        }

        public static Agenzia completaEVerifica(Agenzia agenzia, MainDbContext db)
        {
            agenzia = new Agenzia();
            agenzia.soggettoGiuridico = new SoggettoGiuridico();
            //agenzia.stato = new Stato();
            agenzia.tipoAgenzia = new TipoAgenzia();
            agenzia.tipoNaturaGiuridica = new TipoNaturaGiuridica();
            return agenzia;

        }

        internal static void valorizzaDati(Agenzia agenzia, string username, MainDbContext db)
        {
          
            agenzia.tipoNaturaGiuridica = db.tipoNaturaGiuridica.Find(agenzia.tipoNaturaGiuridica.id);

            //if (agenzia.stato != null)
            //{
            //    agenzia.stato = db.StatiSegnalazione.Find(agenzia.stato.id);
            //}
            
            agenzia.tipoAgenzia = db.TipoAgenzia.Find(agenzia.tipoAgenzia.id);
            agenzia.tipoNaturaGiuridica = db.tipoNaturaGiuridica.Find(agenzia.tipoNaturaGiuridica.id);
            agenzia.soggettoGiuridico = new SoggettoGiuridicoBusiness().completaDati(agenzia.soggettoGiuridico, username, db);
        }



        internal  Agenzia findByPk(int codiceAgenzia, MainDbContext db)
        {
            SoggettoGiuridicoInclude<Agenzia> soggettoInclude = new SoggettoGiuridicoInclude<Agenzia>();
            return soggettoInclude.addIncludeStatement(db.Agenzia, "soggettoGiuridico")
                .Include("tipoNaturaGiuridica")
                .Include("tipoAgenzia").Where(a => a.id == codiceAgenzia).FirstOrDefault();
        }
    }
}
