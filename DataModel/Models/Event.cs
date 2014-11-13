using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mediatori.Models
{
    [Table("events")]
    public class Event
    {
        [Key]
        public int id { get; set; }
        [Display (Name="Data inizio evento")]
        public DateTime start_date { get; set; }
        [Display(Name="Data fine evento")]
        public DateTime end_date { get; set; }
        [Display(Name="testo")]
        public String testo { get; set; }
        [Display(Name="utente")]
        public String utente { get; set; }
        [Display(Name = "segnalazione")]
        public Anagrafiche.Segnalazione segnalazione { get; set; }
       


    }
}