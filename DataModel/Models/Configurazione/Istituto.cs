using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace mediatori.Models
{
    [Table("istituto")]
    public class Istituto
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string nome { get; set; }

        [Required]
        public string applicativo { get; set; }

        [Required]
        public string url { get; set; }

        [Required]
        public DateTime dataInserimento { get; set; }
    }
}
