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
        // per gli stati base di tipo Attivo può essere indicato un gruppo di lavorazione
        public GruppoLavorazione gruppoLavorazione { get; set; }
        public List<Stato> successivi { get; set; }
        public List<Stato> precedenti { get; set; }


    }

    public class StatoSearch {
        public EnumEntitaAssociataStato entita{get;set;}
        public int? codiceStato { get; set; }
        public int? successiviDi { get; set; }
        public int? precedentiDi { get; set; }
    }
  
}