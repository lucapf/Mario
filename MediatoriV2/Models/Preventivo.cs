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
    public class Preventivo
    {
        [Key]
        public int id { get; set; }

        [Required]
        [Display(Name = "Progressivo")]
        public int progressivo { get; set; }

        [Required]
        [Display(Name = "Nome Prodotto")]
        public string nomeProdotto { get; set; }
            
        [Display(Name = "Intermediario")]
        public SoggettoGiuridico finanziaria { get; set; }
        [Required]
        public int finanziariaId { get; set; }
        
        [Display(Name = "Importo Rata")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public decimal? importoRata { get; set; }
       
        [Required]
        [Display(Name = "Durata (mesi)")]
        public int? durata { get; set; }

        [Required]
        [Display(Name = "Tabella  finanziara")]
        public string tabellaFinanziaria { get; set; }

        [Display(Name = "Comp. Ass. vita")]
        public SoggettoGiuridico assicurazioneVita { get; set; }
        [Required]
        public int assicurazioneVitaId { get; set; }


        [Display(Name = "Comp. Ass. impiego")]
        public SoggettoGiuridico assicurazioneImpiego { get; set; }
        [Required]
        public int assicurazioneImpiegoId { get; set; }

        [Required]
        [Display(Name = "Imp. copert. vita")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public decimal? importoCoperturaVita { get; set; }

        [Required]
        [Display(Name = "Imp. copert. impiego")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public decimal? importoCoperturaImpego { get; set; }

        [Display(Name = "Inserito il ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = false)]
        public DateTime? dataInserimento { get; set; }

        [Required]
        [Display(Name = "Data Decorrenza")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = false)]
        public DateTime? dataDecorrenza { get; set; }

        [Required]
        [Display(Name = "Imp. provvigioni")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public decimal? importoProvvigioni { get; set; }

        [Required]
        [Display(Name = "Montante")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public decimal? montante { get; set; }

        [Required]
        [Display(Name = "Interessi")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public decimal? importoInteressi { get; set; }

        [Required]
        [Display(Name = "Spese attivazione")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public decimal? speseAttivazione { get; set; }

        [Required]
        [Display(Name = "Spese incasso")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public decimal? speseIncasso { get; set; }

        [Required]
        [Display(Name = "Spese oneri fiscali")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public decimal? oneriFiscali { get; set; }

        [Required]
        [Display(Name = "Imp.to Imp.gni da estinguere")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public decimal? importoImpegniDaEstinguere { get; set; }

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

        [Required]
        [Display(Name = "TEG")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:P2}")]
        public decimal? teg { get; set; }

        [Display(Name = "Data Conferma Preventivo")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = false)]
        public DateTime? dataConferma { get; set; }

        [Display(Name = "Operatore conferma")]
        public String operatoreConferma;

        //[Display(Name = "Accettato")]
        //public bool? accettato{get;set;}

    }
}