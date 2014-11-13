using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mediatori.Models.Anagrafiche
{
    [Table("indirizzo")]
    public class Indirizzo
    {
        public Indirizzo()
        {
            tipoIndirizzo = new TipologiaIndirizzo();
            toponimo = new Toponimo();
            provincia = new Provincia();
            comune = new Comune();
        }
        [Key]
        public int id { get; set; }

        [Required]
        [Display(Name="Tipo")]
        public TipologiaIndirizzo tipoIndirizzo { get; set; }

        [Required]
        [Display(Name="Toponimo")]
        public Toponimo toponimo { get; set; }

        [Required]
        [Display(Name="Indirizzo")]
        [StringLength(50)]
        public String recapito { get; set; }

        [Required]
        [RegularExpression("[0-9]{5}",ErrorMessage="il cap deve essere composto da 5 caratteri numerici")]
        [Display(Name="CAP")]
        public String cap { get; set; }

        [StringLength(50)]
        [Display(Name="Presso",ShortName="C/O")]
        public String presso { get; set; }

        [Required]
        [Display(Name="Nr.Civico")]
        public String numeroCivico { get; set; }

        [Required]
        [Display(Name="Provincia")]

        public Provincia provincia { get; set; }
        [Required]
        [Display(Name="Comune")]
        public Comune comune { get; set; }
        
        [StringLength(10)]
        [Display(Name="Interno")]
        public String interno { get; set; }

        [Display(Name="Corrispondenza")]
        public Boolean corrispondenza { get; set; }

                
    }
}