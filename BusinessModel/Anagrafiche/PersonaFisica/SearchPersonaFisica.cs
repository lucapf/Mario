using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Anagrafiche.PersonaFisica
{
    public class SearchPersonaFisica : PagedPersonaFisica
    {
        //Filtri di ricerca
       public  PersonaFisicaManager.TipoPersonaFisica tipoPersonaFisica { get; set; }
       public string nome { get; set; }
       public string cognome { get; set; }
       public string codiceFiscale { get; set; }
    }
}
