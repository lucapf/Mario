using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mediatori.Models.Anagrafiche
{
    [Table("riferimento")]
    public class Riferimento
    {
        [Key]
        public int id { get; set; }

        [Required]
        [Display(Name="Tipo Riferimento")]
        public TipoRiferimento tipoRiferimento { get; set; }

        [Required]
        [Display(Name = "valore")]
        public String valore { get; set; }


        //http://msdn.microsoft.com/en-us/data/jj713564.aspx
        public int? contattoId { get; set; }
        public virtual Contatto contatto { get; set; }

        public int? soggettoGiuridicoId { get; set; }
        public virtual SoggettoGiuridico soggettoGiuridico { get; set; }

            
    }
}