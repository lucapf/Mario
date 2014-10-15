using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mediatori.Models.Anagrafiche
{
    [Table("Riferimento")]
    public class Riferimento
    {
     
        [Key]
        public int id { get; set; }
        [Required]
        [Display(Name = "Riferimento")]
        public String riferimento;
        [Required]
        [Display(Name="Tipo Riferimento")]
        public TipoRiferimento tipoRiferimento { get; set; }
        [Required]
        [Display(Name = "valore")]
        public String valore { get; set; }
            
    }
}