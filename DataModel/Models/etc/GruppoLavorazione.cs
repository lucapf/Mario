using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace mediatori.Models.etc
{
    [Table("gruppo_lavorazione")]
    public class GruppoLavorazione
    {
        [Key]
        public int id { get; set; }
        [Required]
        public String nome { get; set; }
        //indica la lista degli utenti seperati da ';'
        public String utenti { get; set; }
    }


   
}