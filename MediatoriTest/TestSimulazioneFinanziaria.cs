using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace mediatori.UnitTest
{
    [TestClass]
    public class TestSimulazioneFinanziaria
    {

        private string urlSimulazioneFinanziaria = "http://creditolab-atlantide-collaudo.techub.lan:8080/PccWS/PccImpl";

        private string urlSimulazioneFinanziariaSSL = "https://creditolab-atlantide-collaudo.techub.it/PccWS/PccImpl";

        [TestMethod]
        public void TestWebService()
        {
            string temp;
            BusinessModel.SimulazioneFinanziaria.SimulazioneManager manager = new BusinessModel.SimulazioneFinanziaria.SimulazioneManager(null,urlSimulazioneFinanziaria,"techubadmin","");

            temp = manager.getVersion();

            Debug.WriteLine(temp);

        }



        [TestMethod]
        public void TestWebServiceSSL()
        {

            //The CRT file contains the SSL certificate that was returned by the CA
            string path = @"..\..\..\MediatoriV2\Content\creditolab-techub-roberto.rutigliano.p12";

          //  path = @"..\..\..\MediatoriV2\Content\ca-server-creditolab.crt";


            
            System.IO.FileInfo certificato = new System.IO.FileInfo(path);
            if (!certificato.Exists)
            {
                Assert.Fail("Certificato non trovato");
            }


            BusinessModel.SimulazioneFinanziaria.SimulazioneManager manager = new BusinessModel.SimulazioneFinanziaria.SimulazioneManager(null,urlSimulazioneFinanziariaSSL,"techubadmin","");
            string temp;
            temp = manager.getVersion();

            Debug.WriteLine(temp);
        }
    }
}
