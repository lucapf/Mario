using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mediatori.Models.Anagrafiche
{ 
    [Table("tipo_indirizzo")]
    public class TipologiaIndirizzo
    {
        [Key]
        [Required(ErrorMessage="il campo Tipo Indirizzo è obbligatorio")]
        public int id { get; set; }
        [Required]
        [DisplayName("descrizione")]
        public String descrizione { get; set; }
        public String toString()
        {
            return id.ToString();
        }
    }
}