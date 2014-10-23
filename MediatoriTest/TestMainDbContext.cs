using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

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

                tables = db.Database.SqlQuery<string>("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'").ToList();

                if (tables.Count > 0)
                {
                    DropTables(db, tables);
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
    }
}
