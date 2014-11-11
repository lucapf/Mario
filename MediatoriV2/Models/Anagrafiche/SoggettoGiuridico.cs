using mediatori.Models.etc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mediatori.Models.Anagrafiche
{
     [Table("soggetto_giuridico")]
    public class SoggettoGiuridico
    {
         [Key]
         public int id { get; set; }
         [Required]
         [Display(Name="Ragione sociale")]
         public string ragioneSociale { get; set; }

         [Required]
         [Display(Name = "Tipo soggetto")]
         public String tipoSoggettoGiuridico { get; set; }

         [Required]
         [Display(Name = "Codice Fiscale")]
         public String codiceFiscale { get; set; }

         [Display(Name="Indirizzi")]
         public ICollection<Indirizzo> indirizzi { get; set; }

         [Display(Name = "Riferimenti")]
         public ICollection<Riferimento> riferimenti { get; set; }

         [Display(Name = "Note")]
         public ICollection<Nota> note { get; set; }
    }
     #region Amministrazione
     [Table("amministrazione")]
     public class Amministrazione
     {
         [Key]
         public int id { get; set; }
         [Required]
         public SoggettoGiuridico soggettoGiuridico{get;set;}
         [Required]
         [Display(Name = "Partita Iva")]
         public string partitaIva {set;get;}
         [Required]
         [Display(Name="Natura Giuridica")]
         public TipoNaturaGiuridica tipoNaturaGiuridica {get;set;}
         [Required]
         [Display(Name="Stato")]
         public Stato stato { get; set; }
         [Required]
         [Display(Name="Capitale Sociale")]
         [DisplayFormat(DataFormatString = "{0:c}")]
         public float? capitaleSociale { get; set; }
         [Required]
         [Display(Name = "Categoria")]
         public TipoCategoriaAmministrazione tipoCategoria { get; set; }
         [Required]
         [Display(Name="Assumibilita")]
         public TipoAssumibilitaAmministrazione assumibilita{get;set;}
         [Display(Name="Amm. Pagante")]
         public Amministrazione pagante {get;set;}
     }
     public class AmministrazioneCreate 
     {
         public Amministrazione amministrazione{get;set;}
         public SoggettoGiuridico soggettoGiuridico { get; set; }
         public List<Indirizzo> indirizzi {get;set;}
         public List<Riferimento> riferimenti { get; set; }
         public List<Nota> note { get; set; }
     }
#endregion 
     #region Agenzia
    [Table("agenzia")] 
    public class Agenzia
     {
        [Key]
        public int id { get; set; }
        [Required]
        [Display(Name = "societa")]
        public SoggettoGiuridico soggettoGiuridico { get; set; }
        [Required]
        [Display(Name = "Partita Iva")]
        public string partitaIva { get; set; }
        [Required]
        [Display(Name = "Rea")]
        public string rea { get; set; }
        [Required]
        [Display(Name = "Natura Giuridica")]
        public TipoNaturaGiuridica tipoNaturaGiuridica { get; set; }
     
        [Display(Name = "Stato")]
        public Stato stato { get; set; }

        [Display(Name="Data inizio mandato")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? dataInizioMandato {get;set;}
        [Display(Name="Data fine mandato")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? dataFineMandato {get;set;}
        [Display(Name="Tipo agenzia")]
        public TipoAgenzia tipoAgenzia {get;set;}
        [Display(Name="Codice Rui")]
        public String codiceRui{get;set;}
        [Display(Name="Codice Oam")]
        public String codiceOam {get;set;}
        [Display(Name="Data Oam")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? dataOam {get;set;}
        [Display(Name = "Documento di pagamento")]
        public String documentoPagamento { get; set; }
     }
    public class AgenziaFilter
    {
        public string ragioneSociale { get; set; }
        public string partitaIva { get; set; }
    }
    public class AgenziaCreate
    {
        public Agenzia agenzia { get; set; }
        public SoggettoGiuridico soggettoGiuridico { get; set; }
        public List<Indirizzo> indirizzi { get; set; }
        public List<Riferimento> riferimenti { get; set; }
        public List<Nota> note { get; set; }
    }
#endregion 

}