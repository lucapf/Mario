using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mediatori.Models.Anagrafiche
{
    [Table("documento_identita")]
    public class DocumentoIdentita
    {
        [Key]
        [Display(Name="Documento identita")]
        public int id { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data Rilascio")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime dataRilascio { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data Scadenza")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime dataScadenza { get; set; }
        [Required]
        [Display(Name="Ente rilascio")]
        public TipoEnteRilascio enteRilascio { get; set; }
        [Required]
        [Display(Name="Provincia")]
        public Provincia provinciaEnte { get; set; }
        [Required]
        [Display(Name="Comune")]
        public Comune comuneEnte { get; set; }
        [Required]
        [Display(Name="Nr. documento")]
        public String numeroDocumento { get; set; }
        public String toString()
        {
            return id.ToString();
        }
    }
}