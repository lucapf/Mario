using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Models.Anagrafiche
{
    public class SoggettoGiuridicoCreateModel
    {
        public SoggettoGiuridico soggettoGiuridico { get; set; }
        
        public Indirizzo indirizzo { get; set; }
        public Riferimento riferimento { get; set; }
        public Nota nota { get; set; }
    }
}