using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mediatori.Models.Pratica
{
    public class Pratica : mediatori.Models.Anagrafiche.Segnalazione
    {
        [Required]
        [Display(Name = "Cedente")]
        public virtual Models.Anagrafiche.Cedente cedente { get; set; }
        public int cedenteId { get; set; }
        
        public virtual Models.Preventivo preventivoConfermato { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Data rinnovo")]
        [DisplayFormat(DataFormatString = MyConstants.DATE_FORMAT, ApplyFormatInEditMode = true)]
        public DateTime? dataRinnovo { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Data decorrenza")]
        [DisplayFormat(DataFormatString = MyConstants.DATE_FORMAT, ApplyFormatInEditMode = true)]
        public DateTime? dataDecorrenza{ get; set; }
 
    }
}