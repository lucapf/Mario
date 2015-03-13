using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mediatori.Models.Anagrafiche
{
    [Table("erogazione")]
    public class Erogazione
    {
        [Key]
        public int id { get; set; }
    
        [Required]
        [Display(Name = "tipo erogazione")]
        public string tipoErogazioneId { get; set; }
        public virtual TipoErogazione tipoErogazione {get;set;}

        [Display(Name = "Coordinata Erogazione")]
        public int coordinataErogazioneId { get; set; }
        public virtual CoordinateErogazione coordinataErogazione { get; set; }

        [Display(Name = "Pratica")]
        public int praticaId { get; set; }
        public virtual mediatori.Models.Pratica.Pratica pratica { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data valuta")]
        [DisplayFormat(DataFormatString = MyConstants.DATE_FORMAT, ApplyFormatInEditMode = true)]
        public DateTime? dataValuta { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data pagamento")]
        [DisplayFormat(DataFormatString = MyConstants.DATE_FORMAT, ApplyFormatInEditMode = true)]
        public DateTime? dataPagamento { get; set; }

        [Required]
        [Display(Name = "Importo")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public decimal? importo { get; set; }

        [Display(Name = "Nota")]
        public Nota nota { get; set; }


    }
}