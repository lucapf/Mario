using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Models.Anagrafiche
{
    public class AgenziaCreateModel
    {
        public Agenzia agenzia { get; set; }
        public SoggettoGiuridico soggettoGiuridico { get; set; }
        //public List<Indirizzo> indirizzi { get; set; }
        //public List<Riferimento> riferimenti { get; set; }
        //public List<Nota> note { get; set; }

        public Indirizzo indirizzo { get; set; }
        public Riferimento riferimento { get; set; }
        public Nota nota { get; set; }
    }
}