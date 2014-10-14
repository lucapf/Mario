using mediatori.Models;
using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Controllers.Business.Tipologia
{
    public class TipoLuogoRitrovoBusiness
    {
        public static ICollection<TipoLuogoRitrovo> valorizzaDatiTipoLuogoRitrovo(ICollection<TipoLuogoRitrovo> listaTipoLuogoRitrovo, MainDbContext db)
        {
            if (listaTipoLuogoRitrovo != null)
            {
                foreach (TipoLuogoRitrovo r in listaTipoLuogoRitrovo)
                {
                    valorizzaDatiTipoLuogoRitrovo(r, db);
                }
            }
            return listaTipoLuogoRitrovo;
        }

        public static TipoLuogoRitrovo valorizzaDatiTipoLuogoRitrovo(TipoLuogoRitrovo r, MainDbContext db)
        {
            return db.TipoLuogoRitrovo.Find(r.id);
        }
    }
}