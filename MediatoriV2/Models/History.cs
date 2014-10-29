using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mediatori.Models
{
    public class History 
    {
        public List<LogEventi> listaEventi { get; set; }
        public EnumEntitaRiferimento entitaRiferimento { get; set; }
        public List<History> listaCollegati { get; set; }
        
    }
}
