using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Models
{
    public class DocumentaleModel
    {
        public List<etc.Documento> documenti { get; set; }

        public int? segnalazioneId { get; set; }
        public int? praticaId { get; set; }

        public List<mediatori.Models.Anagrafiche.TipoDocumento> tipoDocumento { get; set; }
    }
}