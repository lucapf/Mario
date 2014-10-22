using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mediatori.Models;
using System.Diagnostics;

namespace mediatori.Controllers.CQS
{

     [TestClass]
    public class TestPreventivo
    {

         [TestMethod]
         public void InsertPreventivo()
         {
             int segnalazioneId = 2;

             mediatori.Controllers.Business.SegnalazioneBusiness manager = new Business.SegnalazioneBusiness();

             MainDbContext db = new MainDbContext("");

             mediatori.Models.Anagrafiche.Segnalazione s = manager.findByPk(segnalazioneId, db);

             if (s == null)
             {
                 Assert.Fail("Segnalazione is null");
             }

             Models.Preventivo preventivo;
             preventivo = new Preventivo();




             preventivo.durata = 3;
             int idAssicurazioneVita = 1;
             int idAssicurazioneImpiego = 1;
             int idFinanziari = 1;


             preventivo.assicurazioneVita = db.SoggettiGiuridici.Where(aa => aa.id == idAssicurazioneVita).FirstOrDefault();
             preventivo.assicurazioneImpiego = db.SoggettiGiuridici.Where(aa => aa.id == idAssicurazioneImpiego).FirstOrDefault();
             preventivo.finanziaria = db.SoggettiGiuridici.Where(aa => aa.id == idFinanziari).FirstOrDefault();           


             db.preventivi.Add(preventivo);

             db.SaveChanges();

           


         }
    }
}
