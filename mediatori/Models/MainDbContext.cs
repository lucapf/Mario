using mediatori.Models.Anagrafiche;
using mediatori.Models.etc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace mediatori.Models
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(String url)
            // : base("DefaultConnection")
            : base(getConnectionByUrl(url))
        {

        }
        public MainDbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer<MainDbContext>(new CreateDatabaseIfNotExists<MainDbContext>());

        }

        public static string getConnectionByUrl(string url)
        {
            String defaultConnection = "DefaultConnection";
            if (url.Equals(String.Empty)) return defaultConnection;
            if (!(url.Contains("http://") || url.Contains("https://")))
                throw new Exception(String.Format("la url indicata ({0}) non è valida", url));
            //definisce la sringa di connessione
            string dominio = url.Replace("http://", String.Empty).Replace("https://", String.Empty);
            if (dominio == null) throw new Exception(String.Format("formato URL ({0}) non valido", url));
            if (dominio.IndexOf('.') <= 0) return defaultConnection;
            String sottodominio = dominio.Split('.')[0];
            if (sottodominio.Equals("localhost")) return defaultConnection;
            //le connessioni debbono avere lo stesso nome del sotto-dominio assegnato al cliente
            String connetionName = dominio.Split('.')[0];
            return connetionName;
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Toponimo> Toponimi { get; set; }
        public DbSet<Indirizzo> Indirizzi { get; set; }
        public DbSet<Provincia> Province { get; set; }
        public DbSet<Comune> Comuni { get; set; }
        public DbSet<Anagrafiche.Segnalazione> Segnalazioni { get; set; }
        public DbSet<Anagrafiche.FontePubblicitaria> FontiPubblicitarie { get; set; }
        public DbSet<Anagrafiche.TipologiaAzienda> TipoAzienda { get; set; }
        public DbSet<Anagrafiche.TipologiaPrestito> TipoPrestito { get; set; }
        public DbSet<Anagrafiche.TipologiaIndirizzo> TipoIndirizzo { get; set; }
        public DbSet<Anagrafiche.Cedente> Cedenti { get; set; }
        public DbSet<Anagrafiche.Contatto> Contatti { get; set; }
        public DbSet<Anagrafiche.SoggettoGiuridico> SoggettiGiuridici { get; set; }
        public DbSet<Anagrafiche.TipoCampagnaPubblicitaria> TipoCampagnaPubblicitaria { get; set; }
        public DbSet<Anagrafiche.TipoCanaleAcquisizione> TipoCanaleAcquisizione { get; set; }
        public DbSet<Anagrafiche.TipoLuogoRitrovo> TipoLuogoRitrovo { get; set; }
        public DbSet<Anagrafiche.TipoContatto> TipoContatto { get; set; }
        public DbSet<Anagrafiche.TipoRiferimento> TipoRiferimento { get; set; }
        public DbSet<Anagrafiche.TipoProdotto> TipoProdotto { get; set; }
        public DbSet<Anagrafiche.TipoNaturaGiuridica> tipoNaturaGiuridica { get; set; }
        public DbSet<Anagrafiche.TipoCategoriaAmministrazione> TipoCategoriaAmministrazione { get; set; }
        public DbSet<Anagrafiche.TipoAssumibilitaAmministrazione> TipoAssumibilitaAmministrazione { get; set; }
        public DbSet<Anagrafiche.TipoErogazione> TipoErogazione { get; set; }
        public DbSet<Anagrafiche.TipoAgenzia> TipoAgenzia { get; set; }
        public DbSet<Anagrafiche.Agenzia> Agenzia { get; set; }
        public DbSet<Models.etc.Parametro> Parametri { get; set; }
        public DbSet<TipoCategoriaImpiego> TipoCategoriaImpiego { get; set; }
        public DbSet<LogEventi> LogsEventi { get; set; }
        public DbSet<TipoContrattoImpiego> TipoContrattoImpiego { get; set; }
        public DbSet<Impiego> impieghi { get; set; }
        public DbSet<TipoEnteRilascio> TipoEnteRilascio { get; set; }
        public DbSet<DocumentoIdentita> DocumentiIdentita { get; set; }
        public DbSet<TipoDocumentoIdentita> TipoDocumentiIdentita { get; set; }
        public DbSet<Riferimento> Riferimento { get; set; }
        public DbSet<GruppoLavorazione> gruppiLavorazione { get; set; }
        public DbSet<Stato> statiSegnalazione { get; set; }
        public DbSet<Amministrazione> amministazioni { get; set; }
        public DbSet<Event> eventi { get; set; }
        public DbSet<Preventivo> preventivi { get; set; }

        public DbSet<Documento> Documenti { get; set; }
        public DbSet<TipoDocumento> TipoDocumenti { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Entity<Preventivo>()
       .HasRequired(p => p.assicurazioneImpiego)
       .WithRequiredDependent()
       .WillCascadeOnDelete(false);

            modelBuilder.Entity<Preventivo>()
                   .HasRequired(p => p.assicurazioneVita)
                   .WithRequiredDependent()
                   .WillCascadeOnDelete(false);

            modelBuilder.Entity<Preventivo>()
       .HasRequired(p => p.finanziaria)
       .WithRequiredDependent()
       .WillCascadeOnDelete(false);

        }
    }
}