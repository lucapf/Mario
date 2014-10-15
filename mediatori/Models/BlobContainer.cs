using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Models
{
    public class BlobContainer
    {
        public List<Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob> Blobs { get; set; }

        public BlobContainer()
        {
            Blobs = new List<Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob>();
        }

    }
}