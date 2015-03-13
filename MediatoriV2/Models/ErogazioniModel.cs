using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Models
{
    public class ErogazioniModel
    {
        public List<Anagrafiche.Erogazione> erogazioni { get; set; }
        public int cedenteId { get; set; }
    }
}