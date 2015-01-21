using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mediatori.Models.Anagrafiche
{
    [Table("provincia")]
    public class Provincia
    {
        [Key]
        [Required(ErrorMessage = "la provincia è obbligatoria")]
        public String sigla { get; set; }
        
        [Required(ErrorMessage = "la provincia è obbligatoria")]
        public int id { get; set; }

        [Required(ErrorMessage = "la provincia è obbligatoria")]
        public String denominazione { get; set; }

        public virtual ICollection<Comune> comuni { get; set; }

        public String toString()
        {
            return this.sigla;
        }
    }

    //public class ProvinciaSearchResult
    //{
    //    public String sigla { get; set; }
    //    public int id { get; set; }
    //    public String denominazione { get; set; }
    //}

    //public class ProvinciaFilter
    //{
    //    public String denominazione { get; set; }
    //}
}