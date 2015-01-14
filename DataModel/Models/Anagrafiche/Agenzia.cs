using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mediatori.Models.Anagrafiche
{
  
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
            //public mediatori.Models.etc.Stato stato { get; set; }
            public bool disabled { get; set; }

            [Display(Name = "Data inizio mandato")]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime? dataInizioMandato { get; set; }
            [Display(Name = "Data fine mandato")]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime? dataFineMandato { get; set; }
            [Display(Name = "Tipo agenzia")]
            public TipoAgenzia tipoAgenzia { get; set; }
            [Display(Name = "Codice Rui")]
            public String codiceRui { get; set; }
            [Display(Name = "Codice Oam")]
            public String codiceOam { get; set; }
            [Display(Name = "Data Oam")]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime? dataOam { get; set; }
            [Display(Name = "Documento di pagamento")]
            public String documentoPagamento { get; set; }
        }
    }

