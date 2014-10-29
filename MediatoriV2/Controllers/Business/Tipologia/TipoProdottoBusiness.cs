using mediatori.Models;
using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Controllers.Business.Tipologia
{
    public class TipoProdottoBusiness
    {
        public static ICollection<TipoProdotto> valorizzaDatiTipoProdotto(ICollection<TipoProdotto> listaTipologieProdotto, MainDbContext db)
        {
            if (listaTipologieProdotto != null)
            {
                foreach (TipoProdotto r in listaTipologieProdotto)
                {
                    valorizzaDatiTipoProdotto(r, db);
                }
            }
            return listaTipologieProdotto;
        }

        public static TipoProdotto valorizzaDatiTipoProdotto(TipoProdotto r, MainDbContext db)
        {
            return db.TipoProdotto.Find(r.id);
        }
    }
}