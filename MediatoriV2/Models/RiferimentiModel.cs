using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Models
{
    public class RiferimentiModel
    {
        public List<Anagrafiche.Riferimento> riferimenti { get; set; }

        public int? contattoId { get; set; }
        public int? soggettoGiuridicoId { get; set; }
    }
}