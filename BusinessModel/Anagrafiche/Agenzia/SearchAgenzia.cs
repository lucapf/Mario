using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessModel.Anagrafiche.Agenzia
{
    public class SearchAgenzia : PagedAgenzia
    {
        //Filtri di ricerca
        public string filtroRagioneSociale { get; set; }
        public string filtroCodiceFiscale { get; set; }
    }
}