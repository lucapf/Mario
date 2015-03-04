using mediatori.Models.etc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;


namespace mediatori.Models.Anagrafiche
{
    [Table("segnalazione")]
    public class Segnalazione
    {
        [Key]
        public int id { get; set; }

        [Required]
        [Display(Name = "Importo")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public decimal? importoRichiesto { get; set; }

        [Required]
        [Display(Name = "Durata")]
        public int? durataRichiesta { get; set; }

        [Required]
        [Display(Name = "Rata")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public decimal? rataRichiesta { get; set; }


        [Display(Name = "Tipo prodotto")]
        public TipoProdotto prodottoRichiesto { get; set; }

        [Display(Name = "Campagna Pubblicitaria")]
        public virtual TipoCampagnaPubblicitaria campagnaPubblicitaria { get; set; }

        [Display(Name = "Canale")]
        public virtual TipoCanaleAcquisizione canaleAcquisizione { get; set; }

        [Display(Name = "Luogo ritrovo")]
        public virtual TipoLuogoRitrovo tipoLuogoRitrovo { get; set; }

        [Display(Name = "Tipo Contatto")]
        public virtual TipoContatto tipoContatto { get; set; }

        [Display(Name = "Tipo azienda")]
        public virtual TipologiaAzienda tipoAzienda { get; set; }

        [Display(Name = "Altri prestiti in corso")]
        public virtual TipologiaPrestito altroPrestito { get; set; }

        [Display(Name = "Fonte pubbl.")]
        public virtual FontePubblicitaria fontePubblicitaria { get; set; }

        public DateTime dataInserimento { get; set; }
        public String utenteInserimento { get; set; }

        [Display(Name = "Consenso Privacy")]
        public ICollection<ConsensoPrivacy> consensoPrivacy { get; set; }

        [Display(Name = "Nota")]
        public ICollection<Nota> note { get; set; }

        [Display(Name = "Preventivo")]
        public ICollection<PreventivoSmall> preventivi { get; set; }

        [Display(Name = "Stato")]
        public Stato stato { get; set; }
        public int statoId { get; set; }

        [Display(Name = "Documenti")]
        public ICollection<Documento> documenti { get; set; }

        public int? preventivoConfermatoId { get; set; }
        //public virtual Preventivo preventivoConfermato { get; set; }

        [Display(Name = "Contatto")]
        public virtual Contatto contatto { get; set; }
        public int contattoId { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data promemoria")]
        [DisplayFormat(DataFormatString = MyConstants.DATE_FORMAT, ApplyFormatInEditMode = true)]
        public DateTime? dataPromemoria { get; set; }

    }

    
}