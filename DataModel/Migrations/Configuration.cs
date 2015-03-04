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
                new TipoConsensoPrivacy { id = 1, descrizione = "consenso memorizzazione supporto informatico", obbligatorio = true });

        }
    }
}
