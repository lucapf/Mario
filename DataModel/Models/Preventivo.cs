using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mediatori.Models
{
   
    public class Preventivo : PreventivoSmall
    {
        
        [Required]
        public int? assicurazioneVitaId { get; set; }
        [Display(Name = "Comp. Ass. vita")]
        public virtual SoggettoGiuridico assicurazioneVita { get; set; }

        [Required]
        public int? assicurazioneImpiegoId { get; set; }
        [Display(Name = "Comp. Ass. impiego")]
        public virtual SoggettoGiuridico assicurazioneImpiego { get; set; }

        [Required]
        public int? finanziariaId { get; set; }
        [Display(Name = "Intermediario")]
        public virtual SoggettoGiuridico finanziaria { get; set; }
        
        [Required]
        [Display(Name = "Nome Prodotto")]
        public string nomeProdotto { get; set; }

        [Required]
        [Display(Name = "Tabella  finanziara")]
        public string tabellaFinanziaria { get; set; }

        [Required]
        [Display(Name = "Imp. copert. vita")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public decimal? importoCoperturaVita { get; set; }

        [Required]
        [Display(Name = "Imp. copert. impiego")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public decimal? importoCoperturaImpiego { get; set; }
        
        [Required]
        [Display(Name = "Data Decorrenza")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = MyConstants.DATE_FORMAT, ApplyFormatInEditMode = false)]
        public DateTime? dataDecorrenza { get; set; }

        [Required]
        [Display(Name = "Imp. provvigioni")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public decimal? importoProvvigioni { get; set; }

        
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
        [Display(Name = "TEG")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:P2}")]
        public decimal? teg { get; set; }

        
    }
}