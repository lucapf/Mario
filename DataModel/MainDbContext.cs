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
            Database.SetInitializer<MainDbContext>(new CreateDatabaseIfNotExists<MainDbContext>());
            // Console.WriteLine("Costruttore con parametro MainDbContext: " + url);
        }

        public MainDbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer<MainDbContext>(new CreateDatabaseIfNotExists<MainDbContext>());

            //    Database.SetInitializer(new MigrateDatabaseToLatestVersion<MainDbContext, Configuration>());

            // Database.SetInitializer<MainDbContext>(new DropCreateDatabaseAlways<MainDbContext>());
            //Database.SetInitializer<SchoolDBContext>(new DropCreateDatabaseIfModelChanges<SchoolDBContext>());
            //Database.SetInitializer<SchoolDBContext>(new DropCreateDatabaseAlways<SchoolDBContext>());
            //Database.SetInitializer<SchoolDBContext>(new SchoolDBInitializer());

            //Console.WriteLine("Costruttore MainDbContext");

        }

        public static string getConnectionByUrl(string url)
        {

            return "DefaultConnection";
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

        // public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Toponimo> Toponimi { get; set; }
        public DbSet<Indirizzo> Indirizzi { get; set; }
        public DbSet<Provincia> Province { get; set; }
        public DbSet<Comune> Comuni { get; set; }
        public DbSet<Anagrafiche.Segnalazione> Segnalazioni { get; set; }
        public DbSet<Anagrafiche.FontePubblicitaria> FontiPubblicitarie { get; set; }
        public DbSet<Anagrafiche.TipologiaAzienda> TipoAzienda { get; set; }
        public DbSet<Anagrafiche.TipologiaPrestito> TipoPrestito { get; set; }
        public DbSet<Anagrafiche.TipologiaIndirizzo> TipoIndirizzo { get; set; }
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
        public DbSet<GruppoLavorazione> GruppiLavorazione { get; set; }
        public DbSet<Stato> StatiSegnalazione { get; set; }
        public DbSet<Amministrazione> Amministazioni { get; set; }
        public DbSet<Event> Eventi { get; set; }
        public DbSet<Preventivo> Preventivi { get; set; }
        public DbSet<Pratica.Pratica> Pratiche { get; set; }

        public DbSet<Documento> Documenti { get; set; }
        public DbSet<TipoDocumento> TipoDocumenti { get; set; }

        public DbSet<etc.Assegnazione> Assegnazioni { get; set; }

        public DbSet<Contatto> Contatti { get; set; }
        public DbSet<Cedente> Cedenti { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();



            modelBuilder.Entity<PersonaFisica>().Map<Cedente>(m =>  m.Requires("tipoPersonaFisica").HasValue("Cedente")).Map<Contatto>(m => m.Requires("tipoPersonaFisica").HasValue("Contatto"));

            modelBuilder.Entity<PreventivoSmall>().Map<Preventivo>(m =>  m.Requires("Tipo").HasValue("Segnalazione"));
            

            //modelBuilder.Entity<Pratica.Pratica>().HasRequired( c => c.cedente).WithRequiredDependent().Map( d => d.MapKey ("contatto_id").ToTable("persona_fisica") );

         
            //modelBuilder.Entity<PersonaFisica>().;


            //  modelBuilder.Entity<Anagrafiche.Segnalazione>().;



            //.Map<BankAccount>(m => m.Requires("BillingDetailType").HasValue("BA"))
            //  .Map<CreditCard>(m => m.Requires("BillingDetailType").HasValue("CC"));

            //modelBuilder.Entity<Preventivo>().HasRequired(p => p.assicurazioneImpiego).WithRequiredDependent().WillCascadeOnDelete(false);
            //modelBuilder.Entity<Preventivo>().HasRequired(p => p.assicurazioneVita).WithRequiredDependent().WillCascadeOnDelete(false);
            //modelBuilder.Entity<Preventivo>().HasRequired(p => p.finanziaria).WithRequiredDependent().WillCascadeOnDelete(false);



            //modelBuilder.Entity<Preventivo >()
            // .HasMany(e => e.pre)
            // .WithRequired(e => e.soggetto_giuridico)
            // .HasForeignKey(e => e.assicurazioneImpiegoId)
            // .WillCascadeOnDelete(false);

            //modelBuilder.Entity<SoggettoGiuridico>()
            //    .HasMany(e => e.preventivo1)
            //    .WithRequired(e => e.soggetto_giuridico1)
            //    .HasForeignKey(e => e.assicurazioneVitaId)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<SoggettoGiuridico>()
            //    .HasMany(e => e.preventivo2)
            //    .WithRequired(e => e.soggetto_giuridico2)
            //    .HasForeignKey(e => e.finanziariaId)
            //    .WillCascadeOnDelete(false);

        }
    }
}