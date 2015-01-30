using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mediatori.Models.Anagrafiche
{
    [Table("tipo_ente_rilascio")]
    public class TipoEnteRilascio
    {
        [Key]
        [Required(ErrorMessage = "indicare l'Ente Rilascio")]
        public int id { get; set; }

        [Required]
        [Display(Name = "Ente Rilascio")]
        public String descrizione { get; set; }
     
        public String toString()
        {
            return id.ToString();
        }
    }
}