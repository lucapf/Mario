using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using mediatori.Models;

namespace mediatori.Controllers
{
    public class DocumentaleController : MyBaseController
    {


        private Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer getAzureContainer()
        {

            Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(System.Configuration.ConfigurationManager.ConnectionStrings["AzureStorageConnection"].ConnectionString);

            Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            // Retrieve a reference to a container. 
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobClient.GetContainerReference("mediatori");

            return container;
        }


        public ActionResult Index(Models.Documentale.DocumentaleIndex model)
        {

            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);

            if (model.SegnalazioneId != null)
            {
                model.documenti = db.Documenti.Where(p => p.SegnalazioneId == (int)model.SegnalazioneId).OrderBy(d => d.nome).ToList();
            }


            model.tipoDocumento = db.TipoDocumenti.OrderBy(p => p.descrizione).ToList();


            if (Request.IsAjaxRequest())
            {
                return PartialView("_DocumentList", model.documenti);
            }

            return View(model);
        }


        //  public JsonResult Add(HttpPostedFileBase MyFile, string descrizione, int tipoDocumentoId, int SegnalazioneId)

        public ActionResult Add(HttpPostedFileBase MyFile, string descrizione, int tipoDocumentoId, int SegnalazioneId)
        {
            Debug.WriteLine("Add file: " + Request["MyFile"]);

            Models.JsonMessageModel model = new Models.JsonMessageModel();
            if (MyFile == null)
            {
                model.esito = JsonMessageModel.Esito.Failed;
                model.messaggio = "Selezionare un file";
                return Json(model);
            }

            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);

            mediatori.Models.etc.Documento documento = new Models.etc.Documento();
            documento.dataInserimento = DateTime.Now;
            documento.descrizione = descrizione;
            //documento.tipoDocumento = new Models.Anagrafiche.TipoDocumento { id = tipoDocumento };
            documento.tipoDocumento = db.TipoDocumenti.Find(tipoDocumentoId);
            documento.SegnalazioneId = SegnalazioneId;
            documento.id = Guid.NewGuid();
            documento.nome = MyFile.FileName;

            ModelState.Clear();
            TryValidateModel(documento);

            if (ModelState.IsValid)
            {

                Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = getAzureContainer();
                try
                {
                    db.Documenti.Add(documento);
                    db.SaveChanges();

                    container.CreateIfNotExists();
                    Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob blockBlob = container.GetBlockBlobReference(documento.id.ToString());
                    blockBlob.UploadFromStream(MyFile.InputStream);


                    model.esito = JsonMessageModel.Esito.Succes;
                    model.messaggio = "Operazione conlusa con successo";
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    model.esito = JsonMessageModel.Esito.Failed;
                    model.messaggio = MyHelper.getDbEntityValidationException(ex);
                }
                finally
                {
                    MyFile.InputStream.Close();
                }

            }
            else
            {
                List<string> errori = ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage + " " + v.Exception).ToList();
                string temp = "";
                foreach (string errore in errori)
                {
                    Debug.WriteLine("Errore: " + errore);
                    temp += errore + Environment.NewLine;
                }

                model.esito = JsonMessageModel.Esito.Failed;
                model.messaggio = temp;
            }



            //return Json(model, JsonRequestBehavior.AllowGet);
            return RedirectToAction("Details", "Segnalazioni", new { id = SegnalazioneId });
        }




      //  public JsonResult Delete(string id)
        public ActionResult Delete(string id, int SegnalazioneId)
        {
            Debug.WriteLine("Documento: " + id);
            Models.JsonMessageModel model = new Models.JsonMessageModel();

            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            try
            {
                mediatori.Models.etc.Documento documento;
                Guid guid = Guid.Parse(id);
                documento = db.Documenti.First(d => d.id == guid);

                if (documento == null)
                {
                    model.esito = Models.JsonMessageModel.Esito.Failed;
                    model.messaggio = "File inesistente";
                    return Json(model);
                }

                db.Documenti.Remove(documento);
                db.SaveChanges();

                Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = getAzureContainer();
                Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob blockBlob = container.GetBlockBlobReference(id);
                if (blockBlob != null && blockBlob.Exists())
                {
                    blockBlob.Delete();
                }

                model.esito = Models.JsonMessageModel.Esito.Succes;
                model.messaggio = "Operazione conlusa con successo";
            }
            catch (Microsoft.WindowsAzure.Storage.StorageException ex)
            {
                //{"The remote server returned an error: (404) Not Found."}
                model.esito = Models.JsonMessageModel.Esito.Failed;
                model.messaggio = ex.Message;
            }
            catch (Exception ex)
            {
                model.esito = Models.JsonMessageModel.Esito.Failed;
                model.messaggio = ex.Message;
            }


           // return Json(model, JsonRequestBehavior.AllowGet);
            return RedirectToAction("Details", "Segnalazioni", new { id = SegnalazioneId });
        }


        public ActionResult Download(string id)
        {
            Debug.WriteLine("Documento: " + id);

            MainDbContext db = new MainDbContext(HttpContext.Request.Url.AbsoluteUri);
            mediatori.Models.etc.Documento documento;

            Guid guid = Guid.Parse(id);
            documento = db.Documenti.First(d => d.id == guid);

            if (documento == null)
            {
                return HttpNotFound();
            }

            Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = getAzureContainer();
            Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob blockBlob = container.GetBlockBlobReference(id);
            if (blockBlob == null || !blockBlob.Exists())
            {
                return HttpNotFound();
            }


            Response.AddHeader("Content-Disposition", "attachment; filename=" + documento.nome); // force download
            Response.AddHeader("Content-Type", System.Web.MimeMapping.GetMimeMapping(documento.nome));
            blockBlob.DownloadToStream(Response.OutputStream);
            return new EmptyResult();
        }

    }
}
