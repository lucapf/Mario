using mediatori.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Web;

namespace BusinessModel.Log
{
    public static class LogEventiManager
    {
        public static LogEventi getLogEventi(String username, int codiceEntita, EnumEntitaRiferimento entitaRiferimento, EnumTipoEventoLog tipoEvento)
        {
            return new LogEventi
            {
                dataInserimento = System.DateTime.Now,
                operatoreInserimento = username,
                tipoEvento =tipoEvento,
                idEntita = codiceEntita,
                tipoEntitaRiferimento = entitaRiferimento
            };

        }
        public static LogEventi getEventoForCreate(String username, int codiceEntita, EnumEntitaRiferimento entitaRiferimento)
        {
            return getLogEventi(username, codiceEntita, entitaRiferimento, EnumTipoEventoLog.INSERIMENTO);
        }
        public static LogEventi getEventoForDelete(String username, int codiceEntita, EnumEntitaRiferimento entitaRiferimento)
        {
            return getLogEventi(username, codiceEntita, entitaRiferimento, EnumTipoEventoLog.CANCELLAZIONE);
        }
        public static LogEventi getEventoForUpdate(String username, int codiceEntita, EnumEntitaRiferimento entitaRiferimento, Object originale, object modificato)
        {
            LogEventi le =getLogEventi(username, codiceEntita, entitaRiferimento,EnumTipoEventoLog.AGGIORNAMENTO);
            le.messaggio=cercaDatiModificati(originale, modificato);
            return le;
          
        }
        public static void save(LogEventi le,MainDbContext db){
            db.LogsEventi.Add(le);
            db.SaveChanges();
        }
        public static List<LogEventi> findByFilter(int idEntita, EnumEntitaRiferimento tipoEntitaRiferimento, LogEventiFilter lef, MainDbContext db)
        {
            if (idEntita == 0 || !Enum.IsDefined(typeof(EnumEntitaRiferimento), tipoEntitaRiferimento))
            {
                throw new Exception("impossibile ricercare log Evento se non si imposta il tipo e il codice dell'entità di riferimento!");

            }
            List<LogEventi> allEventiByEntita = (from le in db.LogsEventi 
                                                 where le.idEntita == idEntita 
                                                  && le.tipoEntitaRiferimento==tipoEntitaRiferimento select le).ToList();
            IEnumerable<LogEventi> enumerable=allEventiByEntita;
            if (lef.dataInserimentoA != null && lef.dataInserimentoDa.Year!= 001)
            {
                enumerable= enumerable.Where(le => le.dataInserimento >= lef.dataInserimentoA);
            }
            if (lef.dataInserimentoDa != null & lef.dataInserimentoA.Year!= 001)
            {
                enumerable = enumerable.Where(le => le.dataInserimento <= lef.dataInserimentoDa);
            }
            if (lef.messaggioEsatto != null)
            {
                enumerable = enumerable.Where(le => le.messaggio == lef.messaggioEsatto);
            }
            if (lef.operatoreInserimento != null)
            {
                enumerable = enumerable.Where(le => le.operatoreInserimento == lef.operatoreInserimento);
            }
            return enumerable.ToList();
          //return (from le in db.LogsEventi where

          //        (lef.idEntita==0 || le.idEntita==lef.idEntita)&&
          //        (lef.tipoEvento==null || le.tipoEvento== lef.tipoEvento) &&
          //        (lef.operatoreInserimento==null || le.operatoreInserimento==lef.operatoreInserimento) &&
          //        (lef.messaggioEsatto==null || le.messaggio==lef.messaggioEsatto) &&
          //        (lef.tipoEntitaRiferimento==null || le.tipoEntitaRiferimento==lef.tipoEntitaRiferimento) &&
          //        (lef.dataInserimentoA.Year==0001 || le.dataInserimento<=lef.dataInserimentoA) &&
          //        (lef.dataInserimentoDa.Year==001 || le.dataInserimento>=lef.dataInserimentoDa)
          //        select le).ToList();
        }

        private static string cercaDatiModificati(object originale, object modificato)
        {
            String retString = String.Empty;
            Type tipoOriginale = originale.GetType();
            Type tipoModificato = modificato.GetType();
            PropertyInfo[] propertiesOriginale = tipoOriginale.GetProperties();
            PropertyInfo[] propertiesModificato = tipoModificato.GetProperties();
            foreach (PropertyInfo propertyOriginale in propertiesOriginale)
            {
                PropertyInfo propertyModificato = (from p in propertiesModificato 
                                                   where p.Name == propertyOriginale.Name select p).FirstOrDefault();

                if (propertyModificato == null)
                {
                    continue;
                }
                object valoreOriginale= propertyOriginale.GetValue(originale);
                object valoreModificato = propertyModificato.GetValue(modificato);
                if (valoreOriginale == null && valoreModificato == null) continue;
                if (valoreOriginale==null && valoreModificato != null) 
                      retString = getMessageFromTemplate(retString,propertyOriginale.Name, "null",valoreModificato.ToString());
                if (valoreOriginale!=null && valoreModificato!=null  && ! valoreOriginale.Equals(valoreModificato))
                {
                    retString = getMessageFromTemplate(retString, propertyOriginale.Name, valoreOriginale.ToString(), valoreModificato.ToString());
                }
            }
            return retString;
        }
        private static String getMessageFromTemplate(String message, String propertyName, string originale, string modificato)
        {
            return message += String.Format(";{0}:{1}:{2}", propertyName, originale, modificato);
        }

        public static LogEventiModel getIdentityHistory(int idEntitaRiferimento, EnumEntitaRiferimento enumEntitaRiferimento, MainDbContext mainDbContext )
        {
            LogEventiModel h = new LogEventiModel();
            LogEventiFilter logFilter = new LogEventiFilter();

            h.listaEventi = findByFilter(idEntitaRiferimento,enumEntitaRiferimento, logFilter, mainDbContext);
            h.entitaRiferimento = enumEntitaRiferimento;
            return h;
        }
        public static LogEventi getEventoCreazione(int idEntitaRiferimento, EnumEntitaRiferimento enumEntitaRiferimento, MainDbContext mdbContext)
        {
            LogEventiFilter logFilter= new LogEventiFilter();
            logFilter.tipoEvento=EnumTipoEventoLog.INSERIMENTO;
            List<LogEventi> listaEventi=findByFilter(idEntitaRiferimento,enumEntitaRiferimento,logFilter,mdbContext);
            if (listaEventi.Count > 0)
            {
                listaEventi.OrderByDescending(x => x.dataInserimento);
                return listaEventi.First();
            }
            else
            {
                return null;
            }

        }

        public static void delete(LogEventi le, MainDbContext mdbContext)
        {
            mdbContext.LogsEventi.Remove(le);
        }
    }
}