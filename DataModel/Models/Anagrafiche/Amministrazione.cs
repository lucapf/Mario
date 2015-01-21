using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mediatori.Models.Anagrafiche
{
        [Table("amministrazione")]
        public class Amministrazione
        {
            [Key]
            public int id { get; set; }
            
            [Required]
            public SoggettoGiuridico soggettoGiuridico { get; set; }
            public int soggettoGiuridicoId { get; set; }


            [Required]
            [Display(Name = "Partita Iva")]
            public string partitaIva { set; get; }
            [Required]
            [Display(Name = "Natura Giuridica")]
            public TipoNaturaGiuridica tipoNaturaGiuridica { get; set; }
           

            [Display(Name = "Stato")]
            public bool isEnabled { get; set; }

            [Required]
            [Display(Name = "Capitale Sociale")]
            [DisplayFormat(DataFormatString = "{0:c}")]
            public decimal? capitaleSociale { get; set; }

            [Required]
            [Display(Name = "Categoria")]
            public TipoCategoriaAmministrazione tipoCategoria { get; set; }

            [Required]
            [Display(Name = "Assumibilita")]
            public TipoAssumibilitaAmministrazione assumibilita { get; set; }

            [Display(Name = "Amm. Pagante")]
            public Amministrazione pagante { get; set; }
        }
    }

