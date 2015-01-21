using mediatori.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Anagrafiche.Cedente
{
    public class CedenteManager
    {
        public static mediatori.Models.Anagrafiche.Cedente findByPK(int codiceCedente, MainDbContext db)
        {
            if (codiceCedente == 0)
            {
                return null;
            }
            mediatori.Models.Anagrafiche.Cedente cedente;
            cedente = db.Cedenti.Include("provinciaNascita").Include("comuneNascita").Where(c => c.id == codiceCedente).FirstOrDefault();

            // ContattoInclude<Contatto> contattoInclude = new ContattoInclude<Contatto>();
            //return contattoInclude.addIncludeStatement(db.Contatti, null).Where(c => c.id == codiceContatto).FirstOrDefault();

            return cedente;
        }
    }
}
