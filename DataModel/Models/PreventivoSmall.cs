using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mediatori.Models
{
    [Table("preventivo")]
    public class PreventivoSmall
    {
        [Key]
        public int id { get; set; }

        public int segnalazioneId { get; set; }
        public virtual mediatori.Models.Anagrafiche.Segnalazione segnalazione { get; set; }


        [Required]
        [Display(Name = "Progressivo")]
        public int progressivo { get; set; }
        
        [Display(Name = "Importo Rata")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public decimal? importoRata { get; set; }

        [Required]
        [Display(Name = "Durata (mesi)")]
        public int? durata { get; set; }
       
        [Display(Name = "Inserito il ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = MyConstants.DATE_FORMAT, ApplyFormatInEditMode = false)]
        public DateTime? dataInserimento { get; set; }
       
        [Required]
        [Display(Name = "Montante")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public decimal? montante { get; set; }

        [Required]
        [Display(Name = "Netto cliente")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public decimal? nettoCliente { get; set; }

        [Required]
        [Display(Name = "TAN")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:P2}")]
        public decimal? tan { get; set; }

        [Required]
        [Display(Name = "TAEG")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:P2}")]
        public decimal? taeg { get; set; }

        [Display(Name = "Data conferma preventivo")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = MyConstants.DATE_FORMAT, ApplyFormatInEditMode = false)]
        public DateTime? dataConferma { get; set; }

        [Display(Name = "Operatore conferma")]
        public String operatoreConferma;

        [Display(Name = "Operatore inserimento")]
        public String operatoreInserimento;

        public bool bySimulatore { get; set; }

        //[Display(Name = "Accettato")]
        //public bool? accettato{get;set;}

    }
}