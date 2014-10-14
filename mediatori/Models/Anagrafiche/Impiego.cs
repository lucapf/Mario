using mediatori.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mediatori.Models.Anagrafiche
{ 
    [Table("impiego")]
    public class Impiego
    {
       
        [Key]
        [Display(Name="Impiego")]
        public int id { get; set; }
        
        [Display(Name="Stipendio Netto")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        //[DataType(DataType.Currency)]
        public decimal? stipendioNetto { get; set; }
        
        [Display(Name="Nr mensilita'")]
       
        [DataType(DataType.Duration)]
        public int? mensilita { get; set; }

        [Display(Name="Mansione")]
        public String mansione { get; set; }
        
        [Display(Name="Tipo Contratto")]
        public TipoContrattoImpiego tipoImpiego { get; set; }
        
        [Display(Name = "Categoria")]
        public TipoCategoriaImpiego categoriaImpiego { get; set; }
        
        [Display(Name = "Anticipi TFR")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal? anticipiTFR{get;set;}

        [DataType(DataType.Date)]
        [Display(Name = "Data Adesione fondo")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? adesioneTFR { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data Assunzione")]       
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]  
        public DateTime? dataAssunzione { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data licenziamento")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? dataLicenziamento { get; set; }
        //TODO: inserire il datore di lavoro!
        public String toString()
        {
            return id.ToString();
        }

    }
}