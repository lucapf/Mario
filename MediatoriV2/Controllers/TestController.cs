using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace mediatori.Controllers
{
    public class TestController : MyBaseController
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Noty()
        {
            return View();
        }

        public ActionResult Notification()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Notification(string subject)
        {

            TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, "Account CreditoLab configurato con successo");
            return View();
        }

        public ActionResult RGraph01()
        {
            return View();
        }


        public ActionResult RGraph02()
        {
            return View();
        }

        public ActionResult RGraph03()
        {
            return View();
        }

        public ActionResult TableResize()
        {
            List<Models.Test.TestClass> model = new List<Models.Test.TestClass>();
            Models.Test.TestClass item;

            for (int i = 0; i < 20; i++)
            {
                item = new Models.Test.TestClass();
                item.CodiceId = i;
                item.Nome = "Nome " + i;
                item.importoRichiesto = i * 1000;
                item.dataDiNascita = DateTime.Now.AddDays(-i);
                item.isEnabled = true;


                model.Add(item);
            }

            return View(model);
        }

        public ActionResult FlipSwitch(Models.Test.TestClass model)
        {
            return View(model);
        }

        [HttpPost]
        [ActionName("FlipSwitch")]
        public ActionResult FlipSwitchPost(Models.Test.TestClass model)
        {
            Debug.WriteLine("IsEnabled: " + model.isEnabled);

            return View(model);
        }



        #region "___ SIMULAZIONE FINANZIARIA ___"

        public ActionResult Simulazione(mediatori.Models.Test.SimulazioneModel model)
        {

            mediatori.SessionData MySessionData = (Session["MySessionData"] as SessionData);

            if (String.IsNullOrEmpty(model.url))
            {
                //  model.url = "https://creditolab-atlantide.techub.it:20443/PccWS/PccImpl";

                model.url = MySessionData.Istituto.url;

            }

            if (MySessionData.CredenzialiCreditoLab != null)
            {
                model.login = MySessionData.CredenzialiCreditoLab.Login;
                model.login = MySessionData.CredenzialiCreditoLab.Password;
            }



            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("GetVersion")]
        public ActionResult SimulazioneGetVersion(mediatori.Models.Test.SimulazioneModel model)
        {
            BusinessModel.SimulazioneFinanziaria.SimulazioneManager manager = null;
            try
            {
                string temp;
                manager = new BusinessModel.SimulazioneFinanziaria.SimulazioneManager(null, model.url, "techubadmin", "");

                temp = manager.getVersion();

                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, "Connessione stabilita con successso: " + temp);
            }
            catch (Exception ex)
            {
                TempData["Message"] = new MyMessage(ex);
            }
            finally
            {
                if (manager != null)
                {
                    manager.close();
                }
            }

            return View("Simulazione", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Login")]
        public ActionResult SimulazioneLoginPost(mediatori.Models.Test.SimulazioneModel model)
        {
            BusinessModel.SimulazioneFinanziaria.SimulazioneManager manager = null;
            try
            {
                string temp;
                manager = new BusinessModel.SimulazioneFinanziaria.SimulazioneManager(null, model.url, model.login, model.password);

                temp = manager.getVersion();

                TempData["Message"] = new MyMessage(MyMessage.MyMessageType.Success, "Autenticazione eseguita con successso: " + temp);
            }
            catch (Exception ex)
            {
                TempData["Message"] = new MyMessage(ex);
            }
            finally
            {
                if (manager != null)
                {
                    manager.close();
                }
            }

            return View("Simulazione", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Agenzie")]
        public ActionResult SimulazioneGetAgenzie(mediatori.Models.Test.SimulazioneModel model)
        {
            BusinessModel.SimulazioneFinanziaria.SimulazioneManager manager = null;
            try
            {

                manager = new BusinessModel.SimulazioneFinanziaria.SimulazioneManager(null, model.url, model.login, model.password);

                model.agenzie = manager.getAgenzie();


            }
            catch (Exception ex)
            {
                TempData["Message"] = new MyMessage(ex);
            }
            finally
            {
                if (manager != null)
                {
                    manager.close();
                }
            }

            return View("Simulazione", model);
        }

        #endregion

        [HttpGet]
        public ActionResult Email(MyManagerCSharp.MyObject.MyEmail model)
        {
            model.From = System.Configuration.ConfigurationManager.AppSettings["mail.From"];
            model.Subject = "Subject - TEST -";
            model.Body = "Body - TEST -";

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Email")]
        public ActionResult EmailPost(MyManagerCSharp.MyObject.MyEmail model)
        {
            Debug.WriteLine("From: " + model.From);

            MyManagerCSharp.MailManager mail = new MyManagerCSharp.MailManager();

            mail._Subject = model.Subject;
            mail._Body = model.Body;

            mail._To(model.To);
            mail._Cc(model.Cc);
            mail._Bcc(model.Bcc);

            string esito;
            esito = mail.send();

            if (String.IsNullOrEmpty(esito))
            {
                esito = "OK";
            }

            ViewBag.Esito = esito;

            return View(model);
        }






        public ActionResult CSS01()
        {
            mediatori.Models.Test.TestClass model = new Models.Test.TestClass();
            model.CodiceId = 7;
            model.Nome = "Roberto";
            //List <MyManagerCSharp.Models.MyItem> lista= new List<MyManagerCSharp.Models.MyItem> ();
            //lista.Add(new MyManagerCSharp.Models.MyItem ("1", "Valore 1")) ;
            //lista.Add(new MyManagerCSharp.Models.MyItem ("2","Valore 2"));

            List<SelectListItem> lista = new List<SelectListItem>();
            lista.Add(new SelectListItem() { Text = "Valore 1", Value = "1" });
            lista.Add(new SelectListItem() { Text = "Valore con descrizione 2", Value = "2" });
            lista.Add(new SelectListItem() { Text = "Valore con descrizione lunga ", Value = "3" });
            lista.Add(new SelectListItem() { Text = "Valore con descrizione lunga lunga lunga lunga ", Value = "4" });

            ViewBag.listaTipo = lista;

            return View(model);
        }

        public ActionResult Calendar()
        {
            return View();
        }


        public ActionResult CheckBox()
        {
            return View();
        }

        public ActionResult Home()
        {
            return View();
        }

        public ActionResult Validation01()
        {
            mediatori.Models.Test.TestClass model = new Models.Test.TestClass();
            model.CodiceId = 7;
            return View(model);
        }


        public ActionResult Number()
        {
            return View();
        }

        public ActionResult Button()
        {
            return View();
        }

        public ActionResult Decimal(mediatori.Models.Test.Decimal model)
        {
            model.decimal_01 = (decimal)1.5;
            model.decimal_02 = (decimal)3.3;


            model.double_01 = 1.5;
            model.double_02 = 3.3;

            return View(model);
        }

        [HttpPost]
        [ActionName("Decimal")]
        public ActionResult DecimalPost(mediatori.Models.Test.Decimal model)
        {

            Debug.WriteLine(String.Format("decimal_01: {0}", model.decimal_01));
            Debug.WriteLine(String.Format("decimal_02: {0}", model.decimal_02));


            Debug.WriteLine(String.Format("double_01: {0}", model.double_01));
            Debug.WriteLine(String.Format("double_02: {0}", model.double_02));

            return View(model);
        }



        public ActionResult Azure(Models.BlobContainer model)
        {
            //http://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-how-to-use-blobs/#configure-access

            Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(System.Configuration.ConfigurationManager.ConnectionStrings["AzureStorageConnection"].ConnectionString);

            Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container. 
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobClient.GetContainerReference("mycontainer");

            // Create the container if it doesn't already exist.
            container.CreateIfNotExists();


            foreach (Microsoft.WindowsAzure.Storage.Blob.IListBlobItem item in container.ListBlobs(null, false))
            {
                if (item.GetType() == typeof(Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob))
                {
                    Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob blob = (Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob)item;

                    Console.WriteLine("Block blob of length {0}: {1}", blob.Properties.Length, blob.Uri);

                    model.Blobs.Add(blob);
                }

            }


            if (Request.IsAjaxRequest())
            {

                return PartialView("_BlobList", model.Blobs);
            }

            return View(model);
        }


        public ActionResult DhtmlxScheduler()
        {
            return View();
        }

        public ActionResult Stefania()
        {
            return View();
        }
    }
}
