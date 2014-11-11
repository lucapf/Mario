using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mediatori.Models.Pratica
{
    public class Pratica : mediatori.Models.Anagrafiche.Segnalazione  
    {
        [Display(Name = "Indirizzi")]
        public ICollection<mediatori.Models.Anagrafiche.Indirizzo> Indirizzi { get; set; }

        [Required]
        [Display(Name = "Documento identita")]
        public ICollection<mediatori.Models.Anagrafiche.DocumentoIdentita> DocumentiIdentita { get; set; }

        [Required]
        [Display(Name = "Impegni")]
        public ICollection<mediatori.Models.Anagrafiche.Impiego> Impegni { get; set; }

    }
}