using mediatori.Models;
using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Controllers.Business.Anagrafiche.Soggetto
{
    public class DocumentoIdentitaBusiness
    {
        public ICollection<DocumentoIdentita> valorizzaDatiPerInserimentoCancellazione(ICollection<DocumentoIdentita> listaDocumenti, MainDbContext db)
        {
            if (listaDocumenti != null)
            {
                foreach (DocumentoIdentita di in listaDocumenti){
                    valorizzaDatiPerInserimentoCancellazione(di, db);
                }
            }
            return listaDocumenti;
        }

        private DocumentoIdentita valorizzaDatiPerInserimentoCancellazione(DocumentoIdentita di, MainDbContext db)
        {
          
                di.provinciaEnte = (from p in db.Province where p.denominazione == di.provinciaEnte.denominazione select p).FirstOrDefault();
                di.comuneEnte = (from c in db.Comuni
                                 where c.denominazione == di.comuneEnte.denominazione &&
                                     c.provincia.sigla == di.provinciaEnte.sigla
                                 select c).FirstOrDefault();
                di.enteRilascio = (from e in db.TipoEnteRilascio where e.id == di.enteRilascio.id select e).FirstOrDefault();
                return di;
        }

    }
}