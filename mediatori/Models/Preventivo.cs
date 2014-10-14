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
        [Display(Name="Progressivo")]
        public int progressivo { get; set; }


        [Required]
        [Display(Name = "nome Prodotto")]
        public string nomeProdotto { get; set; }
        [Required]
        [Display(Name = "Intermediario")]
        public SoggettoGiuridico finanziaria { get; set; }
        [Required]
        [Display(Name = "Importo Rata")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public float? importoRata { get; set; }
        [Required]
        [Display(Name = "Durata (mesi)")]
        public int? durata { get; set; }
        [Required]
        [Display(Name = "Tabella  finanziara")]
        public string tabellaFinanziaria { get; set; }
        [Required]
        [Display(Name = "Comp. Ass. vita")]
        public SoggettoGiuridico assicurazioneVita { get; set; }
        [Required]
        [Display(Name = "Comp. Ass. impiego")]
        public SoggettoGiuridico assicurazioneImpiego { get; set; }
        [Required]
        [Display(Name = "Imp. copert. vita")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal? importoCoperturaVita { get; set; }
        [Required]
        [Display(Name = "Imp. copert. impiego")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal? importoCoperturaImpego { get; set; }
        [Display(Name = "Inserito il ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? dataInserimento { get; set; }
        [Required]
        [Display(Name = "Data Decorrenza")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? dataDecorrenza { get; set; }
        [Required]
        [Display(Name = "Imp. provvigioni")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public float? importoProvvigioni { get; set; }
        [Required]
        [Display(Name = "Montante")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal? montante { get; set; }
        [Required]
        [Display(Name = "Interessi")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal? importoInteressi { get; set; }
        [Required]
        [Display(Name = "Spese attivazione")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal? speseAttivazione { get; set; }
        [Required]
        [Display(Name = "Spese incasso")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal? speseIncasso { get; set; }
        [Required]
        [Display(Name = "Spese oneri fiscali")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal? oneriFiscali { get; set; }
        [Required]
        [Display(Name = "Imp.to Imp.gni da estinguere")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal? importoImpegniDaEstinguere { get; set; }


        [Required]
        [Display(Name = "Netto cliente")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal? nettoCliente { get; set; }
        [Required]
        [Display(Name = "TAN")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:P2}")]
        public float? tan { get; set; }

        [Required]
        [Display(Name = "TAEG")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:P2}")]
        public float? taeg { get; set; }

        [Required]
        [Display(Name = "TEG")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:P2}")]
        public float? teg { get; set; }


        [Display(Name = "Data Conferma Preventivo")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? dataConferma {get;set;}
        
        [Display(Name = "operatore conferma")]
        public String operatoreConferma;

        //[Display(Name = "Accettato")]
        //public bool? accettato{get;set;}

    }
}