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

                System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding();
                binding.MaxBufferSize = 20000000;
                binding.MaxReceivedMessageSize = 20000000;
                binding.ReaderQuotas.MaxNameTableCharCount = 1638400;
                binding.ReaderQuotas.MaxArrayLength = 1638400;

                _PccServiceReference = new PccWS.PccWSClient(binding, new System.ServiceModel.EndpointAddress(urlWebService));
            }
            else if (urlWebService.StartsWith("https://"))
            {
                System.ServiceModel.BasicHttpsBinding binding = new System.ServiceModel.BasicHttpsBinding();
                binding.MaxBufferSize = 20000000;
                binding.MaxReceivedMessageSize = 20000000;
                binding.ReaderQuotas.MaxNameTableCharCount = 1638400;
                binding.ReaderQuotas.MaxArrayLength = 1638400;

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


        private PccWS.storicoBean getStoricoBean(string operatoreId)
        {
            PccWS.storicoBean bean = new PccWS.storicoBean();
            bean.codiceIstituto = 1;
            bean.codiceIstitutoSpecified = true;
            bean.operatorId = operatoreId;

            return bean;
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



        public List<MyManagerCSharp.Models.MyItem> getAgenzie()
        {
            BusinessModel.PccWS.filtroRicercaAgenzia filtroRicercaAgenzia = new PccWS.filtroRicercaAgenzia();

            BusinessModel.PccWS.anagAgenziaFilialeSmallVO[] listaAgenzie;
            listaAgenzie = _PccServiceReference.findAgenziaFilialeByOperatore(filtroRicercaAgenzia, getStoricoBean("techubadmin"));

            List<MyManagerCSharp.Models.MyItem> risultato;
            risultato = listaAgenzie.OrderBy(p => p.ragioneSociale).Select(p => new MyManagerCSharp.Models.MyItem(p.codice, p.ragioneSociale)).ToList();

            return risultato;
        }

        public List<MyManagerCSharp.Models.MyItem> getProdotti(long agenziaId)
        {
            PccWS.storicoBean bean = getStoricoBean("techubadmin");

            BusinessModel.PccWS.anagAgenziaSmallVO agenzia = _PccServiceReference.findAnagAgenziaSmallByPK(agenziaId, bean);

            if (agenzia == null)
            {
                return null;
            }

            BusinessModel.PccWS.filtroPortafoglio filtro = new PccWS.filtroPortafoglio();
            filtro.anagAgenziaVO = agenzia;

            filtro.usoInterno = false;
            filtro.usoInternoSpecified = true;

            BusinessModel.PccWS.gruppoProdottoSmallVO[] lista = _PccServiceReference.findGruppoProdottoByFiltro(filtro, bean);

            //foreach (BusinessModel.PccWS.gruppoProdottoSmallVO prodotto in lista)
            //{
            //    Debug.WriteLine("Prodotto: " + prodotto.descrizione);
            //}

            List<MyManagerCSharp.Models.MyItem> risultato;
            risultato = lista.OrderBy(p => p.descrizione).Select(p => new MyManagerCSharp.Models.MyItem(p.codice, p.descrizione)).ToList();

            return risultato;
        }



        public void getAllPossiblePortafoglioCombinationFor(SimulazioneModel model)
        {
            PccWS.storicoBean bean = getStoricoBean("techubadmin");

            BusinessModel.PccWS.anagAgenziaSmallVO agenzia = _PccServiceReference.findAnagAgenziaSmallByPK((long)model.agenziaId, bean);
            if (agenzia == null)
            {
                return;
            }

            BusinessModel.PccWS.gruppoProdottoSmallVO prodotto = _PccServiceReference.findGruppoProdottoSmallByPK(model.prodottoId, bean);
            if (prodotto == null)
            {
                return;
            }

            BusinessModel.PccWS.filtroSimulazioneFinanziaria filtro = new PccWS.filtroSimulazioneFinanziaria();
            filtro.simulazioneMultipla = true;
            filtro.simulazioneMultiplaSpecified = true;
            filtro.cittadinanza = model.nazionalita;

            filtro.dataInserimentoPratica = DateTime.Now;
            filtro.dataInserimentoPraticaSpecified = true;

            filtro.dataDecorrenzaPratica = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(1).Month, 1);
            filtro.dataDecorrenzaPraticaSpecified = true;

            filtro.politicaTassiIstFinVO = null;
            filtro.prodottoAssicurativo = "";
            filtro.agenziaVO = agenzia;

            filtro.cedenteVO = new PccWS.personaFisicaVO();
            filtro.cedenteVO.pfSesso = model.sesso;
            filtro.cedenteVO.pfDataNascita = model.dataDiNascita.Value;
            filtro.cedenteVO.pfDataNascitaSpecified = true;

            filtro.durataSpecified = true;
            filtro.durata = (int)model.numeroRate;

            filtro.gruppoProdottoSmallVO = prodotto;

            filtro.importo = model.importoRata.Value;
            filtro.importoSpecified = true;

            filtro.cedenteVO.impiegos = new PccWS.impiegoVO[1];
            filtro.cedenteVO.impiegos[0] = new PccWS.impiegoVO();
            filtro.cedenteVO.impiegos[0].impDataInizio = model.dataAssunzione.Value;
            filtro.cedenteVO.impiegos[0].impDataInizioSpecified = true;

            BusinessModel.PccWS.importiPraticaVO[] lista;

            lista = _PccServiceReference.getAllPossiblePortafoglioCombinationFor(filtro, bean);

            if (lista != null)
            {
                foreach (BusinessModel.PccWS.importiPraticaVO importo in lista)
                {
                    Debug.WriteLine("Importo: " + importo.importoNettoErogato);
                }
            }

        }


    }
}
