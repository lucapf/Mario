using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Models.Anagrafiche
{
 
    [Table("test_anagrafiche")]
    public class TestAnagrafiche
    {
        [Key]
        [Display(Name = "TEST")]
        public int id { get; set; }

        public int codiceProvincia { get; set; }

        [Display(Name = "Denominazione")]
        [Required(ErrorMessage = "il comune è obbligatorio")]
        public String denominazione { get; set; }
    }
}
