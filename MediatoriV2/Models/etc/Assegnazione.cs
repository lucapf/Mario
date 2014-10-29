using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mediatori.Models.etc
{
    [Table("assegnazione")]
    public class Assegnazione
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int segnalazioneId { get; set; }
        public virtual Models.Anagrafiche.Segnalazione segnalazione { get; set; }

        [Required]
        public int statoId { get; set; }
        public virtual Models.etc.Stato stato { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data inserimento")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? dataInserimento { get; set; }

        [Required]
        [Display(Name = "Utente")]
        public string login { get; set; }
    }
}