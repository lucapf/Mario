using mediatori.Models;
using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Controllers.Business.Tipologia
{
    public class TipoContattoBusiness
    {
        public static ICollection<TipoContatto> valorizzaDatiTipoContatto(ICollection<TipoContatto> listaTipoContatto, MainDbContext db)
        {
            if (listaTipoContatto != null)
            {
                foreach (TipoContatto r in listaTipoContatto)
                {
                    valorizzaDatiTipoContatto(r, db);
                }
            }
            return listaTipoContatto;
        }

        public static TipoContatto valorizzaDatiTipoContatto(TipoContatto r, MainDbContext db)
        {
            return db.TipoContatto.Find(r.id);
        }
    }
}