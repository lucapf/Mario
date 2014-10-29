using mediatori.Models;
using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Controllers.Business.Tipologia
{
    public class TipoCanaleAcquisizioneBusiness
    {
        public static ICollection<TipoCanaleAcquisizione> valorizzaDatiTipoCanaleAcquisizione(ICollection<TipoCanaleAcquisizione> listaTipoCanaleAcquisizione, MainDbContext db)
        {
            if (listaTipoCanaleAcquisizione != null)
            {
                foreach (TipoCanaleAcquisizione r in listaTipoCanaleAcquisizione)
                {
                    valorizzaDatiTipoCanaleAcquisizione(r, db);
                }
            }
            return listaTipoCanaleAcquisizione;
        }

        public static TipoCanaleAcquisizione valorizzaDatiTipoCanaleAcquisizione(TipoCanaleAcquisizione r, MainDbContext db)
        {
            return db.TipoCanaleAcquisizione.Find(r.id);
        }
    }
}