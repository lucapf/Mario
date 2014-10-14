using mediatori.Models;
using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Controllers.Business.Tipologia
{
    public class TipoPrestitoBusiness
    {
        public static ICollection<TipologiaPrestito> valorizzaDatiTipologiaPrestito(ICollection<TipologiaPrestito> listaTipologiePrestito, MainDbContext db)
        {
            if (listaTipologiePrestito != null)
            {
                foreach (TipologiaPrestito r in listaTipologiePrestito)
                {
                    valorizzaDatiTipologiaPrestito(r, db);
                }
            }
            return listaTipologiePrestito;
        }

        public static TipologiaPrestito valorizzaDatiTipologiaPrestito(TipologiaPrestito r, MainDbContext db)
        {
            return db.TipoPrestito.Find(r.id);
        }
    }
}