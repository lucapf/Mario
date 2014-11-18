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
            int segnalazioneId = 6;

            MainDbContext db = new MainDbContext();
            //mediatori.Controllers.Business.SegnalazioneBusiness manager = new Business.SegnalazioneBusiness();
            //mediatori.Models.Anagrafiche.Segnalazione s = manager.findByPk(segnalazioneId, db);


            mediatori.Models.Anagrafiche.Segnalazione s = db.Segnalazioni.Where<Models.Anagrafiche.Segnalazione >(p => p.id == segnalazioneId).FirstOrDefault() ;

            if (s == null)
            {
                Assert.Fail("Segnalazione is null");
            }



            if (s.preventivi == null)
            {
                s.preventivi = new List<PreventivoSmall>();
            }

            Models.Preventivo preventivo;
            preventivo = new Preventivo();



            preventivo.progressivo = s.preventivi.Count() + 1;
            preventivo.durata = 3;
            preventivo.nomeProdotto = "Prodotto XXX";

            int idAssicurazioneVita = 1;
            int idAssicurazioneImpiego = 1;
            int idFinanziari = 1;

            preventivo.assicurazioneVita = db.SoggettiGiuridici.Where(aa => aa.id == idAssicurazioneVita).FirstOrDefault();
            preventivo.assicurazioneImpiego = db.SoggettiGiuridici.Where(aa => aa.id == idAssicurazioneImpiego).FirstOrDefault();
            preventivo.finanziaria = db.SoggettiGiuridici.Where(aa => aa.id == idFinanziari).FirstOrDefault();


            db.Preventivi.Add(preventivo);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }


        }
    }
}
