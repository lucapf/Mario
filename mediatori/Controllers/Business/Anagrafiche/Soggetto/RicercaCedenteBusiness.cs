using mediatori.Models;
using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mediatori.Controllers.Business.Anagrafiche.Soggetto
{
    public static class RicercaCedenteBusiness
    {
        public static Cedente find(int id, MainDbContext db)
        {
            Cedente cedente  = (from c in db.Cedenti.Include("provinciaNascita")
                                                    .Include("comuneNascita")
                                                    .Include("Indirizzi")
                                                    .Include("Indirizzi.comune")
                                                    .Include("Indirizzi.provincia")
                                                    .Include("Indirizzi.tipoIndirizzo")
                                                    .Include("Indirizzi.toponimo")
                                                    .Include("Impieghi")
                                                    .Include("Impieghi.tipoImpiego")
                                                    .Include("DocumentoIdentita")
                                                    .Include("DocumentoIdentita.enteRilascio")
                                                    .Include("DocumentoIdentita.provinciaEnte")
                                                    .Include("DocumentoIdentita.comuneEnte")
                                where c.id==id select c ).FirstOrDefault<Cedente>();
            return cedente;
        }
        public static Cedente findDatiGenerali(int id, MainDbContext db)
        {
            Cedente cedente = (from c in db.Cedenti.Include("provinciaNascita")
                                                   .Include("comuneNascita")
                               where c.id == id
                               select c).FirstOrDefault<Cedente>();
            return cedente;
        }
    }
}