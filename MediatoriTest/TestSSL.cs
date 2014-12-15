using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace mediatori.UnitTest
{
    [TestClass]
    public class TestSSL
    {
        [TestMethod]
        public void ConvertCrtToPfx()
        {

            // The path to the certificate.
            string certificate;
            certificate = @"C:\Users\Roberto\Desktop\ca-server-creditolab.crt";

            //certificate = @"C:\Users\Roberto\Desktop\roberto.cer";
            // Load the certificate into an X509Certificate object.
            System.Security.Cryptography.X509Certificates.X509Certificate cert = new System.Security.Cryptography.X509Certificates.X509Certificate(certificate);
            byte[] certData = cert.Export(System.Security.Cryptography.X509Certificates.X509ContentType.Pfx, "admin");

            System.IO.File.WriteAllBytes(@"C:\Users\Roberto\Desktop\ca-server-creditolab.pfx", certData);
            //System.IO.File.WriteAllBytes(@"C:\Users\Roberto\Desktop\cer.pfx", certData);
        }




    }
}
