using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mediatori.Models.Anagrafiche
{
    [Table("coordinate_erogazione")]
    public class CoordinateErogazione
    {
        [Key]
        public int id { get; set; }

        [Required]
        [Display(Name = "coordinata")]
        public String coordinata { get; set; }

        [Required]
        [Display(Name = "tipo coordinata erogazione")]
        public TipoCoordinataErogazione tipoCoordinataErogazione {get;set;}

        //[ForeignKey("Cedente")]
        public int? cedenteId { get; set; }
        public virtual Cedente cedente { get; set; }
    }
}
