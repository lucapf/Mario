using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mediatori.Controllers.CQS;
using mediatori.Models.Anagrafiche;
using mediatori.Models;
using System.Collections.Generic;

namespace mediatori.Controllers.CQS
{

    [TestClass]
    public class LogEventiManagerTest
    {
        private static String TESTURL = "http://TestDatabaseConnection.prova.it";
        [TestMethod]
        public void getEventoForCreate()
        {
            String operatore = "test";
            LogEventi le = LogEventiManager.getEventoForCreate(operatore, 10, EnumEntitaRiferimento.CEDENTE);
            Assert.AreEqual(le.operatoreInserimento, operatore, "operatore non riconosciuto");
            Assert.AreEqual(le.tipoEntitaRiferimento, EnumEntitaRiferimento.CEDENTE, "tipoEntita riconosciuta");
            Assert.AreEqual(le.tipoEvento, EnumTipoEventoLog.INSERIMENTO, "azione non riconosciuta");
        }
        [TestMethod]
        public void getEventoForUpdate()
        {
            String operatore = "test";
            Cedente cedente = new Cedente { nome = "nome", cognome = "cognome", dataNascita = new DateTime(1973, 04, 30) };
            Cedente cedeteModificato = new Cedente { nome = "nome", cognome = "cognomeModificato", dataNascita = new DateTime(1973, 04, 30) };
            LogEventi le = LogEventiManager.getEventoForUpdate(operatore, 10, EnumEntitaRiferimento.CEDENTE, cedente, cedeteModificato);
            Assert.AreEqual(le.messaggio, ";cognome:cognome:cognomeModificato");
            Assert.AreEqual(le.tipoEntitaRiferimento, EnumEntitaRiferimento.CEDENTE, "tipoEntita riconosciuta");
            Assert.AreEqual(le.tipoEvento, EnumTipoEventoLog.AGGIORNAMENTO, "azione non riconosciuta");
        }
        [TestMethod]
        public void SaveEvento()
        {
            String operatore = "test";
            MainDbContext mdbContext=new MainDbContext(TESTURL);
            String message = Guid.NewGuid().ToString();
            int idEntita = 10;
            LogEventi le = LogEventiManager.getEventoForCreate(operatore, idEntita, EnumEntitaRiferimento.CEDENTE);
            le.messaggio = message;
            LogEventiManager.save(le, mdbContext);
            try
            {
                LogEventiFilter lef = new LogEventiFilter {  messaggioEsatto=message };
                List<LogEventi> lle = LogEventiManager.findByFilter(idEntita, EnumEntitaRiferimento.CEDENTE, lef, mdbContext);
                Assert.IsNotNull(lle, "evento non salvato correttamente");
                Assert.IsTrue(lle.Count == 1, "evento salvato più di una volta");
                Assert.AreEqual(lle.ToArray()[0].messaggio, message, "l'evento trovato sembra diverso da quello ricercato");
            }
            finally
            {
                //ripetibilità Evento: cancello l'evento log appena creato.
                LogEventiManager.delete(le, mdbContext);
            }
        }
        [TestMethod]
        public void getHistory()
        {
            String operatore = "test";
            MainDbContext mdbContext = new MainDbContext(TESTURL);
            String message = Guid.NewGuid().ToString();    
            LogEventi le = LogEventiManager.getEventoForCreate(operatore, 10, EnumEntitaRiferimento.CEDENTE);
            le.messaggio = message;
            LogEventiManager.save(le, mdbContext);
            History history = LogEventiManager.getIdentityHistory(10, EnumEntitaRiferimento.CEDENTE, mdbContext);
            Assert.AreEqual(history.entitaRiferimento, EnumEntitaRiferimento.CEDENTE);
            Assert.IsNotNull(history.listaEventi);
            Assert.IsTrue(history.listaEventi.Count > 0);
            LogEventiManager.delete(le, mdbContext);

        }
        [TestMethod]
        public void getEventoCreazione()
        {
            String operatore = "test";
            MainDbContext mdbContext = new MainDbContext(TESTURL);
            String message = Guid.NewGuid().ToString();
            int codiceEntita = new Random().Next();
            LogEventi le = LogEventiManager.getEventoForCreate(operatore, codiceEntita, EnumEntitaRiferimento.CEDENTE);
            le.messaggio = message;
            LogEventiManager.save(le, mdbContext);

            LogEventi le2 = LogEventiManager.getEventoForCreate(operatore, codiceEntita, EnumEntitaRiferimento.CEDENTE);
            le2.messaggio = Guid.NewGuid().ToString();
            LogEventiManager.save(le2, mdbContext);
            LogEventi le3 = LogEventiManager.getEventoCreazione(codiceEntita, EnumEntitaRiferimento.CEDENTE, mdbContext);
            Assert.IsNotNull(le3);
            Assert.AreEqual(le3.tipoEntitaRiferimento, EnumEntitaRiferimento.CEDENTE);
            Assert.AreEqual(le3.id, le.id);
            Assert.AreEqual(le3.messaggio, message);
            LogEventiManager.delete(le, mdbContext);
            LogEventiManager.delete(le2, mdbContext);

        }

    }
}
