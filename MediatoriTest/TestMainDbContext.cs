using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using mediatori.Models.etc;
using System.Data.Entity.Migrations;

namespace mediatori.Models
{
    [TestClass]
    public class TestMainDbContext
    {
        [TestMethod]
        public void TestGetConnectionByUrl()
        {
            String url = "http://test.localhost.it/Controller/Action";
            String result = MainDbContext.getConnectionByUrl(url);
            Assert.AreEqual(result, "test", String.Format("codifica {0} fallita restituito {1}", url, result));

            url = "https://test.localhost.it/Controller/Action";
            result = MainDbContext.getConnectionByUrl(url);
            Assert.AreEqual(result, "test", String.Format("codifica {0} fallita restituito {1}", url, result));

            url = "http://localhost.it/Controller/Action";
            result = MainDbContext.getConnectionByUrl(url);
            Assert.AreEqual(result, "DefaultConnection", String.Format("codifica {0} fallita restituito {1}", url, result));



        }

        [TestMethod]
        public void DeleteAllTablesInDataBase()
        {
            MainDbContext db = new MainDbContext("");

            //System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseAlways<MainDbContext>());
            // db.Database.Initialize(true);

            List<string> tables;

            for (int i = 0; i < 3; i++)
            {
               // tables = db.GetType().GetProperties().Where(x => x.PropertyType.Name == "DbSet`1").Select(x => x.Name).ToList();
               
                //tables = db.Database.SqlQuery<string>("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'").ToList();

                tables = db.Database.SqlQuery<string>("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' and TABLE_NAME <> '__MigrationHistory'").ToList();
                
                if (tables.Count > 0)
                {
                   //DropTables(db, tables);
                }

            }
        }




        private void DropTables(MainDbContext db, List<string> tables)
        {
            foreach (string tableName in tables)
            {
                Debug.WriteLine(tableName);
                try
                {
                    db.Database.ExecuteSqlCommand("DROP TABLE [" + tableName + "]");
                }
                catch (Exception ex)
                {
                    //ignoro 
                    Debug.WriteLine("Exception: " + ex.Message);
                }
            }
        }



        [TestMethod]
        public void populate()
        {
            MainDbContext context = new MainDbContext("");

             context.statiSegnalazione.AddOrUpdate<Stato>(t => t.id,
                //AMMINISTRAZIONI
                new Stato { id = 1, descrizione = "CENSITA", entitaAssociata = EnumEntitaAssociataStato.AMMINISTRAZIONE, statoBase = EnumStatoBase.ATTIVO },
                new Stato { id = 2, descrizione = "ATTIVA", entitaAssociata = EnumEntitaAssociataStato.AMMINISTRAZIONE, statoBase = EnumStatoBase.ATTIVO },
                new Stato { id = 3, descrizione = "DISATTIVA", entitaAssociata = EnumEntitaAssociataStato.AMMINISTRAZIONE, statoBase = EnumStatoBase.CHIUSO },
                //SEGNALAZIONI
                new Stato { id = 20, descrizione = "Assegnazione ad operatori di telemarketing", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
                new Stato { id = 21, descrizione = "Richiesta preventivo", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
                new Stato { id = 22, descrizione = "Attesa documentazione per analisi", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
                new Stato { id = 23, descrizione = "Analisi in sede", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
                new Stato { id = 24, descrizione = "Proposta in analisi", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
                new Stato { id = 25, descrizione = "Mancato appuntamento", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
                new Stato { id = 26, descrizione = "Proposta analizzata", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
                new Stato { id = 27, descrizione = "Attesa decisione cliente collaboratore", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
                new Stato { id = 28, descrizione = "Incontro in sede per esito positivo", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
                new Stato { id = 29, descrizione = "Raccolta in sede", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
                new Stato { id = 30, descrizione = "Raccolta a domicilio", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
                new Stato { id = 31, descrizione = "Raccolta fax – corrispondenza - collaboratore", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
                new Stato { id = 32, descrizione = "Attesa documenti per avvio istruttoria", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
                new Stato { id = 33, descrizione = "Inoltro documentazione sede centrale per avvio istruttoria", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
                new Stato { id = 34, descrizione = "Avvio istruttoria", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
                new Stato { id = 35, descrizione = "Annullamento", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ANNULLATO },
                new Stato { id = 36, descrizione = "Ripristino per cliente finanziabile", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
                new Stato { id = 37, descrizione = "Ripristino per cliente non finanziabile", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
                new Stato { id = 38, descrizione = "Chiusura", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.CHIUSO}
                );



             context.SaveChanges();
        }


    }
}
