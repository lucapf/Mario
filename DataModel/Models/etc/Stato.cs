using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace mediatori.Models.etc
{
    [Table("stato")]
    public class Stato
    {
        [Key]
        public int id { get; set; }

        [Required]
        public String descrizione { get; set; }

        [Required]
        public EnumStatoBase statoBase { get; set; }

        [Required]
        public EnumEntitaAssociataStato entitaAssociata { get; set; }

        
        public int? gruppoId { get; set; }
        // per gli stati base di tipo Attivo può essere indicato un gruppo di lavorazione
        [NotMapped]
        public virtual MyUsers.Models.MyGroup gruppo { get; set; }

      //  public virtual List<Stato> successivi { get; set; }
        //public virtual List<Stato> precedenti { get; set; }
    }



}