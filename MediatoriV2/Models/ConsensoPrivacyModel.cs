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
        public ICollection<Anagrafiche.ConsensoPrivacy> consensiPrivacy { get; set; }
        public int? segnalazioneId { get; set; }
        
    }
}