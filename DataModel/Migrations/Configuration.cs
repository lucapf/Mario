namespace DataModel.Migrations
{
    using mediatori.Models.Anagrafiche;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<mediatori.Models.MainDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(mediatori.Models.MainDbContext context)
        {

            context.TipoConsensoPrivacy.AddOrUpdate<TipoConsensoPrivacy>(t => t.id,
                new TipoConsensoPrivacy { id = 1, descrizione = "Autorizzo il trattamento dei miei dati personali, ai sensi del D.lgs. 196 del 30 giugno 2003", isSystem = true, obbligatorio = true, attivo = true });
            context.TipoCoordinataErogazione.AddOrUpdate<TipoCoordinataErogazione>(t => t.sigla,
               new TipoCoordinataErogazione { sigla = "ASS", descrizione = "Assegno" });
            context.TipoCoordinataErogazione.AddOrUpdate<TipoCoordinataErogazione>(t => t.sigla,
              new TipoCoordinataErogazione { sigla = "OBB", descrizione = "Bonifico Bancario" });

        }
    }
}
