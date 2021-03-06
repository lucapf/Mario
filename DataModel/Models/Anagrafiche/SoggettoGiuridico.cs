﻿using mediatori.Models.etc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mediatori.Models.Anagrafiche
{
    [Table("soggetto_giuridico")]
    public class SoggettoGiuridico
    {
        [Key]
        public int id { get; set; }
        [Required]
        [Display(Name = "Ragione sociale")]
        public string ragioneSociale { get; set; }

        [Required]
        [Display(Name = "Tipo soggetto")]
        public String tipoSoggettoGiuridico { get; set; }

        [Required]
        [Display(Name = "CF")]
        //Contiene un CF oppure una partita iva
        [RegularExpression("([A-Z]{6}[0-9]{2}[A-Z][0-9]{2}[A-Z][0-9]{3}[A-Z])|([0-9]{11})", ErrorMessage = "Verificare la correttezza del codice fiscale")]
        public String codiceFiscale { get; set; }

        [Display(Name = "Indirizzi")]
        public ICollection<Indirizzo> indirizzi { get; set; }

        [Display(Name = "Riferimenti")]
        public ICollection<Riferimento> riferimenti { get; set; }

        [Display(Name = "Note")]
        public ICollection<Nota> note { get; set; }
    }
}