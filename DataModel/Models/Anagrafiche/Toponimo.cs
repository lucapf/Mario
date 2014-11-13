using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace mediatori.Models.Anagrafiche
{
    [Table("toponimo")]
    public class Toponimo
    {
        [Key]
        [Required(ErrorMessage="il campo toponimo è obbligatorio")]
        public String sigla { get; set; }
        public String toString()
        {
            return sigla;
        }
    }
}
