using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace DataModelTest.Clean
{
    [TestClass]
    public class CleanDB
    {
        [TestMethod]
        public void Test()
        {
            BusinessModel.Anagrafiche.Amministrazione.AmministrazioneManager manager = new BusinessModel.Anagrafiche.Amministrazione.AmministrazioneManager("DefaultConnection");

            BusinessModel.Anagrafiche.Amministrazione.SearchAmministrazione model = new BusinessModel.Anagrafiche.Amministrazione.SearchAmministrazione();

            manager.openConnection();

            try
            {
                manager.getList(model);
            }
            finally
            {
                manager.closeConnection();
            }



            foreach (mediatori.Models.Anagrafiche.Amministrazione amministrazione in model.Amministrazioni)
            {
                Debug.WriteLine(amministrazione.partitaIva);
            }

        }

        [TestMethod]
        public void DeleteALL()
        {
            //La tabella delle segnalazioni contiene anche le pratiche!
            //in questo modo stiamo cancellando tutte le pratiche e le segnalazioni

            BusinessModel.Segnalazione.SegnalazioneManager
            manager = new BusinessModel.Segnalazione.SegnalazioneManager("DefaultConnection");
            try
            {
                manager.openConnection();
                manager.deleteAllSegnalazioni();
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


            ////Cancelliamo anche gli utenti tranne ...
            MyUsers.UserManager usersManager = new MyUsers.UserManager("DefaultConnection");
            BusinessModel.Sicurezza.UtentiManager utentiManager = new BusinessModel.Sicurezza.UtentiManager(usersManager.getConnection());
            try
            {
                usersManager.openConnection();
                MyUsers.Models.SearchUsers model = new MyUsers.Models.SearchUsers();

                usersManager.getList(model);

                foreach (MyUsers.Models.MyUser user in model.Utenti)
                {
                    Debug.WriteLine(user.userId + " " + user.login);

                    // if (user.login.StartsWith("admin@") || user.login.StartsWith("amministratore@") || user.login.StartsWith("collaboratore@") || user.login.StartsWith("operatore@"))
                    if (user.login.StartsWith("admin@"))
                    {
                        continue;
                    }

                    utentiManager.delete((long)user.userId);
                }
            }
            catch (Exception ex)
            {
                Debug.Write("Exception: " + ex.Message);
                Assert.Fail();
            }
            finally
            {
                usersManager.closeConnection();
            }


        }


        [TestMethod]
        public void DeleteUtenteById()
        {

            BusinessModel.Sicurezza.UtentiManager manager = new BusinessModel.Sicurezza.UtentiManager("DefaultConnection");
            bool esito = false;
            try
            {
                manager.openConnection();

                esito = manager.delete(9);

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

            if (esito == false)
            {
                Assert.Fail();
            }

        }

    }
}
