using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mediatori.Models.Anagrafiche
{   [Table("comune")]
    public class Comune
    {
    [Key]
    [Display(Name="Comune")]
    public int id { get; set; }
    public int codiceProvincia { get; set; }
    [Display(Name="Denominazione")]
    [Required(ErrorMessage="il comune è obbligatorio")]
    public String denominazione { get; set; }
    public Provincia provincia { get; set; }
    
    }
    public class ComuneSearch{
        public int codiceProvincia { get; set; }
        public String denominazione { get; set; }
        public int id { get; set; }
    }
}