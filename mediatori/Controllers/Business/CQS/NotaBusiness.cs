using mediatori.Controllers.CQS;
using mediatori.Models;
using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Controllers.Business
{
    public class NotaBusiness
    {
        public ICollection<Nota> valorizzaDatiDefault(ICollection<Nota> listaNote, String operatore)
        {
            if (listaNote != null)
            {
                foreach (Nota nota in listaNote)
                {
                    valorizzaDatiDefault(nota, operatore);
                }
            }
            return listaNote;
        }

        public Nota valorizzaDatiDefault(Nota nota, String operatore)
        {
            if (nota.operatoreInserimento == null)
            {
                nota.operatoreInserimento = operatore;
                nota.dataInserimento = System.DateTime.Now;
            }
            return nota;
        }



        internal static Nota createBySegnalazione(string username, int codiceSegnalazione, Nota nota, MainDbContext db)
        {

            Segnalazione segnalazione = new SegnalazioneBusiness().findByPk(codiceSegnalazione, db);
            if (segnalazione.note == null) segnalazione.note = new List<Nota>();
            segnalazione.note.Add(nota);
            LogEventi le = LogEventiManager.getEventoForCreate(username, nota.id, EnumEntitaRiferimento.NOTA);
            LogEventiManager.save(le, db);
            db.SaveChanges();
            return nota;
        }

        internal static ICollection<Nota> valorizzaInserimento(ICollection<Nota> listaNote, string utenteInserimento)
        {
            if (listaNote == null) return null;
            foreach (Nota n in listaNote)
            {
               valorizzaInserimento(n, utenteInserimento);
            }
            return listaNote;
        }
        internal static Nota valorizzaInserimento(Nota nota, String utente)
        {
            nota.dataInserimento = System.DateTime.Now;
            nota.operatoreInserimento = utente;
            return nota;
        }

        internal static Nota createBySoggettoGiuridico(string username, int codiceSoggettoGiuridico, Nota nota, MainDbContext db)
        {
            SoggettoGiuridico soggettoGiuridico = new SoggettoGiuridicoBusiness().findByPK(codiceSoggettoGiuridico, db);
            if (soggettoGiuridico.note == null) soggettoGiuridico.note = new List<Nota>();
            soggettoGiuridico.note.Add(nota);
            LogEventi le = LogEventiManager.getEventoForCreate(username, nota.id, EnumEntitaRiferimento.NOTA);
            LogEventiManager.save(le, db);
            db.SaveChanges();
            return nota;
        }
    }
}