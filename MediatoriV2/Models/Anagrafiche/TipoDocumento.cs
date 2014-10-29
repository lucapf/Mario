using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mediatori.Models.Anagrafiche
{
    [Table("tipo_documento")]
    public class TipoDocumento
    {
        [Key]
        public int id { get; set; }
        [Required]
        [Display(Name = "Descrizione")]
        public String descrizione { get; set; }
    }
}