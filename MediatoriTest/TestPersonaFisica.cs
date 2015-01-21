using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace mediatori.UnitTest
{
    [TestClass]
    public class TestPersonaFisica
    {
        private BusinessModel.Anagrafiche.PersonaFisica.PersonaFisicaManager manager;

        [TestMethod]
        public void DeleteAllPersoneFisiche()
        {
            int conta;

            manager = new BusinessModel.Anagrafiche.PersonaFisica.PersonaFisicaManager("DefaultConnection");
            try
            {
                manager.openConnection();
                conta = manager.deleteAllPersoneFisiche();

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
