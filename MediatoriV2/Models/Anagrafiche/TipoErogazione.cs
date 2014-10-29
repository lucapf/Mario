using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mediatori.Models.Anagrafiche
{
    [Table("tipo_erogazione")]
    public class TipoErogazione
    {
        [Key]
        [Required(ErrorMessage="indicare il tipo di erogazione")]
        public string sigla { get; set; }
        [Required]
        [Display(Name="tipo Erogazione")]
        public String descrizione { get; set; }
        public String toString()
        {
            return sigla;
        }
    }
}