using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mediatori.Models.Test
{
    public class TestClass
    {
        public int CodiceId { get; set; }

        [Required]
        [Display(Name = "Nominativo")]
        public String Nome { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data di nascita")]
        public DateTime?  dataDiNascita { get; set; }


        [Required]
        [Display(Name = "Importo")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public decimal? importoRichiesto { get; set; }


        [Required]
        [Display(Name = "Tipo con descrizione")]
        public int tipoId { get; set; }
    }
}