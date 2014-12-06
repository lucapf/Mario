using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BusinessModel.SimulazioneFinanziaria
{
    public class SimulazioneManager
    {
        private PccWS.PccWSClient _PccServiceReference;


        public SimulazioneManager(string urlWebService)
        {
            _PccServiceReference = new PccWS.PccWSClient(new System.ServiceModel.BasicHttpBinding(), new System.ServiceModel.EndpointAddress(urlWebService));
        }

        public SimulazioneManager(string urlWebService, System.IO.FileInfo certificato)
        {
            _PccServiceReference = new PccWS.PccWSClient(new System.ServiceModel.BasicHttpsBinding(), new System.ServiceModel.EndpointAddress(urlWebService));


            System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                        System.Security.Cryptography.X509Certificates.X509Chain chain,
                        System.Net.Security.SslPolicyErrors sslPolicyErrors)
                            {
                                return true; // **** Always accept
                            };


            System.Net.ServicePointManager.Expect100Continue = true;
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3;
            //ServicePointManager.CertificatePolicy = new TrustHBSCertificatePolicy();


            //System.Security.Cryptography.X509Certificates.X509Certificate2 cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(certificato.FullName);

            System.Security.Cryptography.X509Certificates.X509Certificate cert = System.Security.Cryptography.X509Certificates.X509Certificate.CreateFromCertFile(certificato.FullName);


            System.Security.Cryptography.X509Certificates.X509Certificate2 cert2 = new System.Security.Cryptography.X509Certificates.X509Certificate2(cert);
            bool esito;
            esito = cert2.Verify();
            Debug.WriteLine("Verify: " + esito);



            _PccServiceReference.ClientCredentials.ClientCertificate.Certificate = cert2;


        }


        public string getVersion()
        {

            System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                     System.Security.Cryptography.X509Certificates.X509Chain chain,
                     System.Net.Security.SslPolicyErrors sslPolicyErrors)
            {
                return true; // **** Always accept
            };


            System.Net.ServicePointManager.Expect100Continue = true;
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3;
          

            string temp = "";
            try
            {
                temp = _PccServiceReference.getVersioneProgetto();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
            }


            return temp;
        }

    }
}
