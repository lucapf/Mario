using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Models
{
    public class IndirizziModel
    {
        public List<Anagrafiche.Indirizzo> indirizzi { get; set; }

        public int? contattoId { get; set; }
        public int? soggettoGiuridicoId { get; set; }
    }
}