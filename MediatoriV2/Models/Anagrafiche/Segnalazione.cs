using mediatori.Models.etc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace mediatori.Models.Anagrafiche
{
    [Table("Segnalazione")]
    public class Segnalazione
    {
        [Key]
        public int id { get; set; }
        [Required]
        [Display(Name = "Contatto")]
        public Contatto contatto { get; set; }
        //   [Display(Name = "Riferimenti")]
        //   public ICollection<Riferimento> riferimenti { get; set; }
        [Required]
        [Display(Name = "Importo")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public float? importoRichiesto { get; set; }
        [Required]
        [Display(Name="Durata")]
        public int? durataRichiesta { get; set; }
        [Required]
        [Display(Name = "Rata")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public float? rataRichiesta { get; set; }
        [Display(Name = "Prodotto")]
        public TipoProdotto prodottoRichiesto { get; set; }

        [Display(Name = "Campagna Pubblicitaria")]
        public virtual TipoCampagnaPubblicitaria campagnaPubblicitaria { get; set; }

        [Display(Name = "Canale")]
        public virtual TipoCanaleAcquisizione canaleAcquisizione { get; set; }

        [Display(Name = "Luogo ritrovo")]
        public virtual TipoLuogoRitrovo tipoLuogoRitrovo { get; set; }
        [Display(Name = "tipo Contatto")]
        public virtual TipoContatto tipoContatto { get; set; }


        [Display(Name = "Tipo azienda")]
        public virtual TipologiaAzienda tipoAzienda { get; set; }
        [Required]
        [Display(Name = "Altri prestiti in corso")]
        public virtual TipologiaPrestito altroPrestito { get; set; }
        [Required]
        [Display(Name = "Fonte pubbl.")]
        public virtual FontePubblicitaria fontePubblicitaria { get; set; }
        public DateTime dataInserimento { get; set; }
        public String utenteInserimento { get; set; }
        [Display(Name = "Nota")]
        public ICollection<Nota> note { get; set; }
        [Display(Name = "Preventivo")]
        public ICollection<Preventivo> preventivi { get; set; }
        [Display(Name = "Stato")]
        public Stato stato { get; set; }



        [Display(Name = "Documenti")]
        public ICollection<Documento> documenti { get; set; }

    }

   

    public class SegnalazioneCreate
    {
        public Segnalazione segnalazione { set; get; }
        [Display(Name = "Impieghi")]
        public ICollection<Impiego> impieghi { set; get; }
        [Display(Name = "Riferimenti")]
        public ICollection<Riferimento> riferimenti { set; get; }
        [Display(Name="Nota")]
        public ICollection<Nota> note { get; set; }
    }






}