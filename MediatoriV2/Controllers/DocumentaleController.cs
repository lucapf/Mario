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

        public ActionResult Add(HttpPostedFileBase MyFile, string descrizione, int tipoDocumentoId, int segnalazioneId)
        {
            Debug.WriteLine("Add file: " + Request["MyFile"]);

            Models.JsonMessageModel model = new Models.JsonMessageModel();
            if (MyFile == null)
            {
                model.esito = JsonMessageModel.Esito.Failed;
                model.messaggio = "Selezionare un file";
                return Json(model);
            }


            mediatori.Models.etc.Documento documento = new Models.etc.Documento();
            documento.dataInserimento = DateTime.Now;
            documento.descrizione = descrizione;
            //documento.tipoDocumento = new Models.Anagrafiche.TipoDocumento { id = tipoDocumento };
            documento.tipoDocumento = db.TipoDocumenti.Find(tipoDocumentoId);
            documento.SegnalazioneId = segnalazioneId;
            documento.id = Guid.NewGuid();
            documento.nome = MyFile.FileName;

            ModelState.Clear();
            TryValidateModel(documento);

            if (ModelState.IsValid)
            {

                Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = getAzureContainer();
                try
                {
                    container.CreateIfNotExists();
                    Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob blockBlob = container.GetBlockBlobReference(documento.id.ToString());
                    blockBlob.UploadFromStream(MyFile.InputStream);


                    db.Documenti.Add(documento);
                    db.SaveChanges();
                                     

                    model.esito = JsonMessageModel.Esito.Succes;
                    model.messaggio = "Operazione conlusa con successo";
                    model.referenceId = segnalazioneId.ToString() ;
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    model.esito = JsonMessageModel.Esito.Failed;
                    model.messaggio = MyHelper.getDbEntityValidationException(ex);
                }
                catch (Exception ex)
                {
                    model.esito = JsonMessageModel.Esito.Failed;
                    model.messaggio = ex.Message;
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



            return Json(model, JsonRequestBehavior.AllowGet);
           // return RedirectToAction("Details", "Segnalazioni", new { id = segnalazioneId });
        }




          public JsonResult Delete(string id)
       // public ActionResult Delete(string id, int SegnalazioneId)
        {
            Debug.WriteLine("Documento: " + id);
            Models.JsonMessageModel model = new Models.JsonMessageModel();

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


             return Json(model, JsonRequestBehavior.AllowGet);
           // return RedirectToAction("Details", "Segnalazioni", new { id = SegnalazioneId });
        }


        public ActionResult Download(string id)
        {
            Debug.WriteLine("Documento: " + id);

           
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


        [ChildActionOnly]
        public ActionResult DetailsFromSegnalazione(int segnalazioneId)
        {
            //mediatori.Models.Anagrafiche.Segnalazione segnalazione;
            //segnalazione = db.Segnalazioni.Include("documenti").Where(p => p.id == segnalazioneId).First();
            //if (segnalazione == null)
            //{
            //    return HttpNotFound();
            //}

            DocumentaleModel model = new DocumentaleModel();
           // model.documenti = segnalazione.documenti.ToList<Models.etc.Documento>();
            model.segnalazioneId = segnalazioneId;

            model.tipoDocumento = db.TipoDocumenti.OrderBy(p => p.descrizione).ToList();

            // valorizzaDatiViewBag();

            return View("_Documenti", model);
        }


        [ChildActionOnly]
        public ActionResult DetailsFromPratica(int praticaId)
        {
            //mediatori.Models.Anagrafiche.Segnalazione segnalazione;
            //segnalazione = db.Segnalazioni.Include("documenti").Where(p => p.id == praticaId).First();
            //if (segnalazione == null)
            //{
            //    return HttpNotFound();
            //}

            DocumentaleModel model = new DocumentaleModel();
           // model.documenti = segnalazione.documenti.ToList<Models.etc.Documento>();
            model.praticaId = praticaId;

            model.tipoDocumento = db.TipoDocumenti.OrderBy(p => p.descrizione).ToList();

            // valorizzaDatiViewBag();

            return View("_Documenti", model);
        }


        //private void valorizzaDatiViewBag()
        //{
        //    ViewBag.tipoDocumento = new SelectList(db.TipoDocumenti, "id", "descrizione");
        //}


        [HttpGet]
        public ActionResult List(int id)
        {

            List<mediatori.Models.etc.Documento> lista;

            lista = db.Documenti.Include("tipoDocumento").Where(p => p.SegnalazioneId == id).ToList();

            //mediatori.Models.Anagrafiche.Segnalazione segnalazione;
            //segnalazione = db.Segnalazioni.Include("documenti").Include("tipoDocumento").Where(p => p.id == id).First();
            //if (segnalazione == null)
            //{
            //    return HttpNotFound();
            //}

           
            return View("_DocumentList", lista );
        }



    }
}
