using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mediatori.Models;

namespace mediatori.Controllers.CQS
{
     [TestClass]
    public class TestSegnalazione
    {
         [TestMethod]
         public void DeleteSenalazione()
         {
             int segnalazioneId = 2;


             MainDbContext db = new MainDbContext("");

             Models.Anagrafiche.Segnalazione segnalazione;

             segnalazione = db.Segnalazioni.Where( p => p.id == segnalazioneId ).FirstOrDefault();

           

             //if 

         }
    }
}
