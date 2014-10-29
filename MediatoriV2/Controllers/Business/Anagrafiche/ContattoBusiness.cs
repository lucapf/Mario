using mediatori.Filters;
using mediatori.helper;
using mediatori.Models;
using mediatori.Models.Anagrafiche;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;

namespace mediatori.Controllers.Business.Anagrafiche
{
    public class ContattoBusiness
    {
        internal static ICollection<Contatto> findByFilter(ContattoFilter contattoFilter, Models.MainDbContext db)
        {
            var contatti = from c in db.Contatti select c;
            if (!String.IsNullOrWhiteSpace(contattoFilter.nome))
            {
                contatti = contatti.Where(c => c.nome.ToUpper().Contains(contattoFilter.nome.ToUpper()));
            }
            if (!String.IsNullOrWhiteSpace(contattoFilter.cognome))
            {
                contatti = contatti.Where(c => c.cognome.ToUpper().Contains(contattoFilter.cognome.ToUpper()));
            }
            if (!String.IsNullOrWhiteSpace(contattoFilter.codiceFiscale))
            {
                contatti = contatti.Where(c => c.codiceFiscale.ToUpper().Equals(contattoFilter.codiceFiscale.ToUpper()));
            }
            return contatti.ToList();
        }
        internal static String asHtml(ICollection<Contatto> contatti)
        {
            if (contatti == null || contatti.Count == 0)
            {
                return "<div>Nessun contatto candidato trovato</div>";

            }
            HtmlTable table = new HtmlTable();
            table.Attributes.Add("class", "listTable");
            HtmlTableRow intestazione = new HtmlTableRow();

            intestazione.Cells.Add(FireAntHtmlHelper.buildTH("Nome"));
            intestazione.Cells.Add(FireAntHtmlHelper.buildTH("Cognome"));
            intestazione.Cells.Add(FireAntHtmlHelper.buildTH("Codice Fiscale"));
            intestazione.Cells.Add(FireAntHtmlHelper.buildTH("e' Lui!"));
            table.Rows.Add(intestazione);
            foreach (Contatto contatto in contatti)
            {
                HtmlTableRow rowContatto = new HtmlTableRow();
                rowContatto.Cells.Add(FireAntHtmlHelper.buildCell(contatto.nome));
                rowContatto.Cells.Add(FireAntHtmlHelper.buildCell(contatto.cognome));
                rowContatto.Cells.Add(FireAntHtmlHelper.buildCell(contatto.codiceFiscale));
                TagBuilder tagBuilder = new TagBuilder("input");
                tagBuilder.Attributes.Add("type", "checkbox");
                tagBuilder.Attributes.Add("onclick", "contattoDetail(" + contatto.id + ")");
                rowContatto.Cells.Add(FireAntHtmlHelper.buildCell(tagBuilder));
                table.Rows.Add(rowContatto);
            }

            return FireAntHtmlHelper.renderControl(table);


        }




        internal Contatto findByPK(int codiceContatto, MainDbContext db)
        {
            if (codiceContatto == null) return null;
            ContattoInclude<Contatto> contattoInclude = new ContattoInclude<Contatto>();
            return     contattoInclude.addIncludeStatement(db.Contatti,null)
                              .Where(c => c.id == codiceContatto)
                              .FirstOrDefault();
        }


        internal Contatto copiaRiferimenti(Contatto contattoSrc, Contatto contattoTarget)
        {
            contattoTarget.impieghi = contattoSrc.impieghi;
            contattoTarget.riferimenti = contattoSrc.riferimenti;
            return contattoTarget;
        }
    }
    public class ContattoInclude<T>
    {


        public DbQuery<T> addIncludeStatement(DbQuery<T> dbQuery, String prefisso)
        {

            prefisso = (prefisso == null || prefisso==String.Empty)? "" : prefisso + ".";

            return dbQuery.Include(prefisso + "riferimenti")
            .Include(prefisso + "riferimenti.tipoRiferimento")
            .Include(prefisso + "Impieghi")
            .Include(prefisso + "Impieghi.tipoImpiego")
            .Include(prefisso + "Impieghi.categoriaImpiego");

        }
    }
}