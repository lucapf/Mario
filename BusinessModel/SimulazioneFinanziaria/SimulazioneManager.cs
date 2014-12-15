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
            if (urlWebService.StartsWith("http://"))
            {
                _PccServiceReference = new PccWS.PccWSClient(new System.ServiceModel.BasicHttpBinding(), new System.ServiceModel.EndpointAddress(urlWebService));

            }
            else if (urlWebService.StartsWith("https://"))
            {
                System.ServiceModel.BasicHttpsBinding binding = new System.ServiceModel.BasicHttpsBinding();
                binding.Security.Mode = System.ServiceModel.BasicHttpsSecurityMode.Transport;
                //binding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.Certificate;

                _PccServiceReference = new PccWS.PccWSClient(binding, new System.ServiceModel.EndpointAddress(urlWebService));


                //System.Security.Cryptography.X509Certificates.X509Certificate2 x509 = null;
                //System.Security.Cryptography.X509Certificates.X509Store store = new System.Security.Cryptography.X509Certificates.X509Store(System.Security.Cryptography.X509Certificates.StoreName.My, System.Security.Cryptography.X509Certificates.StoreLocation.CurrentUser);
                //store.Open(System.Security.Cryptography.X509Certificates.OpenFlags.ReadOnly);

                //foreach (System.Security.Cryptography.X509Certificates.X509Certificate2 cert in store.Certificates)
                //{
                //    Debug.WriteLine(String.Format ("Friendly Name: {0}", cert.FriendlyName));
                //    Debug.WriteLine(String.Format ("Serial Number: {0}", cert.GetSerialNumberString()));
                //    Debug.WriteLine(String.Format ("Thumbprint: {0}", cert.Thumbprint));
                //    Debug.WriteLine(String.Format ("Simple Name: {0}", cert.GetNameInfo(System.Security.Cryptography.X509Certificates.X509NameType.SimpleName, true)));
                //    Debug.WriteLine("");
                //}

               
                //try
                //{
                //    System.Security.Cryptography.X509Certificates.X509Certificate2Collection risultato;

                    
                //    //risultato = store.Certificates.Find(System.Security.Cryptography.X509Certificates.X509FindType.FindBySerialNumber, "‎128C", false);
                //    //if (risultato != null && risultato.Count > 0)
                //    //{
                //    //    x509 = risultato[0];
                //    //}


                //    //risultato = store.Certificates.Find(System.Security.Cryptography.X509Certificates.X509FindType.FindBySubjectKeyIdentifier, "‎‎E05A09A9003A35148FEFA8B8D5874B1FB8EDB445", false);
                //    //if (risultato != null && risultato.Count > 0)
                //    //{
                //    //    x509 = risultato[0];
                //    //}


                //    //risultato = store.Certificates.Find(System.Security.Cryptography.X509Certificates.X509FindType.FindBySubjectName, "creditolab-techub-roberto.rutigliano", false);
                //    //if (risultato != null && risultato.Count > 0)
                //    //{
                //    //    x509 = risultato[0];
                //    //}

                //    //                    ‎12 8c

                //    risultato = store.Certificates.Find(System.Security.Cryptography.X509Certificates.X509FindType.FindByThumbprint, "e0 5a 09 a9 00 3a 35 14 8f ef a8 b8 d5 87 4b 1f b8 ed b4 45", false);
                //    if (risultato != null && risultato.Count > 0)
                //    {
                //        x509 = risultato[0];
                //    }

                //    //risultato = store.Certificates.Find(System.Security.Cryptography.X509Certificates.X509FindType.FindByThumbprint, "‎e05a09a9003a35148fefa8b8d5874b1fb8edb445", false);
                //    //if (risultato != null && risultato.Count > 0)
                //    //{
                //    //    x509 = risultato[0];
                //    //}

                //    //risultato = store.Certificates.Find(System.Security.Cryptography.X509Certificates.X509FindType.FindByThumbprint, "‎‎E05A09A9003A35148FEFA8B8D5874B1FB8EDB445", false);
                //    //if (risultato != null && risultato.Count > 0)
                //    //{
                //    //    x509 = risultato[0];
                //    //}

                //}
                //catch (Exception ex)
                //{
                //    Debug.WriteLine(ex.Message);
                //    throw new MyManagerCSharp.MyException("SimulazioneManager", ex);
                //}
                //finally
                //{
                //    store.Close();
                //}


                //if (x509 == null)
                //{
                //    throw new MyManagerCSharp.MyException("Certificato NON trovato");
                //}

                ////byte[] rawdata = x509.RawData;
                ////   Debug.WriteLine("Content Type: {0}{1}", X509Certificate2.GetCertContentType(rawdata), Environment.NewLine);
                //Debug.WriteLine(String.Format( "Friendly Name: {0}", x509.FriendlyName));
                //Debug.WriteLine(String.Format("Serial Number: {0}", x509.GetSerialNumberString()));

                //Debug.WriteLine(String.Format("Certificate Verified?: {0}", x509.Verify()));
                //Debug.WriteLine(String.Format("Simple Name: {0}", x509.GetNameInfo(System.Security.Cryptography.X509Certificates.X509NameType.SimpleName, true)));

                //Debug.WriteLine(String.Format("Signature Algorithm: {0}", x509.SignatureAlgorithm.FriendlyName));
                //Debug.WriteLine(String.Format("Private Key: {0}", x509.PrivateKey.ToXmlString(false)));
                //Debug.WriteLine(String.Format("Public Key: {0}", x509.PublicKey.Key.ToXmlString(false)));
                //Debug.WriteLine(String.Format("Certificate Archived?: {0}", x509.Archived));
                //Debug.WriteLine(String.Format("Length of Raw Data: {0}", x509.RawData.Length));
                                    
                //_PccServiceReference.ClientCredentials.ClientCertificate.Certificate = x509;
            }
            else
            {
                throw new MyManagerCSharp.MyException("endpoint errato: " + urlWebService);
            }


        }


        public void close()
        {
            if (_PccServiceReference != null)
            {
                _PccServiceReference.Close();
                _PccServiceReference = null;
            }
        }


        public string getVersion()
        {

            System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                     System.Security.Cryptography.X509Certificates.X509Chain chain,
                     System.Net.Security.SslPolicyErrors sslPolicyErrors)
            {
                return true; // **** Always accept
            };

            //System.Net.ServicePointManager.Expect100Continue = true;
            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3;

            string temp = "";
            try
            {
                temp = _PccServiceReference.getVersioneProgetto();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
                throw new MyManagerCSharp.MyException("getVersion", ex);
            }

            return temp;
        }

    }
}
