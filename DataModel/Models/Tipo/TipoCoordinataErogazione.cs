using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mediatori.Models.Anagrafiche
{
    [Table("tipo_coordinata_erogazione")]
    public class TipoCoordinataErogazione
    {
        [Key]
        [Required(ErrorMessage = "indicare il tipo di coordinata erogazione")]
        [Display(Name = "sigla")]
        public string sigla { get; set; }
        [Required]
        [Display(Name = "tipo coordinata erogazione")]
        public String descrizione { get; set; }
        public String toString()
        {
            return sigla;
        }
    }
}