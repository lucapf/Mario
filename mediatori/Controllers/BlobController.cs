using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace mediatori.Controllers
{
    public class BlobController : Controller
    {


        private Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer getAzureContainer()
        {


            Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(System.Configuration.ConfigurationManager.ConnectionStrings["AzureConnection"].ConnectionString);

            Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container. 
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobClient.GetContainerReference("mycontainer");

            return container;

        }


        [HttpPost]
        public JsonResult Add(HttpPostedFileBase MyFile)
        {
            Debug.WriteLine("Add Blog: " + Request["MyFile"]);

            Models.JsonMessageModel model = new Models.JsonMessageModel();

            if (MyFile == null)
            {
                model.esito = Models.JsonMessageModel.Esito.Failed;
                model.messaggio = "Selezionare un file";
                return Json(model);
            }

            Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = getAzureContainer();

            // Create the container if it doesn't already exist.
            container.CreateIfNotExists();

            Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob blockBlob = container.GetBlockBlobReference(MyFile.FileName);

            try
            {
                blockBlob.UploadFromStream(MyFile.InputStream);
            }
            finally
            {
                MyFile.InputStream.Close();
            }


            model.esito = Models.JsonMessageModel.Esito.Succes;
            model.messaggio = "Operazione conlusa con successo";
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Delete(string id)
        {
            Debug.WriteLine("Blob: " + id);
            Models.JsonMessageModel model = new Models.JsonMessageModel();


            Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = getAzureContainer();
            Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob blockBlob = container.GetBlockBlobReference(id);
            if (blockBlob == null)
            {
                model.esito = Models.JsonMessageModel.Esito.Failed;
                model.messaggio = "Selezionare un file";
                return Json(model);
            }

            blockBlob.Delete();

            model.esito = Models.JsonMessageModel.Esito.Succes;
            model.messaggio = "Operazione conlusa con successo";
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Download(string id)
        {
            Debug.WriteLine("Blob: " + id);

            Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = getAzureContainer();
            Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob blockBlob = container.GetBlockBlobReference(id);
            if (blockBlob == null)
            {
                return HttpNotFound();
            }

            Response.AddHeader("Content-Disposition", "attachment; filename=" + blockBlob.Name); // force download
            Response.AddHeader("Content-Type", System.Web.MimeMapping.GetMimeMapping(blockBlob.Name));
            blockBlob.DownloadToStream(Response.OutputStream);
            return new EmptyResult();
        }



    }
}
