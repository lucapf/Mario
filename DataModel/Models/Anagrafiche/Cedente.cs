using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mediatori.Models.Anagrafiche
{
    public class Cedente : Contatto
    {


        #region Cedente

        [Required]
        [Display(Name = "Documento identita")]
        public virtual ICollection<DocumentoIdentita> documentiIdentita { get; set; }

        [Required]
        [Display(Name = "Indirizzi")]
        public virtual ICollection<Indirizzo> indirizzi { get; set; }

        #endregion

    }
}
