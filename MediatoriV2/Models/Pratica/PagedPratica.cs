using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Models.Pratica
{
    public class PagedPratica: MyManagerCSharp.Models.Paged
    {
        public IEnumerable<Pratica > Pratiche { get; set; }
    }
}