using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mediatori.Models.Anagrafiche;

namespace mediatori.Models
{
    public class ConsensoPrivacyModel 
    {
      
        public int id { get; set; }
      
        public TipoConsensoPrivacy tipoConsensoPrivacy { get; set; }
       
        public Boolean acconsento { get; set; }
      
        public Boolean nonAcconsento { get; set; }
       
        public DateTime dataInserimento { get; set; }

        public String untenteInserimento { get; set; }
    }
}