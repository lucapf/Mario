using mediatori.Models;
using mediatori.Models.etc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Controllers.Business.CQS
{
    public class StatoBusiness
    {

        public List<Stato> getStatiSuccessivi(int statoId, MainDbContext db)
        {
            Stato stato = db.StatiSegnalazione.Find(statoId);
            return getStatiSuccessivi(stato, db);
        }

        public List<Stato> getStatiSuccessivi(Stato statoCorrente, MainDbContext db)
        {
            List<Stato> listaStati = db.StatiSegnalazione.Where(s => s.entitaAssociata == statoCorrente.entitaAssociata).Where(s => s.id != statoCorrente.id).OrderBy(s => s.descrizione).ToList();
            return listaStati;
        }

        //public List<Stato> findByFilter(StatoSearch statoFilter, MainDbContext db) {

        //    IQueryable<Stato> querystato = db.StatiSegnalazione;
        //    if (statoFilter == null) { return querystato.ToList(); }
        //    if (statoFilter.entita != null) {
        //        querystato = querystato.Where(s => s.entitaAssociata == statoFilter.entita);
        //    }
        //    if (statoFilter.codiceStato != null)
        //    {
        //        querystato = querystato.Where(s => s.id == statoFilter.codiceStato);
        //    }

        //    if (statoFilter.precedentiDi != null) { 
        //        querystato = querystato.Where(s => s.id != statoFilter.codiceStato);
        //    }
        //    //if (statoFilter.successiviDi!= null)
        //    //{
        //    //   querystato = querystato.Where(s => s.id ==statoFilter.codiceStato);
        //    //}
        //    return querystato.OrderBy(s => s.descrizione).ToList();
        //}
    }
}