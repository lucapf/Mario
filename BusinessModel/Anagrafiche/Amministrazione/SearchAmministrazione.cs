using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessModel.Anagrafiche.Amministrazione
{
    public class SearchAmministrazione : PagedAmministrazione
    {
        //Filtri di ricerca
        public string filtroRagioneSociale { get; set; }
        public string filtroPartitaIva { get; set; }
    }
}