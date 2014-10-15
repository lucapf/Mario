using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Models.Documentale
{
    public class DocumentaleIndex
    {
        public string Container { get; set; }
        public List<mediatori.Models.etc.Documento> documenti { get; set; }
        public List<mediatori.Models.Anagrafiche.TipoDocumento> tipoDocumento { get; set; }
    }
}