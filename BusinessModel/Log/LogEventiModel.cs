using mediatori.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessModel.Log
{
    public class LogEventiModel
    {
        public List<LogEventi> listaEventi { get; set; }
        public EnumEntitaRiferimento entitaRiferimento { get; set; }
        public int idEntita { get; set; }
        public mediatori.Models.etc.EnumEntitaAssociataStato? entitaAssociataStato { get; set; }
        // public List<History> listaCollegati { get; set; }

    }
}
