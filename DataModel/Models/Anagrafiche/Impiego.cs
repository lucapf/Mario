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
        [Display(Name = "Impiego")]
        public int id { get; set; }

        [Required]
        [Display(Name = "Azienda")]
        public String azienda { get; set; }

        [Display(Name = "Sede di lavoro azienda")]
        public String aziendaSedeLavoro { get; set; }


        [Display(Name = "Retribuzione netta mensile")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        //[DataType(DataType.Currency)]
        public decimal? stipendioNettoMensile { get; set; }

        [Display(Name = "Retribuzione lorda mensile")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        //[DataType(DataType.Currency)]
        public decimal? stipendioLordoMensile { get; set; }

        [Display(Name = "Retribuzione lorda annua")]
        //[DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        //[DataType(DataType.Currency)]
        public decimal? stipendioLordoAnnuo { get; set; }


        [Display(Name = "Nr mensilita'")]
        //[DataType(DataType.Duration)]
        public int? mensilita { get; set; }

        [Display(Name = "Mansione")]
        public String mansione { get; set; }

        [Display(Name = "Tipo contratto")]
        public TipoContrattoImpiego tipoImpiego { get; set; }

        [Display(Name = "Categoria")]
        public TipoCategoriaImpiego categoriaImpiego { get; set; }

        [Display(Name = "Anticipi TFR")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public decimal? anticipiTFR { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data adesione fondo")]
        //[DisplayFormat(NullDisplayText = "", DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //[DisplayFormat(NullDisplayText = "", DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? adesioneTFR { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data assunzione")]
        [DisplayFormat(ApplyFormatInEditMode = true, NullDisplayText = "", DataFormatString = MyConstants.DATE_FORMAT)]
        public DateTime? dataAssunzione { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data licenziamento")]
        //[DisplayFormat(NullDisplayText = "", DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //[DisplayFormat(NullDisplayText = "", DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? dataLicenziamento { get; set; }


        //TODO: inserire il datore di lavoro!


    }
}