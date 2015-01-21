using mediatori.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Anagrafiche.Contatto
{
    public class ContattoManager
    {
        public static mediatori.Models.Anagrafiche.Contatto findByPK(int codiceContatto, MainDbContext db)
        {
            if (codiceContatto == 0)
            {
                return null;
            }
            mediatori.Models.Anagrafiche.Contatto contatto;
            contatto = db.Contatti.Include("provinciaNascita").Include("comuneNascita").Where(c => c.id == codiceContatto).FirstOrDefault();

            // ContattoInclude<Contatto> contattoInclude = new ContattoInclude<Contatto>();
            //return contattoInclude.addIncludeStatement(db.Contatti, null).Where(c => c.id == codiceContatto).FirstOrDefault();

            return contatto;
        }


        public static void valorizzaDati(mediatori.Models.Anagrafiche.Contatto c, MainDbContext db)
        {
            c.provinciaNascita = db.Province.Where(p => p.denominazione == c.provinciaNascita.denominazione).FirstOrDefault();
            c.comuneNascita = db.Comuni.Where(p => p.denominazione == c.comuneNascita.denominazione).FirstOrDefault();
        }
    }
}
