using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace mediatori.UnitTest
{
    [TestClass]
    public class TestAgenzia
    {
        private BusinessModel.Anagrafiche.Agenzia.AgenziaManager manager;

        [TestMethod]
        public void DeleteAllAgenzie()
        {
            int conta;

            manager = new BusinessModel.Anagrafiche.Agenzia.AgenziaManager("DefaultConnection");
            try
            {
                manager.openConnection();
                conta = manager.deleteAllAgenzie();

                Debug.WriteLine(String.Format("Sono stati cancellati {0:N0} records", conta));
            }
            catch (Exception ex)
            {
                Debug.Write("Exception: " + ex.Message);
                Assert.Fail();
            }
            finally
            {
                manager.closeConnection();
            }
        }
    }
}
