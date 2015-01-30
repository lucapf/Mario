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
        [Display(Name = "Societa")]
        public SoggettoGiuridico soggettoGiuridico { get; set; }

        [Required]
        [Display(Name = "Partita iva")]
        [StringLength(11, ErrorMessage = "La partita iva deve essere lunga 11 caratteri")]
        [RegularExpression("[0-9]{11}", ErrorMessage = "La partita iva deve essere lunga 11 caratteri numerici")]
        public string partitaIva { get; set; }

        [Required]
        [Display(Name = "Rea")]
        public string rea { get; set; }

        [Required]
        [Display(Name = "Natura giuridica")]
        public TipoNaturaGiuridica tipoNaturaGiuridica { get; set; }

        [Display(Name = "Stato")]
        public bool isEnabled { get; set; }

        [Display(Name = "Data inizio mandato")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = MyConstants.DATE_FORMAT, ApplyFormatInEditMode = true)]
        public DateTime? dataInizioMandato { get; set; }

        [Display(Name = "Data fine mandato")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = MyConstants.DATE_FORMAT, ApplyFormatInEditMode = true)]
        public DateTime? dataFineMandato { get; set; }

        [Display(Name = "Tipo agenzia")]
        public TipoAgenzia tipoAgenzia { get; set; }

        [Display(Name = "Codice Rui")]
        public String codiceRui { get; set; }

        [Display(Name = "Codice Oam")]
        public String codiceOam { get; set; }

        [Display(Name = "Data Oam")]
        [DisplayFormat(DataFormatString = MyConstants.DATE_FORMAT, ApplyFormatInEditMode = true)]
        public DateTime? dataOam { get; set; }

        [Display(Name = "Documento di pagamento")]
        public String documentoPagamento { get; set; }
    }
}

