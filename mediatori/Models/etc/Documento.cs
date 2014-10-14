using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mediatori.Models.etc
{
    [Table("documento")]
    public class Documento
    {
        [Key]
        public int id { get; set; }

        [Required]
        [Display(Name = "Descrizione")]
        public String descrizione { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data inserimento")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? dataInserimento { get; set; }

        [Required]
        [Display(Name = "Tipo documento")]
        public Anagrafiche.TipoDocumento tipoDocumento { get; set; }
    }
}