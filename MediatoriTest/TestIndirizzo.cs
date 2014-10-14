using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mediatori.Models.Anagrafiche;
using mediatori.Models;
using mediatori.Controllers.Business.Anagrafiche.Soggetto;

namespace mediatori.Controllers.CQS
{
    [TestClass]
    public class TestIndirizzo
    {
        public static String URLTEST = "http://TestDatabaseConnection.prova.it";
        public static String utente = "userTest";
        public Indirizzo getIndirizzo()
        {
            return  new Indirizzo{
                cap= "00100",
                comune = new Comune{denominazione="Roma"},
                corrispondenza=true,
                recapito="indirizzo",
                interno="A",
                numeroCivico="12",
                presso="presso",
                provincia= new Provincia{denominazione="Roma"},
                tipoIndirizzo=new TipologiaIndirizzo{descrizione="Lavoro"},
                toponimo=new Toponimo{sigla="Via"}  
            };
        }
        [TestMethod]
        public void indirizzoSave()
        {
            MainDbContext db = new MainDbContext(URLTEST);
            Indirizzo indirizzo = new Indirizzo() {
                 cap="000", interno="00", numeroCivico="0"
            };
            IndirizzoBusiness.save("test", indirizzo,db);
            Indirizzo indirizzoSalvato =IndirizzoBusiness.save(utente, indirizzo, db);
            Assert.IsNotNull(indirizzoSalvato, "indirizzo non salvato correttamente");
            indirizzoSalvato = db.Indirizzi.Find(indirizzoSalvato.id);
            Assert.AreEqual(indirizzo.recapito, indirizzoSalvato.recapito, "indirizzo non salvato correttamente");
            Assert.AreEqual(indirizzo.cap, indirizzoSalvato.cap, "cap non salvato correttamente");
            Assert.AreEqual(indirizzo.provincia.denominazione, indirizzoSalvato.provincia.denominazione, "provincia non salvato correttamente");
            Assert.AreEqual(indirizzo.comune.denominazione, indirizzoSalvato.comune.denominazione, "comune non salvato correttamente");
            Assert.AreEqual(indirizzo.toponimo.sigla, indirizzoSalvato.toponimo.sigla, "toponimo non salvato correttamente");
            Assert.AreEqual(indirizzo.tipoIndirizzo.descrizione, indirizzoSalvato.tipoIndirizzo.descrizione, "tipo indirizzo non salvato correttamente");
        }
    }
}
