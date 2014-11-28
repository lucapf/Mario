using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Models
{
    public class StatoModel
    {
        public List<mediatori.Models.etc.Stato> listaStati { get; set; }
        
        public mediatori.Models.etc.Stato stato { get; set; }

        //public System.Web.Mvc.SelectList listaStatiBase { get; set; }
        //public System.Web.Mvc.SelectList listaEntitaAssociate { get; set; }
        public List<MyUsers.Models.MyGroup > listaGruppi { get; set; }
    }
}