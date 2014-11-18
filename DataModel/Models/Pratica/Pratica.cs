using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mediatori.Models.Pratica
{
    public class Pratica : mediatori.Models.Anagrafiche.Segnalazione
    {
        [Required]
        [Display(Name = "Cedente")]
        public virtual Models.Anagrafiche.Cedente cedente { get; set; }

        
        public virtual Models.Preventivo preventivoConfermato { get; set; }
    }
}