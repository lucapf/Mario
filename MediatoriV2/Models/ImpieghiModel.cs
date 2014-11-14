using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Models
{
    public class ImpieghiModel
    {
        public List<Anagrafiche.Impiego> impieghi { get; set; }

        public int? contattoId { get; set; }
        
    }
}