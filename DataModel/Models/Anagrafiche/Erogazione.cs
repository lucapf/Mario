using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mediatori.Models.Anagrafiche
{
     [Table("Amministrazione")]
    public class Erogazione
    {
         [Key]
         public int id { get; set; }
         [Required]
         [Display(Name="codice Iban")]
         public String iban { get; set; }
         [Required]
         [Display(Name = "tipo erogazione")]
         public TipoErogazione tipoErogazione;
    }
}