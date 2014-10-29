using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mediatori.Models
{
    [Table("nota")]
    public class Nota
    {
        public Nota()
        {
            id = 0;
        }

        [Key]
        public int id { get; set; }
        [Required]
        [Display(Name="Data ins.")]
        public DateTime? dataInserimento { get; set; }
        [Required]
        [Display(Name="Operatore ins.")]
        public String operatoreInserimento { get; set; }

        [Display(Name = "Testo")]
        public String valore { get; set; }
    }
}