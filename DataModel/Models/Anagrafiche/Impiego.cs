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

        //http://msdn.microsoft.com/en-us/data/jj713564.aspx
        //[ForeignKey("Contatto")]
        public int contattoId { get; set; }
        public virtual Contatto contatto { get; set; }

        [Required]
        [Display(Name = "Azienda")]
        public int amministrazioneId { get; set; }
        public virtual Amministrazione amministrazione { get; set; }

        public string amministrazione_descrizione { get; set; }

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
        public int tipoImpiegoId { get; set; }
        public virtual TipoContrattoImpiego tipoImpiego { get; set; }

        [Display(Name = "Categoria")]
        public int categoriaImpiegoId { get; set; }
        public virtual TipoCategoriaImpiego categoriaImpiego { get; set; }

        [Display(Name = "Anticipi TFR")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public decimal? anticipiTFR { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data adesione fondo")]
        [DisplayFormat(DataFormatString = MyConstants.DATE_FORMAT, ApplyFormatInEditMode = true)]
        public DateTime? adesioneTFR { get; set; }

        [DataType(DataType.Date)]
        [Required]
        [Display(Name = "Data assunzione")]
        [DisplayFormat(DataFormatString = MyConstants.DATE_FORMAT, ApplyFormatInEditMode = true)]
        public DateTime? dataAssunzione { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data licenziamento")]
        [DisplayFormat(DataFormatString = MyConstants.DATE_FORMAT, ApplyFormatInEditMode = true)]
        public DateTime? dataLicenziamento { get; set; }


        //TODO: inserire il datore di lavoro!


    }
}