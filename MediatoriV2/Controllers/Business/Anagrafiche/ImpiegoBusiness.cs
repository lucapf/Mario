using mediatori.Models;
using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Controllers.Business.Anagrafiche.Soggetto
{
    public class ImpiegoBusiness
    {
        public static ICollection<Impiego> valorizzaDatiImpiego(ICollection<Impiego> listaImpieghi, MainDbContext db)
        {
            if (listaImpieghi != null)
            {
                foreach (Impiego impiego in listaImpieghi)
                {
                    if ( impiego.id==0)
                    {
                        valorizzaDatiImpiego(impiego, db);
                    }
                    
                }
            }
            return listaImpieghi;
        }

        public static Impiego valorizzaDatiImpiego(Impiego i, MainDbContext db)
        {
          
                i.tipoImpiego = db.TipoContrattoImpiego.Find(i.tipoImpiego.id);
                i.categoriaImpiego = db.TipoCategoriaImpiego.Find(i.categoriaImpiego.id);
                
               // if (i.dataLicenziamento. == 1) i.dataLicenziamento = new DateTime(3000, 01, 01);
                return i;
                  
        }

    }
}