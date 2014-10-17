using mediatori.Controllers.Business.Anagrafiche;
using mediatori.Controllers.Business.Anagrafiche.Soggetto;
using mediatori.Controllers.Business.Tipologia;
using mediatori.Controllers.CQS;
using mediatori.Filters;
using mediatori.Models;
using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace mediatori.Controllers.Business
{
    public class SegnalazioneBusiness
    {
        public Segnalazione create(Segnalazione segnalazione, String operatore, MainDbContext db)
        {
            
           // segnalazione = popolaDatiSegnalazione(segnalazione, operatore, db);
            db.Segnalazioni.Add(segnalazione);
            db.SaveChanges();
           
            LogEventiManager.save(
                   LogEventiManager.getEventoForCreate(operatore, segnalazione.id, EnumEntitaRiferimento.CEDENTE), db);
            return segnalazione;

        }

        public Segnalazione popolaDatiSegnalazione(Segnalazione segnalazione, String opreratore, MainDbContext db)
        {
            segnalazione.dataInserimento = System.DateTime.Now;
            segnalazione.utenteInserimento = opreratore;
            segnalazione.note = new NotaBusiness().valorizzaDatiDefault(segnalazione.note, opreratore);
            if (segnalazione.contatto.id != null && segnalazione.contatto.id > 0)
            {
                Contatto contattoOriginale =segnalazione.contatto;
                   segnalazione.contatto = new ContattoBusiness().findByPK(segnalazione.contatto.id,db);
                //CopyObject.copy(segnalazione.contatto,contattoOriginale);
            }
          
            segnalazione.contatto.impieghi = ImpiegoBusiness.valorizzaDatiImpiego(segnalazione.contatto.impieghi, db);
            segnalazione.contatto.riferimenti = RiferimentoBusiness.valorizzaDatiRiferimento(segnalazione.contatto.riferimenti, db);
            segnalazione.altroPrestito = TipoPrestitoBusiness.valorizzaDatiTipologiaPrestito(segnalazione.altroPrestito, db);
            segnalazione.prodottoRichiesto = TipoProdottoBusiness.valorizzaDatiTipoProdotto(segnalazione.prodottoRichiesto, db);
            segnalazione.fontePubblicitaria = FontePubblicitariaBusiness.valorizzaDatiFontePubblicitaria(segnalazione.fontePubblicitaria, db);
            segnalazione.canaleAcquisizione = TipoCanaleAcquisizioneBusiness.valorizzaDatiTipoCanaleAcquisizione(segnalazione.canaleAcquisizione, db);
            segnalazione.tipoLuogoRitrovo = TipoLuogoRitrovoBusiness.valorizzaDatiTipoLuogoRitrovo(segnalazione.tipoLuogoRitrovo, db);
            segnalazione.tipoContatto = TipoContattoBusiness.valorizzaDatiTipoContatto(segnalazione.tipoContatto, db);
            return segnalazione;

        }


        public List<Segnalazione> findByFilter(SegnalazioneSearch segnalazioniSearch, MainDbContext db)
        {
            IQueryable<Segnalazione> listaSegnalazioni = db.Segnalazioni.Include("contatto").Include("prodottoRichiesto") ;
            if (segnalazioniSearch.cognome != null)
            {
                listaSegnalazioni = listaSegnalazioni.Where(s => s.contatto.cognome == segnalazioniSearch.cognome);

            }
            if (segnalazioniSearch.nome != null)
            {
                listaSegnalazioni = listaSegnalazioni.Where(s => s.contatto.nome == segnalazioniSearch.nome);

            }
            if (segnalazioniSearch.dataInserimentoA != null)
            {
                listaSegnalazioni = listaSegnalazioni.Where(s => s.dataInserimento <= segnalazioniSearch.dataInserimentoA);

            }
            if (segnalazioniSearch.dataInserimentoDa != null)
            {
                listaSegnalazioni = listaSegnalazioni.Where(s => s.dataInserimento >= segnalazioniSearch.dataInserimentoDa);

            }
            listaSegnalazioni.OrderByDescending(s => s.id);
            listaSegnalazioni.Take(50);
            List<Segnalazione> segnalazioni = listaSegnalazioni.ToList();
            return segnalazioni;
        }

        internal Segnalazione findByPk(int id, MainDbContext db)
        {
            ContattoBusiness contattoBusiness = new ContattoBusiness();
            ContattoInclude<Segnalazione> contattoInclude = new ContattoInclude<Segnalazione>();
            DbQuery<Segnalazione> segnalazioneQuery = contattoInclude
                .addIncludeStatement(db.Segnalazioni, "contatto").Include("note").Include("stato") ;
                  segnalazioneQuery.Include("preventivi").Include("preventivi.finanziaria")
                .Include("preventivi.assicurazioneVita").Include("preventivi.assicurazioneImpiego")
                .Include("documenti").Include("documenti.tipoDocumento");
            Segnalazione segnalazione = segnalazioneQuery.Where(s => s.id == id).FirstOrDefault(); ;
            if (segnalazione.preventivi == null) segnalazione.preventivi = new List<Preventivo>();
            return segnalazione;
        }
    }
}