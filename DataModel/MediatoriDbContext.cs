namespace DataModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MediatoriDbContext : DbContext
    {
        public MediatoriDbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer<MediatoriDbContext>(new CreateDatabaseIfNotExists<MediatoriDbContext>());
            //Database.SetInitializer<MediatoriDbContext>(new DropCreateDatabaseIfModelChanges<MediatoriDbContext>());
            //Database.SetInitializer<MediatoriDbContext>(new DropCreateDatabaseAlways<MediatoriDbContext>());
            //Database.SetInitializer<MediatoriDbContext>(new SchoolDBInitializer());
        }

        //public virtual DbSet<agenzia> agenzia { get; set; }
        //public virtual DbSet<amministrazione> amministrazione { get; set; }
        //public virtual DbSet<assegnazione> assegnazione { get; set; }
        //public virtual DbSet<cedente> cedente { get; set; }
        //public virtual DbSet<comune> comune { get; set; }
        //public virtual DbSet<contatto> contatto { get; set; }
        //public virtual DbSet<documento> documento { get; set; }
        //public virtual DbSet<documento_identita> documento_identita { get; set; }
        //public virtual DbSet<Events> Events { get; set; }
        //public virtual DbSet<gruppo_lavorazione> gruppo_lavorazione { get; set; }
        //public virtual DbSet<impiego> impiego { get; set; }
        //public virtual DbSet<indirizzo> indirizzo { get; set; }
        //public virtual DbSet<log_evento> log_evento { get; set; }
        //public virtual DbSet<nota> nota { get; set; }
        //public virtual DbSet<Parametro> Parametro { get; set; }
        //public virtual DbSet<preventivo> preventivo { get; set; }
        //public virtual DbSet<provincia> provincia { get; set; }
        //public virtual DbSet<Riferimento> Riferimento { get; set; }
        //public virtual DbSet<Segnalazione> Segnalazione { get; set; }
        //public virtual DbSet<soggetto_giuridico> soggetto_giuridico { get; set; }
        //public virtual DbSet<stato> stato { get; set; }
        //public virtual DbSet<tipo_agenzia> tipo_agenzia { get; set; }
        //public virtual DbSet<tipo_assumibilita_amministrazione> tipo_assumibilita_amministrazione { get; set; }
        //public virtual DbSet<tipo_azienda> tipo_azienda { get; set; }
        //public virtual DbSet<tipo_campagna_pubblicitaria> tipo_campagna_pubblicitaria { get; set; }
        //public virtual DbSet<tipo_canale_acquisizione> tipo_canale_acquisizione { get; set; }
        //public virtual DbSet<Tipo_categoria_amministrazione> Tipo_categoria_amministrazione { get; set; }
        //public virtual DbSet<Tipo_categoria_impiego> Tipo_categoria_impiego { get; set; }
        //public virtual DbSet<tipo_contatto> tipo_contatto { get; set; }
        //public virtual DbSet<tipo_contratto_impiego> tipo_contratto_impiego { get; set; }
        //public virtual DbSet<tipo_documento> tipo_documento { get; set; }
        //public virtual DbSet<tipo_documento_identita> tipo_documento_identita { get; set; }
        //public virtual DbSet<tipo_ente_rilascio> tipo_ente_rilascio { get; set; }
        //public virtual DbSet<tipo_erogazione> tipo_erogazione { get; set; }
        //public virtual DbSet<tipo_fonte_pubblicitaria> tipo_fonte_pubblicitaria { get; set; }
        //public virtual DbSet<tipo_indirizzo> tipo_indirizzo { get; set; }
        //public virtual DbSet<tipo_luogo_ritrovo> tipo_luogo_ritrovo { get; set; }
        //public virtual DbSet<tipo_natura_giuridica> tipo_natura_giuridica { get; set; }
        //public virtual DbSet<tipo_prestito> tipo_prestito { get; set; }
        //public virtual DbSet<tipo_prodotto> tipo_prodotto { get; set; }
        //public virtual DbSet<tipo_riferimento> tipo_riferimento { get; set; }
        //public virtual DbSet<toponimo> toponimo { get; set; }
        //public virtual DbSet<UserProfile> UserProfile { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<amministrazione>()
        //        .HasMany(e => e.amministrazione1)
        //        .WithOptional(e => e.amministrazione2)
        //        .HasForeignKey(e => e.pagante_id);

        //    modelBuilder.Entity<cedente>()
        //        .HasMany(e => e.documento_identita)
        //        .WithOptional(e => e.cedente)
        //        .HasForeignKey(e => e.Cedente_id);

        //    modelBuilder.Entity<cedente>()
        //        .HasMany(e => e.impiego)
        //        .WithOptional(e => e.cedente)
        //        .HasForeignKey(e => e.Cedente_id);

        //    modelBuilder.Entity<cedente>()
        //        .HasMany(e => e.indirizzo)
        //        .WithOptional(e => e.cedente)
        //        .HasForeignKey(e => e.Cedente_id);

        //    modelBuilder.Entity<cedente>()
        //        .HasMany(e => e.Riferimento)
        //        .WithOptional(e => e.cedente)
        //        .HasForeignKey(e => e.Cedente_id);

        //    modelBuilder.Entity<comune>()
        //        .HasMany(e => e.cedente)
        //        .WithRequired(e => e.comune)
        //        .HasForeignKey(e => e.comuneNascita_id)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<comune>()
        //        .HasMany(e => e.documento_identita)
        //        .WithRequired(e => e.comune)
        //        .HasForeignKey(e => e.comuneEnte_id)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<comune>()
        //        .HasMany(e => e.indirizzo)
        //        .WithRequired(e => e.comune)
        //        .HasForeignKey(e => e.comune_id)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<contatto>()
        //        .HasMany(e => e.impiego)
        //        .WithOptional(e => e.contatto)
        //        .HasForeignKey(e => e.Contatto_id);

        //    modelBuilder.Entity<contatto>()
        //        .HasMany(e => e.Riferimento)
        //        .WithOptional(e => e.contatto)
        //        .HasForeignKey(e => e.Contatto_id);

        //    modelBuilder.Entity<contatto>()
        //        .HasMany(e => e.Segnalazione)
        //        .WithRequired(e => e.contatto)
        //        .HasForeignKey(e => e.contatto_id)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<gruppo_lavorazione>()
        //        .HasMany(e => e.stato)
        //        .WithOptional(e => e.gruppo_lavorazione)
        //        .HasForeignKey(e => e.gruppoLavorazione_id);

        //    modelBuilder.Entity<provincia>()
        //        .HasMany(e => e.cedente)
        //        .WithRequired(e => e.provincia)
        //        .HasForeignKey(e => e.provinciaNascita_sigla)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<provincia>()
        //        .HasMany(e => e.comune)
        //        .WithOptional(e => e.provincia)
        //        .HasForeignKey(e => e.provincia_sigla);

        //    modelBuilder.Entity<provincia>()
        //        .HasMany(e => e.documento_identita)
        //        .WithRequired(e => e.provincia)
        //        .HasForeignKey(e => e.provinciaEnte_sigla)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<provincia>()
        //        .HasMany(e => e.indirizzo)
        //        .WithRequired(e => e.provincia)
        //        .HasForeignKey(e => e.provincia_sigla)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<Segnalazione>()
        //        .HasMany(e => e.assegnazione)
        //        .WithRequired(e => e.Segnalazione)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<Segnalazione>()
        //        .HasMany(e => e.documento)
        //        .WithRequired(e => e.Segnalazione)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<Segnalazione>()
        //        .HasMany(e => e.documento_identita)
        //        .WithOptional(e => e.Segnalazione)
        //        .HasForeignKey(e => e.Pratica_id);

        //    modelBuilder.Entity<Segnalazione>()
        //        .HasMany(e => e.Events)
        //        .WithOptional(e => e.Segnalazione)
        //        .HasForeignKey(e => e.segnalazione_id);

        //    modelBuilder.Entity<Segnalazione>()
        //        .HasMany(e => e.impiego)
        //        .WithOptional(e => e.Segnalazione)
        //        .HasForeignKey(e => e.Pratica_id);

        //    modelBuilder.Entity<Segnalazione>()
        //        .HasMany(e => e.indirizzo)
        //        .WithOptional(e => e.Segnalazione)
        //        .HasForeignKey(e => e.Pratica_id);

        //    modelBuilder.Entity<Segnalazione>()
        //        .HasMany(e => e.nota)
        //        .WithOptional(e => e.Segnalazione)
        //        .HasForeignKey(e => e.Segnalazione_id);

        //    modelBuilder.Entity<Segnalazione>()
        //        .HasMany(e => e.preventivo)
        //        .WithOptional(e => e.Segnalazione)
        //        .HasForeignKey(e => e.Segnalazione_id);

        //    modelBuilder.Entity<soggetto_giuridico>()
        //        .HasMany(e => e.agenzia)
        //        .WithRequired(e => e.soggetto_giuridico)
        //        .HasForeignKey(e => e.soggettoGiuridico_id)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<soggetto_giuridico>()
        //        .HasMany(e => e.amministrazione)
        //        .WithRequired(e => e.soggetto_giuridico)
        //        .HasForeignKey(e => e.soggettoGiuridico_id)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<soggetto_giuridico>()
        //        .HasMany(e => e.indirizzo)
        //        .WithOptional(e => e.soggetto_giuridico)
        //        .HasForeignKey(e => e.SoggettoGiuridico_id);

        //    modelBuilder.Entity<soggetto_giuridico>()
        //        .HasMany(e => e.nota)
        //        .WithOptional(e => e.soggetto_giuridico)
        //        .HasForeignKey(e => e.SoggettoGiuridico_id);

        //    modelBuilder.Entity<soggetto_giuridico>()
        //        .HasMany(e => e.preventivo)
        //        .WithRequired(e => e.soggetto_giuridico)
        //        .HasForeignKey(e => e.assicurazioneImpiegoId)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<soggetto_giuridico>()
        //        .HasMany(e => e.preventivo1)
        //        .WithRequired(e => e.soggetto_giuridico1)
        //        .HasForeignKey(e => e.assicurazioneVitaId)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<soggetto_giuridico>()
        //        .HasMany(e => e.preventivo2)
        //        .WithRequired(e => e.soggetto_giuridico2)
        //        .HasForeignKey(e => e.finanziariaId)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<soggetto_giuridico>()
        //        .HasMany(e => e.Riferimento)
        //        .WithOptional(e => e.soggetto_giuridico)
        //        .HasForeignKey(e => e.SoggettoGiuridico_id);

        //    modelBuilder.Entity<stato>()
        //        .HasMany(e => e.agenzia)
        //        .WithOptional(e => e.stato)
        //        .HasForeignKey(e => e.stato_id);

        //    modelBuilder.Entity<stato>()
        //        .HasMany(e => e.amministrazione)
        //        .WithRequired(e => e.stato)
        //        .HasForeignKey(e => e.stato_id)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<stato>()
        //        .HasMany(e => e.assegnazione)
        //        .WithRequired(e => e.stato)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<stato>()
        //        .HasMany(e => e.Segnalazione)
        //        .WithOptional(e => e.stato)
        //        .HasForeignKey(e => e.stato_id);

        //    modelBuilder.Entity<stato>()
        //        .HasMany(e => e.stato1)
        //        .WithMany(e => e.stato2)
        //        .Map(m => m.ToTable("StatoStatoes").MapLeftKey("Stato_id").MapRightKey("Stato_id1"));

        //    modelBuilder.Entity<tipo_agenzia>()
        //        .HasMany(e => e.agenzia)
        //        .WithOptional(e => e.tipo_agenzia)
        //        .HasForeignKey(e => e.tipoAgenzia_id);

        //    modelBuilder.Entity<tipo_assumibilita_amministrazione>()
        //        .HasMany(e => e.amministrazione)
        //        .WithRequired(e => e.tipo_assumibilita_amministrazione)
        //        .HasForeignKey(e => e.assumibilita_id)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<tipo_azienda>()
        //        .HasMany(e => e.Segnalazione)
        //        .WithOptional(e => e.tipo_azienda)
        //        .HasForeignKey(e => e.tipoAzienda_id);

        //    modelBuilder.Entity<tipo_campagna_pubblicitaria>()
        //        .HasMany(e => e.Segnalazione)
        //        .WithOptional(e => e.tipo_campagna_pubblicitaria)
        //        .HasForeignKey(e => e.campagnaPubblicitaria_id);

        //    modelBuilder.Entity<tipo_canale_acquisizione>()
        //        .HasMany(e => e.Segnalazione)
        //        .WithOptional(e => e.tipo_canale_acquisizione)
        //        .HasForeignKey(e => e.canaleAcquisizione_id);

        //    modelBuilder.Entity<Tipo_categoria_amministrazione>()
        //        .HasMany(e => e.amministrazione)
        //        .WithRequired(e => e.Tipo_categoria_amministrazione)
        //        .HasForeignKey(e => e.tipoCategoria_id)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<Tipo_categoria_impiego>()
        //        .HasMany(e => e.impiego)
        //        .WithOptional(e => e.Tipo_categoria_impiego)
        //        .HasForeignKey(e => e.categoriaImpiego_id);

        //    modelBuilder.Entity<tipo_contatto>()
        //        .HasMany(e => e.Segnalazione)
        //        .WithOptional(e => e.tipo_contatto)
        //        .HasForeignKey(e => e.tipoContatto_id);

        //    modelBuilder.Entity<tipo_contratto_impiego>()
        //        .HasMany(e => e.impiego)
        //        .WithOptional(e => e.tipo_contratto_impiego)
        //        .HasForeignKey(e => e.tipoImpiego_id);

        //    modelBuilder.Entity<tipo_documento>()
        //        .HasMany(e => e.documento)
        //        .WithRequired(e => e.tipo_documento)
        //        .HasForeignKey(e => e.tipoDocumento_id)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<tipo_ente_rilascio>()
        //        .HasMany(e => e.documento_identita)
        //        .WithRequired(e => e.tipo_ente_rilascio)
        //        .HasForeignKey(e => e.enteRilascio_id)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<tipo_fonte_pubblicitaria>()
        //        .HasMany(e => e.Segnalazione)
        //        .WithRequired(e => e.tipo_fonte_pubblicitaria)
        //        .HasForeignKey(e => e.fontePubblicitaria_id)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<tipo_indirizzo>()
        //        .HasMany(e => e.indirizzo)
        //        .WithRequired(e => e.tipo_indirizzo)
        //        .HasForeignKey(e => e.tipoIndirizzo_id)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<tipo_luogo_ritrovo>()
        //        .HasMany(e => e.Segnalazione)
        //        .WithOptional(e => e.tipo_luogo_ritrovo)
        //        .HasForeignKey(e => e.tipoLuogoRitrovo_id);

        //    modelBuilder.Entity<tipo_natura_giuridica>()
        //        .HasMany(e => e.agenzia)
        //        .WithRequired(e => e.tipo_natura_giuridica)
        //        .HasForeignKey(e => e.tipoNaturaGiuridica_id)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<tipo_natura_giuridica>()
        //        .HasMany(e => e.amministrazione)
        //        .WithRequired(e => e.tipo_natura_giuridica)
        //        .HasForeignKey(e => e.tipoNaturaGiuridica_id)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<tipo_prestito>()
        //        .HasMany(e => e.Segnalazione)
        //        .WithRequired(e => e.tipo_prestito)
        //        .HasForeignKey(e => e.altroPrestito_id)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<tipo_prodotto>()
        //        .HasMany(e => e.Segnalazione)
        //        .WithOptional(e => e.tipo_prodotto)
        //        .HasForeignKey(e => e.prodottoRichiesto_id);

        //    modelBuilder.Entity<tipo_riferimento>()
        //        .HasMany(e => e.Riferimento)
        //        .WithRequired(e => e.tipo_riferimento)
        //        .HasForeignKey(e => e.tipoRiferimento_id)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<toponimo>()
        //        .HasMany(e => e.indirizzo)
        //        .WithRequired(e => e.toponimo)
        //        .HasForeignKey(e => e.toponimo_sigla)
        //        .WillCascadeOnDelete(false);
        //}
    }
}
