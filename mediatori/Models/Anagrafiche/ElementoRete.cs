using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mediatori.Models.Anagrafiche
{
    [Table("elemento_rete")]
    public class ElementoRete
    {
        [Key]
        public int id { get; set; }
        [Required]
        [Display(Name="punti percentuali")]
        public float punti { get; set; }
        [Required]
        [Display(Name = "Agenzia")]
        public Agenzia agenzia { get; set; }

    }
}