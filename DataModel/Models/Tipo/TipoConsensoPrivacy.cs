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
    [Table("tipo_consenso_privacy")]
   public  class TipoConsensoPrivacy
    {
        [Key]
        [Display(Name = "codice")]
        public int id { get; set; }
        [Required]
        [Display(Name = "Descrizione")]
        public String descrizione { get; set; }
        [Required]
        [Display(Name = "Attivo")]
        public Boolean attivo { get; set; }

        [Display(Name = "Eliminabile")]
        public Boolean eliminabile{ get; set; }
    }
}
