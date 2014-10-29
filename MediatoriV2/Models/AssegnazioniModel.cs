using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Models
{
    public class AssegnazioniModel
    {
       public List<Models.Anagrafiche.Segnalazione> DaAssegnare { get; set;}
        public List<Models.etc.Assegnazione> Assegnate { get; set; }
    }
}