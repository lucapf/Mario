using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Filters
{
    public class SegnalazioneSearch
    {
        public String nome { get; set; }
        public String cognome { get; set; }
        public DateTime? dataInserimentoDa { get; set; }
        public DateTime? dataInserimentoA { get; set; }
    }
}