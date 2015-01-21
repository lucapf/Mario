using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Anagrafiche.PersonaFisica
{
    public class PagedPersonaFisica : MyManagerCSharp.Models.Paged
    {
        public IEnumerable<mediatori.Models.Anagrafiche.PersonaFisica> PersoneFisiche { get; set; }
    }
}
