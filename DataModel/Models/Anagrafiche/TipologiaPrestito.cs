using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mediatori.Models.Anagrafiche
{
    [Table("tipo_prestito")]
    public class TipologiaPrestito
    {
        [Key]
        public int id { get; set; }
        [Required]
        [Display(Name="Descrizione")]
        public String descrizione { get; set; }
        public String toString()
        {
            return id.ToString();
        }
    }
}