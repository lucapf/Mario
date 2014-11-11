using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mediatori.Models.Anagrafiche
{
    [Table("persona_fisica")]
    public class PersonaFisicaDatiBase
    {
        public PersonaFisicaDatiBase()
        {
            cittadinanza = "ITALIANA";
            nazioneNascita = "ITALIA";
        }

        [Key]
        public int id { get; set; }
        [Required]
        [Display(Name = "Nome")]
        public String nome { get; set; }
        [Required]
        [Display(Name = "Cognome")]
        public String cognome { get; set; }
        [Required]
        [Display(Name = "Sesso")]
        public EnumSesso sesso { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data Nascita")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? dataNascita { get; set; }

        [Required]
        [Display(Name = "Stato Civile")]
        public EnumStatoCivile statoCivile { get; set; }

        [Required]
        [Display(Name = "Codice Fiscale")]
        [RegularExpression("[A-Z]{6}[0-9]{2}[A-Z][0-9]{2}[A-Z][0-9]{3}[A-Z]", ErrorMessage = "verificare la correttezza del codice fiscale")]
        public String codiceFiscale { get; set; }

        [Display(Name = "Nazione di Nascita")]
        public String nazioneNascita { get; set; }

        [Required]
        [Display(Name = "Cittadinanza")]
        public String cittadinanza { get; set; }

        [Display(Name = "Riferimenti")]
        public ICollection<Riferimento> riferimenti { get; set; }
    }
    #region Contatto
    [Table("contatto")]
    public class Contatto : PersonaFisicaDatiBase
    {
        [Required]
        [Display(Name = "Impieghi")]
        public ICollection<Impiego> impieghi { get; set; }
    }
    #endregion

    #region personaFisica
    public class PersonaFisica : PersonaFisicaDatiBase
    {

        [Required]
        [Display(Name = "Provincia nascita")]
        public Provincia provinciaNascita { get; set; }

        [Required]
        [Display(Name = "Comune nascita")]
        public Comune comuneNascita { get; set; }

        [Required]
        [Display(Name = "Indirizzi")]
        public ICollection<Indirizzo> indirizzi { get; set; }

        [Required]
        [Display(Name = "Impieghi")]
        public ICollection<Impiego> impieghi { get; set; }
        public String toString()
        {
            return id.ToString();
        }

    }
    #endregion


    #region Cedente
    [Table("cedente")]
    public class Cedente : PersonaFisica
    {

        [Required]
        [Display(Name = "Documento identita")]
        public ICollection<DocumentoIdentita> documentoIdentita { get; set; }

    }
    #endregion


    #region
    [Table("legale_rappresentante")]
    public class legaleRappresentante : PersonaFisicaDatiBase
    {
    }
    #endregion
}