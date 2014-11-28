using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Models
{
    public class SegnalazioneDetailsModel
    {
        public Anagrafiche.Segnalazione segnalazione { get; set; }
        public List<mediatori.Models.etc.Stato> listaStatiSuccessivi { get; set; }
       
    }
}