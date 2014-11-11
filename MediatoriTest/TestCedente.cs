using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mediatori.Models;
using mediatori.Models.Anagrafiche;
using mediatori.Controllers.Business.Anagrafiche.Soggetto;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
namespace mediatori.Controllers.CQS
{
    [TestClass]
    public class TestCedente
    {
            public static String URLTEST = "http://TestDatabaseConnection.prova.it";
            public Cedente ValorizzaDatiGeneraliCedente()
            {
                Cedente cedente = new Cedente();
                cedente.nome = Guid.NewGuid().ToString();
                cedente.cognome = Guid.NewGuid().ToString();
                cedente.codiceFiscale = "PLNLCU73D30H282W";
                cedente.dataNascita = new DateTime(1973, 04, 30);
                cedente.provinciaNascita = new Provincia { denominazione = "Roma" };
                cedente.comuneNascita = new Comune { denominazione = "Roma" };
                return cedente;

            }
        public Cedente valorizzaDatiCedente()
        {
            Cedente cedente = new Cedente();
            cedente.nome =Guid.NewGuid().ToString();
            cedente.cognome = Guid.NewGuid().ToString();
            cedente.codiceFiscale = "PLNLCU73D30H282W";
            cedente.dataNascita = new DateTime(1973, 04, 30);
            cedente.provinciaNascita = new Provincia { denominazione = "Roma" };
            cedente.comuneNascita = new Comune { denominazione = "Roma" };
            cedente.documentoIdentita = new List<DocumentoIdentita>();
            cedente.documentoIdentita.Add(new DocumentoIdentita
            {
                comuneEnte = new Comune { denominazione = "Amatrice" },
                dataRilascio = new DateTime(2010, 01, 01),
                dataScadenza = new DateTime(2012, 01, 01),
                enteRilascio = new TipoEnteRilascio { id = 1 },
                numeroDocumento = "AA1122",
                provinciaEnte = new Provincia { denominazione = "Rieti" }
            });
            cedente.impieghi = new List<Impiego>();
            cedente.impieghi.Add(new Impiego
            {
                dataAssunzione = new DateTime(2010, 01, 01),
                mansione = "impiegato",
                mensilita = 12,
                stipendioNettoMensile = 1200,
               tipoImpiego = new TipoContrattoImpiego { id = 1 }
            });

            cedente.indirizzi = new List<Indirizzo>();
            cedente.indirizzi.Add(new Indirizzo
            {
                cap = "00100",
                comune = new Comune { denominazione = "Amatrice" },
                corrispondenza = true,
                id = 0,
                recapito = "strada",
                interno = "A",
                numeroCivico = "12",
                presso = "presso",
                provincia = new Provincia { denominazione = "Rieti" },
                tipoIndirizzo = new TipologiaIndirizzo { id = 1 },
                toponimo = new Toponimo { sigla = "Via" }
            });
            return cedente;
        }
        [TestMethod]
        public void testCreaCedente() {
            MainDbContext db = new MainDbContext(URLTEST);
            Cedente cedenteDaCaricare = valorizzaDatiCedente();
            Cedente cedenteCaricato  = InserimentoCedenteBusiness.inserisci(cedenteDaCaricare, db, "userTest");
            Assert.IsTrue(cedenteCaricato.id > 0, "cedente non inserito correttamente");
            Cedente cedenteRecuperato = (from c in db.Cedenti 
                                            where c.nome==cedenteDaCaricare.nome && c.cognome == cedenteDaCaricare.cognome 
                                           select c).FirstOrDefault();
            Assert.IsNotNull(cedenteRecuperato, "impossibile recuperare il cedente memorizzato");

        }
        // test ricerca dettaglio cedente il seguente test richiede implicitamente che il test di creazione sia eseguito 
        // correttamente in quanto inserisce il cedente prima di ricercarlo.
        [TestMethod]
        public void testCedenteDetail() {
            MainDbContext db = new MainDbContext(URLTEST);
            Cedente cedenteMemorizzato = InserimentoCedenteBusiness.inserisci(valorizzaDatiCedente(), db, "userTest") ;
            Assert.IsTrue(cedenteMemorizzato.id > 0, "cedente non memorizzato correttamente");
            Cedente cedenteRecuperato = RicercaCedenteBusiness.find(cedenteMemorizzato.id, db);
            Assert.AreEqual(cedenteMemorizzato.provinciaNascita.id, cedenteRecuperato.provinciaNascita.id, "proprietà provincia nascita non recuperata correttamente");
            Assert.AreEqual(cedenteMemorizzato.comuneNascita.id, cedenteRecuperato.comuneNascita.id, "proprietà comune nascita non recuperata correttamente");

        }
        [TestMethod]
        public void testSalvaModificheDatiGeneraliCedente()
        {
            MainDbContext db = new MainDbContext(URLTEST);
            int codiceCedenteDaModificare= db.Cedenti.ToList().ElementAt(2).id;
            Cedente cedente = ValorizzaDatiGeneraliCedente();
            cedente.id = codiceCedenteDaModificare;
            String nome = cedente.nome;
            String cognome = cedente.cognome;
            Cedente cedenteSalvato = SalvaModificheCedente.salvaModificheDatiGenerali("userTest", cedente, db);
            Assert.AreEqual(nome, cedenteSalvato.nome,"nome non salvato correttamente");
            Assert.AreEqual(cognome, cedenteSalvato.cognome, "cognome non salvoato correttamente");

        }

    }
       
}
