using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Anagrafiche.SoggettoGiuridico
{
    public class PagedSoggettoGiuridico : MyManagerCSharp.Models.Paged
    {
        public IEnumerable<mediatori.Models.Anagrafiche.SoggettoGiuridico> SoggettiGiuridici { get; set; }
    }
}
