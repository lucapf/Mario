using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mediatori.Models.Anagrafiche
{
    public class Contatto: PersonaFisica
    {
        #region Contatto

        [Display(Name = "Impieghi")]
        public virtual ICollection<Impiego> impieghi { get; set; }

        [Display(Name = "Riferimenti")]
        public virtual ICollection<Riferimento> riferimenti { get; set; }

        #endregion
    }
}
