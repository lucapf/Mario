using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace mediatori.UnitTest
{
    [TestClass]
    public class TestPersonaFisica
    {
        private BusinessModel.Anagrafiche.PersonaFisicaManager  manager;

        [TestMethod]
        public void DeleteAllPersoneFisiche()
        {
            manager = new BusinessModel.Anagrafiche.PersonaFisicaManager("DefaultConnection");
            try
            {
                manager.openConnection();
                manager.deleteAllPersoneFisiche ();
            }
            catch (Exception ex)
            {
                Debug.Write("Exception: " + ex.Message);
            }
            finally
            {
                manager.closeConnection();
            }
        }
    }
}
