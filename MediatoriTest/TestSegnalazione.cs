using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mediatori.Models;
using System.Diagnostics;

namespace mediatori.UnitTest
{
     [TestClass]
    public class TestSegnalazione
    {
         private BusinessModel.SegnalazioneManager manager;


         [TestMethod]
         public void DeleteSegnalazione()
         {

             manager = new BusinessModel.SegnalazioneManager("DefaultConnection");

             int segnalazioneId = 1;
             try
             {
                 manager.openConnection();
                 manager.delete(segnalazioneId);
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


         [TestMethod]
         public void DeleteAllSegnalazioni()
         {
             manager = new BusinessModel.SegnalazioneManager("DefaultConnection");
             try
             {
                 manager.openConnection();
                 manager.deleteAllSegnalazioni();
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
