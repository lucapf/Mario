using mediatori.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Filters
{
    public class SoggettoGiuridicoSearch
    {
        public String ragioneSociale { get; set; }
        public String codiceFiscale { get; set; }
        public string tipoSoggettoGiuridico { get; set; }
    }
}