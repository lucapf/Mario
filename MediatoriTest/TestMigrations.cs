using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mediatori.Models;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;

namespace mediatori.UnitTest
{
    [TestClass]
    public class TestMigrations
    {
        [TestMethod]
        public void TestMethod1()
        {
            MainDbContext db = new MainDbContext();

          //  System.Data.Entity.ModelConfiguration.Configuration.ConfigurationRegistrar(); 

            //var configuration = new System.Data.Entity.ModelConfiguration.Configuration();
            //configuration.TargetDatabase = new DbConnectionInfo(
            //    "Server=MyServer;Database=MyDatabase;Trusted_Connection=True;",
            //    "System.Data.SqlClient");

            //var migrator = new DbMigrator(configuration);
            //migrator.Update();
        }
    }
}
