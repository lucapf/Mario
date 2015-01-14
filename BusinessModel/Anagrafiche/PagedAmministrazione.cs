using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessModel.Anagrafiche
{
    public class PagedAmministrazione : MyManagerCSharp.Models.Paged
    {
        public IEnumerable<mediatori.Models.Anagrafiche.Amministrazione> Amministrazioni { get; set; }
    }
}