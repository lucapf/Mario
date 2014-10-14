using mediatori.Models;
using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Controllers.Business.Tipologia
{
    public class FontePubblicitariaBusiness
    {
        public static ICollection<FontePubblicitaria> valorizzaDatiFontePubblicitaria(ICollection<FontePubblicitaria> listaFontePubblicitaria, MainDbContext db)
        {
            if (listaFontePubblicitaria != null)
            {
                foreach (FontePubblicitaria r in listaFontePubblicitaria)
                {
                    valorizzaDatiFontePubblicitaria(r, db);
                }
            }
            return listaFontePubblicitaria;
        }

        public static FontePubblicitaria valorizzaDatiFontePubblicitaria(FontePubblicitaria r, MainDbContext db)
        {
            return db.FontiPubblicitarie.Find(r.id);
        }
    }
}