using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mediatori.Models.Anagrafiche
{
      [Table("anagrafica_rete")]
    public class AnagraficaRete
    {
          [Key]
          public int id {get;set;}
          [Required]
          [Display(Name="Sigla")]
          public String sigla { get; set; }
          [Required]
          [Display(Name="Rete")]
          public ICollection<ElementoRete> listaElementiRete { get; set; }
          
    }
}