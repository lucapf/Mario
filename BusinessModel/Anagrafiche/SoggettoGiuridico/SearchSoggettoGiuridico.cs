using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Anagrafiche.SoggettoGiuridico
{
    public class SearchSoggettoGiuridico : PagedSoggettoGiuridico
    {
        //Filtri di ricerca
        public mediatori.Models.EnumTipoSoggettoGiuridico? tipoSoggettoSelezionato {get; set;}
        public string ragioneSociale {get; set;}
        public string codiceFiscale {get; set;}
    }
}
