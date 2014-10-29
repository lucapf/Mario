using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mediatori.Models
{
    [Table("log_evento")]
    public class LogEventi
    {
        [Key]
        public int id { get; set; }
        public String operatoreInserimento { get; set; }
        public DateTime dataInserimento { get; set; }
        public EnumTipoEventoLog tipoEvento { get; set; }
        public String messaggio { get; set; }
        public int idEntita { get; set; }
        public EnumEntitaRiferimento tipoEntitaRiferimento { get; set; }
    }

    public class LogEventiFilter
    {
        public String operatoreInserimento { get; set; }
        public DateTime dataInserimentoDa { get; set; }
        public DateTime dataInserimentoA { get; set; }
        public EnumTipoEventoLog tipoEvento { get; set; }
        public String messaggioEsatto { get; set; }
        //public int idEntita { get; set; }
        //public EnumEntitaRiferimento tipoEntitaRiferimento { get; set; }
    }
}