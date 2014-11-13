using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Models
{
    public class DocumentiIdentitaModel
    {
        public List<Anagrafiche.DocumentoIdentita > documentiIdentia { get; set; }
        public int cedenteId { get; set; }
    }
}