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
     public enum EnumPersonaFisica
    {
         Contatto,
         Cedente
     }
    

    [Table("persona_fisica")]
    public abstract  class PersonaFisica
    {
        public PersonaFisica()
        {
            cittadinanza = "ITALIANA";
            nazioneNascita = "ITALIA";
        }

        [Key]
        public int id { get; set; }

        //[Required]
        //public String tipoPersonaFisica { get; set; }

        [Required]
        [Display(Name = "Nome")]
        public String nome { get; set; }

        [Required]
        [Display(Name = "Cognome")]
        public String cognome { get; set; }

        [Required]
        [Display(Name = "Sesso")]
        public EnumSesso? sesso { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data Nascita")]
        [DisplayFormat(DataFormatString = MyConstants.DATE_FORMAT, ApplyFormatInEditMode = true)]
        public DateTime? dataNascita { get; set; }

        [Required]
        [Display(Name = "Stato Civile")]
        public EnumStatoCivile? statoCivile { get; set; }

        [Required]
        [Display(Name = "Codice Fiscale")]
        [RegularExpression("[A-Z]{6}[0-9]{2}[A-Z][0-9]{2}[A-Z][0-9]{3}[A-Z]", ErrorMessage = "verificare la correttezza del codice fiscale")]
        public String codiceFiscale { get; set; }

        [Display(Name = "Nazione di Nascita")]
        public String nazioneNascita { get; set; }

        [Required]
        [Display(Name = "Provincia nascita")]
        public Provincia provinciaNascita { get; set; }

        [Required]
        [Display(Name = "Comune nascita")]
        public Comune comuneNascita { get; set; }

        [Required]
        [Display(Name = "Cittadinanza")]
        public String cittadinanza { get; set; }


       


    }






}