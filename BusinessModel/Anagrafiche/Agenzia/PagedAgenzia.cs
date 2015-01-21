using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessModel.Anagrafiche.Agenzia
{
    public class PagedAgenzia : MyManagerCSharp.Models.Paged
    {
        public IEnumerable<mediatori.Models.Anagrafiche.Agenzia> Agenzie { get; set; }
    }
}