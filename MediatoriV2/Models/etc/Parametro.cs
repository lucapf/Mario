using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mediatori.Models.etc
{
    [Table ("Parametro")]
    public class Parametro
    {
        [Key]
        public int id { get; set; }
        [Required]
        [Display(Name="Nome parametro")]
        public string key { get; set; }
        [Required]
        [Display(Name = "Descrizione parametro")]
        public string descrizione { get; set; }
        [Required]
        [Display(Name="Valore")]
        public string value { get; set; }
    }
}