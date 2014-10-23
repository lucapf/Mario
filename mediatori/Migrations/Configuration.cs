namespace mediatori.Migrations
{
    using mediatori.Models;
    using mediatori.Models.Anagrafiche;
    using mediatori.Models.etc;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;
    using WebMatrix.WebData;
    using System.Diagnostics;


    internal sealed class Configuration : DbMigrationsConfiguration<mediatori.Models.MainDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }


     

        protected override void Seed(mediatori.Models.MainDbContext context)
        {
            //TipoMigrazione tipoMigrazione = TipoMigrazione.INIZIALE;


           if (!WebSecurity.Initialized)
             WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);

            if (!Roles.RoleExists("Amministratore"))
                Roles.CreateRole("Amministratore");
            if (!Roles.RoleExists("Back Office"))
                Roles.CreateRole("Back Office");
            if (!Roles.RoleExists("Front Office"))
                Roles.CreateRole("Front Office");


            //if (!WebSecurity.UserExists("Terence-Hill"))
            //    WebSecurity.CreateUserAndAccount(
            //        "Terence-Hill",
            //        "password");
            //if (!Roles.GetRolesForUser("Terence-Hill").Contains("Amministratore"))
            //    Roles.AddUsersToRoles(new[] { "Terence-Hill" }, new[] { "Amministratore" });

            if (!WebSecurity.UserExists("Operatore"))
            {
                WebSecurity.CreateUserAndAccount( "Operatore", "password");
            }
            if (!Roles.GetRolesForUser("Operatore").Contains("Amministratore"))
                Roles.AddUsersToRoles(new[] { "Operatore" }, new[] { "Amministratore" });

           


            context.Toponimi.AddOrUpdate<Toponimo>(t => t.sigla,
                new Toponimo { sigla = "Via" },
                new Toponimo { sigla = "Piazza" });


           // if (tipoMigrazione.Equals(TipoMigrazione.INIZIALE))
            if (context.Province.Count() == 0)
            {
                codiciProvince(context);
                MigrazioneComuni.codiciComuni(context);

                context.Database.ExecuteSqlCommand("update comune set provincia_sigla=sigla from provincia where provincia.id=comune.codiceProvincia");
                foreach (Provincia p in context.Province.ToList())
                {
                    context.Database.ExecuteSqlCommand("update comune set  provincia_sigla=@p0 where codiceProvincia=@p1", p.sigla, p.id);
                }
            }


            context.statiSegnalazione.AddOrUpdate<Stato>(t => t.id,
            new Stato { id = 1, descrizione = "Segnalazione caricata", statoBase = EnumStatoBase.ATTIVO, entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE });
            context.Parametri.AddOrUpdate<Parametro>(p => p.id,
             new Parametro { id = 1, key = "stato.predefinito.segnalazione", value = "1", descrizione = "Stato predefinito al caricamento della segnalazione" });

            context.FontiPubblicitarie.AddOrUpdate<FontePubblicitaria>(t => t.id,
                new FontePubblicitaria { id = 1, descrizione = "PASSA PAROLA" },
                new FontePubblicitaria { id = 2, descrizione = "VOLANTINO" });

            context.TipoAzienda.AddOrUpdate<TipologiaAzienda>(t => t.id,
                new TipologiaAzienda { id = 1, descrizione = "PUBBLICO" },
                new TipologiaAzienda { id = 2, descrizione = "MUNICIPALIZZATA" },
                new TipologiaAzienda { id = 3, descrizione = "MINISTERIALE" },
                new TipologiaAzienda { id = 4, descrizione = "PRIVATA" }
                  );
            context.TipoContrattoImpiego.AddOrUpdate<TipoContrattoImpiego>(t => t.id,
            new TipoContrattoImpiego { id = 1, descrizione = "PUBBLICO" },
            new TipoContrattoImpiego { id = 2, descrizione = "PRIVATO" });

            context.TipoLuogoRitrovo.AddOrUpdate<TipoLuogoRitrovo>(t => t.id,
           new TipoLuogoRitrovo { id = 1, descrizione = "NON SPECIFICATO" },
           new TipoLuogoRitrovo { id = 2, descrizione = "PARABREZZA" });


            context.TipoContatto.AddOrUpdate<TipoContatto>(t => t.id,
           new TipoContatto { id = 1, descrizione = "EMAIL" },
           new TipoContatto { id = 2, descrizione = "PRESENZA IN SEDE" });

            context.TipoRiferimento.AddOrUpdate<TipoRiferimento>(t => t.id,
           new TipoRiferimento { id = 1, descrizione = "EMAIL" },
           new TipoRiferimento { id = 2, descrizione = "CELLULARE" });

            context.TipoCategoriaImpiego.AddOrUpdate<TipoCategoriaImpiego>(t => t.id,
            new TipoCategoriaImpiego { id = 1, descrizione = "PUBBLICO" },
            new TipoCategoriaImpiego { id = 2, descrizione = "STATALE" },
            new TipoCategoriaImpiego { id = 3, descrizione = "MUNICIPALIZZATO" });

            context.TipoCanaleAcquisizione.AddOrUpdate<TipoCanaleAcquisizione>(t => t.id,
            new TipoCanaleAcquisizione { id = 1, descrizione = "VOLANTINAGGIO" },
            new TipoCanaleAcquisizione { id = 2, descrizione = "PUBBLICITA CINEMA" });


            context.TipoPrestito.AddOrUpdate<TipologiaPrestito>(t => t.id,
                new TipologiaPrestito { id = 1, descrizione = "NESSUNO" },
                new TipologiaPrestito { id = 2, descrizione = "PRESENTE IN BUSTA PAGA" });

            context.TipoProdotto.AddOrUpdate<TipoProdotto>(t => t.id,
                new TipoProdotto { id = 1, descrizione = "CESSIONE" },
                new TipoProdotto { id = 2, descrizione = "DELEGA" });

            context.TipoIndirizzo.AddOrUpdate<TipologiaIndirizzo>(t => t.id,
                new TipologiaIndirizzo { id = 1, descrizione = "RESIDENZA" },
                new TipologiaIndirizzo { id = 2, descrizione = "DOMICILIO" },
                new TipologiaIndirizzo { id = 3, descrizione = "LAVORO" });
            context.TipoDocumentiIdentita.AddOrUpdate<TipoDocumentoIdentita>(t => t.id,
                new TipoDocumentoIdentita { id = 1, descrizione = "CARTA IDENTITA'" },
                new TipoDocumentoIdentita { id = 2, descrizione = "PASSAPORTO" },
                new TipoDocumentoIdentita { id = 3, descrizione = "PATENTE GUIDA" });

            context.TipoAgenzia.AddOrUpdate<TipoAgenzia>(t => t.id,
                new TipoAgenzia { id = 1, descrizione = "COLLABORATORE" },
                new TipoAgenzia { id = 2, descrizione = "AGENTE" },
                new TipoAgenzia { id = 3, descrizione = "DIPENDENTE" }
                );
            context.TipoErogazione.AddOrUpdate<TipoErogazione>(t => t.sigla,
              new TipoErogazione { sigla = "OBB", descrizione = "ORDINE BONIFICO BANCARIO" },
              new TipoErogazione { sigla = "ASS", descrizione = "ASSEGNO" }
              );




            context.TipoDocumenti.AddOrUpdate<TipoDocumento>(t => t.id,
                         new TipoDocumento { id = 1, descrizione = "Carta di identità" },
                         new TipoDocumento { id = 2, descrizione = "Passaporto" },
                          new TipoDocumento { id = 3, descrizione = "Benestare" },
                           new TipoDocumento { id = 4, descrizione = "Proposta" },
                            new TipoDocumento { id = 5, descrizione = "Contratto" },
                             new TipoDocumento { id = 6, descrizione = "Busta paga" }
                         );


            context.TipoCategoriaAmministrazione.AddOrUpdate<TipoCategoriaAmministrazione>(t => t.id,
                       new TipoCategoriaAmministrazione { id = 1, descrizione = "Pubblica" },
                       new TipoCategoriaAmministrazione { id = 2, descrizione = "Privata" },
                        new TipoCategoriaAmministrazione { id = 3, descrizione = "Municipalizzata" },
                         new TipoCategoriaAmministrazione { id = 4, descrizione = "Statale" }
                         );

            context.TipoAssumibilitaAmministrazione.AddOrUpdate<TipoAssumibilitaAmministrazione>(t => t.id,
                       new TipoAssumibilitaAmministrazione { id = 1, descrizione = "Assumibile" },
                       new TipoAssumibilitaAmministrazione { id = 2, descrizione = "Non assumibile" }
                         );


            //context.statiSegnalazione.AddOrUpdate<Stato>(t => t.id,
            //    //AMMINISTRAZIONI
            //    new Stato { id = 1, descrizione = "CENSITA", entitaAssociata = EnumEntitaAssociataStato.AMMINISTRAZIONE, statoBase = EnumStatoBase.ATTIVO },
            //    new Stato { id = 2, descrizione = "ATTIVA", entitaAssociata = EnumEntitaAssociataStato.AMMINISTRAZIONE, statoBase = EnumStatoBase.ATTIVO },
            //    new Stato { id = 3, descrizione = "DISATTIVA", entitaAssociata = EnumEntitaAssociataStato.AMMINISTRAZIONE, statoBase = EnumStatoBase.CHIUSO },
            //    //SEGNALAZIONI
            //    new Stato { id = 20, descrizione = "Assegnazione ad operatori di telemarketing", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
            //    new Stato { id = 21, descrizione = "Richiesta preventivo", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
            //    new Stato { id = 22, descrizione = "Attesa documentazione per analisi", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
            //    new Stato { id = 23, descrizione = "Analisi in sede", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
            //    new Stato { id = 24, descrizione = "Proposta in analisi", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
            //    new Stato { id = 25, descrizione = "Mancato appuntamento", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
            //    new Stato { id = 26, descrizione = "Proposta analizzata", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
            //    new Stato { id = 27, descrizione = "Attesa decisione cliente collaboratore", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
            //    new Stato { id = 28, descrizione = "Incontro in sede per esito positivo", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
            //    new Stato { id = 29, descrizione = "Raccolta in sede", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
            //    new Stato { id = 30, descrizione = "Raccolta a domicilio", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
            //    new Stato { id = 31, descrizione = "Raccolta fax – corrispondenza - collaboratore", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
            //    new Stato { id = 32, descrizione = "Attesa documenti per avvio istruttoria", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
            //    new Stato { id = 33, descrizione = "Inoltro documentazione sede centrale per avvio istruttoria", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
            //    new Stato { id = 34, descrizione = "Avvio istruttoria", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
            //    new Stato { id = 35, descrizione = "Annullamento", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ANNULLATO },
            //    new Stato { id = 36, descrizione = "Ripristino per cliente finanziabile", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
            //    new Stato { id = 37, descrizione = "Ripristino per cliente non finanziabile", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.ATTIVO },
            //    new Stato { id = 38, descrizione = "Chiusura", entitaAssociata = EnumEntitaAssociataStato.SEGNALAZIONE, statoBase = EnumStatoBase.CHIUSO}
            //    );



            context.tipoNaturaGiuridica.AddOrUpdate<TipoNaturaGiuridica>(t => t.id,
                new TipoNaturaGiuridica { id = 1, sigla = "AA", descrizione = "societa' in accomandita per azioni" },
                new TipoNaturaGiuridica { id = 2, sigla = "AC", descrizione = "associazione" },
                new TipoNaturaGiuridica { id = 3, sigla = "AE", descrizione = "societa' consortile in accomandita semplice" },
                new TipoNaturaGiuridica { id = 4, sigla = "AF", descrizione = "altre forme" },
                new TipoNaturaGiuridica { id = 5, sigla = "AI", descrizione = "associazione impresa" },
                new TipoNaturaGiuridica { id = 6, sigla = "AL", descrizione = "azienda speciale di ente locale" },
                new TipoNaturaGiuridica { id = 7, sigla = "AM", descrizione = "azienda municipale" },
                new TipoNaturaGiuridica { id = 8, sigla = "AN", descrizione = "societa' consortile in nome collettivo" },
                new TipoNaturaGiuridica { id = 9, sigla = "AP", descrizione = "azienda provinciale" },
                new TipoNaturaGiuridica { id = 10, sigla = "AR", descrizione = "azienda regionale" },
                new TipoNaturaGiuridica { id = 11, sigla = "AS", descrizione = "societa' in accomandita semplice" },
                new TipoNaturaGiuridica { id = 12, sigla = "AT", descrizione = "azienda autonoma statale" },
                new TipoNaturaGiuridica { id = 13, sigla = "AU", descrizione = "societa' per azioni con socio unico" },
                new TipoNaturaGiuridica { id = 14, sigla = "AZ", descrizione = "azienda speciale" },
                new TipoNaturaGiuridica { id = 15, sigla = "CC", descrizione = "consorzio con attivita' esterna" },
                new TipoNaturaGiuridica { id = 16, sigla = "CE", descrizione = "comunione ereditaria" },
                new TipoNaturaGiuridica { id = 17, sigla = "CI", descrizione = "societa' cooperativa a responsabilita illimitata" },
                new TipoNaturaGiuridica { id = 18, sigla = "CL", descrizione = "societa' cooperativa a responsabilita limitata" },
                new TipoNaturaGiuridica { id = 19, sigla = "CM", descrizione = "consorzio municipale" },
                new TipoNaturaGiuridica { id = 20, sigla = "CN", descrizione = "societa' consortile" },
                new TipoNaturaGiuridica { id = 21, sigla = "CO", descrizione = "consorzio" },
                new TipoNaturaGiuridica { id = 22, sigla = "CR", descrizione = "consorzio intercomunale" },
                new TipoNaturaGiuridica { id = 23, sigla = "CS", descrizione = "consorzio senza attivita' esterna" },
                new TipoNaturaGiuridica { id = 24, sigla = "CZ", descrizione = "consorzio di cui al dlgs 267/2000" },
                new TipoNaturaGiuridica { id = 25, sigla = "DI", descrizione = "impresa individuale" },
                new TipoNaturaGiuridica { id = 26, sigla = "EC", descrizione = "ente pubblico commerciale" },
                new TipoNaturaGiuridica { id = 27, sigla = "ED", descrizione = "ente diritto pubblico" },
                new TipoNaturaGiuridica { id = 28, sigla = "EE", descrizione = "ente ecclesiastico" },
                new TipoNaturaGiuridica { id = 29, sigla = "EI", descrizione = "ente impresa" },
                new TipoNaturaGiuridica { id = 30, sigla = "EM", descrizione = "ente morale" },
                new TipoNaturaGiuridica { id = 31, sigla = "EN", descrizione = "ente" },
                new TipoNaturaGiuridica { id = 32, sigla = "EP", descrizione = "ente pubblico economico" },
                new TipoNaturaGiuridica { id = 33, sigla = "ER", descrizione = "ente ecclesiastico civilmente riconosciuto" },
                new TipoNaturaGiuridica { id = 34, sigla = "ES", descrizione = "ente di cui alla l.r. 21-12-93, n.88" },
                new TipoNaturaGiuridica { id = 35, sigla = "FI", descrizione = "fondazione impresa" },
                new TipoNaturaGiuridica { id = 36, sigla = "FO", descrizione = "fondazione" },
                new TipoNaturaGiuridica { id = 37, sigla = "GE", descrizione = "gruppo europeo di interesse economico" },
                new TipoNaturaGiuridica { id = 38, sigla = "IC", descrizione = "istituto di credito" },
                new TipoNaturaGiuridica { id = 39, sigla = "ID", descrizione = "istituto di credito di diritto pubblico" },
                new TipoNaturaGiuridica { id = 40, sigla = "IF", descrizione = "impresa familiare" },
                new TipoNaturaGiuridica { id = 41, sigla = "IR", descrizione = "istituto religioso" },
                new TipoNaturaGiuridica { id = 42, sigla = "LL", descrizione = "azienda speciale di cui al dlgs 267/2000" },
                new TipoNaturaGiuridica { id = 43, sigla = "MA", descrizione = "mutua assicurazione" },
                new TipoNaturaGiuridica { id = 44, sigla = "OC", descrizione = "societa' cooperativa consortil" },
                new TipoNaturaGiuridica { id = 45, sigla = "OO", descrizione = "cooperativa sociale" },
                new TipoNaturaGiuridica { id = 46, sigla = "OS", descrizione = "societa' consortile cooperativa a responsabilita' limitata" },
                new TipoNaturaGiuridica { id = 47, sigla = "PA", descrizione = "associazione in partecipazione" },
                new TipoNaturaGiuridica { id = 48, sigla = "PC", descrizione = "piccola societa' cooperativa" },
                new TipoNaturaGiuridica { id = 49, sigla = "PS", descrizione = "piccola societa' cooperativa a responsabilita' limitata" },
                new TipoNaturaGiuridica { id = 50, sigla = "SA", descrizione = "societa' anonima" },
                new TipoNaturaGiuridica { id = 51, sigla = "SC", descrizione = "societa' cooperativa" },
                new TipoNaturaGiuridica { id = 52, sigla = "SE", descrizione = "societa' semplice" },
                new TipoNaturaGiuridica { id = 53, sigla = "SF", descrizione = "societa' di fatto" },
                new TipoNaturaGiuridica { id = 54, sigla = "SI", descrizione = "societa' irregolare" },
                new TipoNaturaGiuridica { id = 55, sigla = "SL", descrizione = "societa' consortile a responsabilita' limitata" },
                new TipoNaturaGiuridica { id = 56, sigla = "SM", descrizione = "societa' di mutuo soccorso" },
                new TipoNaturaGiuridica { id = 57, sigla = "SN", descrizione = "societa' in nome collettivo" },
                new TipoNaturaGiuridica { id = 58, sigla = "SO", descrizione = "societa' consortile per azioni" },
                new TipoNaturaGiuridica { id = 59, sigla = "SP", descrizione = "societa' per azioni" },
                new TipoNaturaGiuridica { id = 60, sigla = "SR", descrizione = "societa' a responsabilita' limitata" },
                new TipoNaturaGiuridica { id = 61, sigla = "SS", descrizione = "societa' costituita in base a leggi di altro stato" },
                new TipoNaturaGiuridica { id = 62, sigla = "ST", descrizione = "soggetto estero" },
                new TipoNaturaGiuridica { id = 63, sigla = "SU", descrizione = "societa' a responsabilita' limitata con unico socio" },
                new TipoNaturaGiuridica { id = 64, sigla = "SV", descrizione = "societa' tra avvocati" },
                new TipoNaturaGiuridica { id = 65, sigla = "SZ", descrizione = "societa' non prevista dalla legislazione italiana" });



            //    WebSecurity.InitializeDatabaseConnection("DefaultConnection", "webpages_Roles", "UserId", "UserName", autoCreateTables: true);
            //    SimpleRoleProvider srp = new SimpleRoleProvider();
            //    srp.CreateRole("FO");
            //    srp.CreateRole("AMM");
            //    srp.CreateRole("BO");

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }


        private void codiciProvince(MainDbContext context)
        {
            context.Province.AddOrUpdate<Provincia>(p => p.sigla,
               new Provincia { denominazione = "Torino", sigla = "TO", id = 1 },
new Provincia { denominazione = "Vercelli", sigla = "VC", id = 2 },
new Provincia { denominazione = "Novara", sigla = "NO", id = 3 },
new Provincia { denominazione = "Cuneo", sigla = "CN", id = 4 },
new Provincia { denominazione = "Asti", sigla = "AT", id = 5 },
new Provincia { denominazione = "Alessandria", sigla = "AL", id = 6 },
new Provincia { denominazione = "Biella", sigla = "BI", id = 96 },
new Provincia { denominazione = "Verbano-Cusio-Ossola", sigla = "VB", id = 103 },
new Provincia { denominazione = "Valle d'Aosta/Vallée d'Aoste", sigla = "AO", id = 7 },
new Provincia { denominazione = "Varese", sigla = "VA", id = 12 },
new Provincia { denominazione = "Como", sigla = "CO", id = 13 },
new Provincia { denominazione = "Sondrio", sigla = "SO", id = 14 },
new Provincia { denominazione = "Milano", sigla = "MI", id = 15 },
new Provincia { denominazione = "Bergamo", sigla = "BG", id = 16 },
new Provincia { denominazione = "Brescia", sigla = "BS", id = 17 },
new Provincia { denominazione = "Pavia", sigla = "PV", id = 18 },
new Provincia { denominazione = "Cremona", sigla = "CR", id = 19 },
new Provincia { denominazione = "Mantova", sigla = "MN", id = 20 },
new Provincia { denominazione = "Lecco", sigla = "LC", id = 97 },
new Provincia { denominazione = "Lodi", sigla = "LO", id = 98 },
new Provincia { denominazione = "Monza e della Brianza", sigla = "MB", id = 108 },
new Provincia { denominazione = "Bolzano/Bozen", sigla = "BZ", id = 21 },
new Provincia { denominazione = "Trento", sigla = "TN", id = 22 },
new Provincia { denominazione = "Verona", sigla = "VR", id = 23 },
new Provincia { denominazione = "Vicenza", sigla = "VI", id = 24 },
new Provincia { denominazione = "Belluno", sigla = "BL", id = 25 },
new Provincia { denominazione = "Treviso", sigla = "TV", id = 26 },
new Provincia { denominazione = "Venezia", sigla = "VE", id = 27 },
new Provincia { denominazione = "Padova", sigla = "PD", id = 28 },
new Provincia { denominazione = "Rovigo", sigla = "RO", id = 29 },
new Provincia { denominazione = "Udine", sigla = "UD", id = 30 },
new Provincia { denominazione = "Gorizia", sigla = "GO", id = 31 },
new Provincia { denominazione = "Trieste", sigla = "TS", id = 32 },
new Provincia { denominazione = "Pordenone", sigla = "PN", id = 93 },
new Provincia { denominazione = "Imperia", sigla = "IM", id = 8 },
new Provincia { denominazione = "Savona", sigla = "SV", id = 9 },
new Provincia { denominazione = "Genova", sigla = "GE", id = 10 },
new Provincia { denominazione = "La Spezia", sigla = "SP", id = 11 },
new Provincia { denominazione = "Piacenza", sigla = "PC", id = 33 },
new Provincia { denominazione = "Parma", sigla = "PR", id = 34 },
new Provincia { denominazione = "Reggio nell'Emilia", sigla = "RE", id = 35 },
new Provincia { denominazione = "Modena", sigla = "MO", id = 36 },
new Provincia { denominazione = "Bologna", sigla = "BO", id = 37 },
new Provincia { denominazione = "Ferrara", sigla = "FE", id = 38 },
new Provincia { denominazione = "Ravenna", sigla = "RA", id = 39 },
new Provincia { denominazione = "Forlì-Cesena", sigla = "FC", id = 40 },
new Provincia { denominazione = "Rimini", sigla = "RN", id = 99 },
new Provincia { denominazione = "Massa-Carrara", sigla = "MS", id = 45 },
new Provincia { denominazione = "Lucca", sigla = "LU", id = 46 },
new Provincia { denominazione = "Pistoia", sigla = "PT", id = 47 },
new Provincia { denominazione = "Firenze", sigla = "FI", id = 48 },
new Provincia { denominazione = "Livorno", sigla = "LI", id = 49 },
new Provincia { denominazione = "Pisa", sigla = "PI", id = 50 },
new Provincia { denominazione = "Arezzo", sigla = "AR", id = 51 },
new Provincia { denominazione = "Siena", sigla = "SI", id = 52 },
new Provincia { denominazione = "Grosseto", sigla = "GR", id = 53 },
new Provincia { denominazione = "Prato", sigla = "PO", id = 100 },
new Provincia { denominazione = "Perugia", sigla = "PG", id = 54 },
new Provincia { denominazione = "Terni", sigla = "TR", id = 55 },
new Provincia { denominazione = "Pesaro e Urbino", sigla = "PU", id = 41 },
new Provincia { denominazione = "Ancona", sigla = "AN", id = 42 },
new Provincia { denominazione = "Macerata", sigla = "MC", id = 43 },
new Provincia { denominazione = "Ascoli Piceno", sigla = "AP", id = 44 },
new Provincia { denominazione = "Fermo", sigla = "FM", id = 109 },
new Provincia { denominazione = "Viterbo", sigla = "VT", id = 56 },
new Provincia { denominazione = "Rieti", sigla = "RI", id = 57 },
new Provincia { denominazione = "Roma", sigla = "RM", id = 58 },
new Provincia { denominazione = "Latina", sigla = "LT", id = 59 },
new Provincia { denominazione = "Frosinone", sigla = "FR", id = 60 },
new Provincia { denominazione = "L'Aquila", sigla = "AQ", id = 66 },
new Provincia { denominazione = "Teramo", sigla = "TE", id = 67 },
new Provincia { denominazione = "Pescara", sigla = "PE", id = 68 },
new Provincia { denominazione = "Chieti", sigla = "CH", id = 69 },
new Provincia { denominazione = "Campobasso", sigla = "CB", id = 70 },
new Provincia { denominazione = "Isernia", sigla = "IS", id = 94 },
new Provincia { denominazione = "Caserta", sigla = "CE", id = 61 },
new Provincia { denominazione = "Benevento", sigla = "BN", id = 62 },
new Provincia { denominazione = "Napoli", sigla = "NA", id = 63 },
new Provincia { denominazione = "Avellino", sigla = "AV", id = 64 },
new Provincia { denominazione = "Salerno", sigla = "SA", id = 65 },
new Provincia { denominazione = "Foggia", sigla = "FG", id = 71 },
new Provincia { denominazione = "Bari", sigla = "BA", id = 72 },
new Provincia { denominazione = "Taranto", sigla = "TA", id = 73 },
new Provincia { denominazione = "Brindisi", sigla = "BR", id = 74 },
new Provincia { denominazione = "Lecce", sigla = "LE", id = 75 },
new Provincia { denominazione = "Barletta-Andria-Trani", sigla = "BT", id = 110 },
new Provincia { denominazione = "Potenza", sigla = "PZ", id = 76 },
new Provincia { denominazione = "Matera", sigla = "MT", id = 77 },
new Provincia { denominazione = "Cosenza", sigla = "CS", id = 78 },
new Provincia { denominazione = "Catanzaro", sigla = "CZ", id = 79 },
new Provincia { denominazione = "Reggio di Calabria", sigla = "RC", id = 80 },
new Provincia { denominazione = "Crotone", sigla = "KR", id = 101 },
new Provincia { denominazione = "Vibo Valentia", sigla = "VV", id = 102 },
new Provincia { denominazione = "Trapani", sigla = "TP", id = 81 },
new Provincia { denominazione = "Palermo", sigla = "PA", id = 82 },
new Provincia { denominazione = "Messina", sigla = "ME", id = 83 },
new Provincia { denominazione = "Agrigento", sigla = "AG", id = 84 },
new Provincia { denominazione = "Caltanissetta", sigla = "CL", id = 85 },
new Provincia { denominazione = "Enna", sigla = "EN", id = 86 },
new Provincia { denominazione = "Catania", sigla = "CT", id = 87 },
new Provincia { denominazione = "Ragusa", sigla = "RG", id = 88 },
new Provincia { denominazione = "Siracusa", sigla = "SR", id = 89 },
new Provincia { denominazione = "Sassari", sigla = "SS", id = 90 },
new Provincia { denominazione = "Nuoro", sigla = "NU", id = 91 },
new Provincia { denominazione = "Cagliari", sigla = "CA", id = 92 },
new Provincia { denominazione = "Oristano", sigla = "OR", id = 95 },
new Provincia { denominazione = "Olbia-Tempio", sigla = "OT", id = 104 },
new Provincia { denominazione = "Ogliastra", sigla = "OG", id = 105 },
new Provincia { denominazione = "Medio Campidano", sigla = "VS", id = 106 },
new Provincia { denominazione = "Carbonia-Iglesias", sigla = "CI", id = 107 }

);





        }
    }
}
