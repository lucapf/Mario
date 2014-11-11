using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Models.Pratica
{
    public class SearchPratica : PagedPratica
    {
        //Filtri di ricerca
        public string nome { get; set; }
        public string cognome { get; set; }
        

       
    }
}