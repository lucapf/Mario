using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
namespace mediatori.Models.Anagrafiche
{
    [Table("consenso_privacy")]
    public class ConsensoPrivacy
    {
        [Key]
        public int id { get; set; }

        [Required]
        [Display(Name = "Tipo Consenso Privacy")]
        public TipoConsensoPrivacy tipoConsensoPrivacy { get; set; }

        [Required]
        [Display(Name = "Acconsento")]
        public Boolean? acconsento { get; set; }
        
        [Required]
        [Display(Name = "Data Inserimento")]
        public DateTime dataInserimento { get; set; }

        [Required]
        [Display(Name = "Utente Inserimento")]
        public String untenteInserimento { get; set; }

       
        public int segnalazioneId { get; set; }
        public virtual Segnalazione segnalazione { get; set; }
    }
}
