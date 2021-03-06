namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.agenzia",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        partitaIva = c.String(nullable: false),
                        rea = c.String(nullable: false),
                        dataInizioMandato = c.DateTime(),
                        dataFineMandato = c.DateTime(),
                        codiceRui = c.String(),
                        codiceOam = c.String(),
                        dataOam = c.DateTime(),
                        documentoPagamento = c.String(),
                        soggettoGiuridico_id = c.Int(nullable: false),
                        stato_id = c.Int(),
                        tipoAgenzia_id = c.Int(),
                        tipoNaturaGiuridica_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.soggetto_giuridico", t => t.soggettoGiuridico_id)
                .ForeignKey("dbo.stato", t => t.stato_id)
                .ForeignKey("dbo.tipo_agenzia", t => t.tipoAgenzia_id)
                .ForeignKey("dbo.tipo_natura_giuridica", t => t.tipoNaturaGiuridica_id)
                .Index(t => t.soggettoGiuridico_id)
                .Index(t => t.stato_id)
                .Index(t => t.tipoAgenzia_id)
                .Index(t => t.tipoNaturaGiuridica_id);
            
            CreateTable(
                "dbo.soggetto_giuridico",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        ragioneSociale = c.String(nullable: false),
                        tipoSoggettoGiuridico = c.String(nullable: false),
                        codiceFiscale = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.indirizzo",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        recapito = c.String(nullable: false, maxLength: 50),
                        cap = c.String(nullable: false),
                        presso = c.String(maxLength: 50),
                        numeroCivico = c.String(nullable: false),
                        interno = c.String(nullable: false, maxLength: 10),
                        corrispondenza = c.Boolean(nullable: false),
                        comune_id = c.Int(nullable: false),
                        provincia_sigla = c.String(nullable: false, maxLength: 128),
                        tipoIndirizzo_id = c.Int(nullable: false),
                        toponimo_sigla = c.String(nullable: false, maxLength: 128),
                        SoggettoGiuridico_id = c.Int(),
                        Cedente_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.comune", t => t.comune_id)
                .ForeignKey("dbo.provincia", t => t.provincia_sigla)
                .ForeignKey("dbo.tipo_indirizzo", t => t.tipoIndirizzo_id)
                .ForeignKey("dbo.toponimo", t => t.toponimo_sigla)
                .ForeignKey("dbo.soggetto_giuridico", t => t.SoggettoGiuridico_id)
                .ForeignKey("dbo.contatto", t => t.Cedente_id)
                .Index(t => t.comune_id)
                .Index(t => t.provincia_sigla)
                .Index(t => t.tipoIndirizzo_id)
                .Index(t => t.toponimo_sigla)
                .Index(t => t.SoggettoGiuridico_id)
                .Index(t => t.Cedente_id);
            
            CreateTable(
                "dbo.comune",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        codiceProvincia = c.Int(nullable: false),
                        denominazione = c.String(nullable: false),
                        provincia_sigla = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.provincia", t => t.provincia_sigla)
                .Index(t => t.provincia_sigla);
            
            CreateTable(
                "dbo.provincia",
                c => new
                    {
                        sigla = c.String(nullable: false, maxLength: 128),
                        id = c.Int(nullable: false),
                        denominazione = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.sigla);
            
            CreateTable(
                "dbo.tipo_indirizzo",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        descrizione = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.toponimo",
                c => new
                    {
                        sigla = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.sigla);
            
            CreateTable(
                "dbo.nota",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        dataInserimento = c.DateTime(nullable: false),
                        operatoreInserimento = c.String(nullable: false),
                        valore = c.String(),
                        SoggettoGiuridico_id = c.Int(),
                        Segnalazione_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.soggetto_giuridico", t => t.SoggettoGiuridico_id)
                .ForeignKey("dbo.segnalazione", t => t.Segnalazione_id)
                .Index(t => t.SoggettoGiuridico_id)
                .Index(t => t.Segnalazione_id);
            
            CreateTable(
                "dbo.riferimento",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        valore = c.String(nullable: false),
                        tipoRiferimento_id = c.Int(nullable: false),
                        SoggettoGiuridico_id = c.Int(),
                        Contatto_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.tipo_riferimento", t => t.tipoRiferimento_id)
                .ForeignKey("dbo.soggetto_giuridico", t => t.SoggettoGiuridico_id)
                .ForeignKey("dbo.contatto", t => t.Contatto_id)
                .Index(t => t.tipoRiferimento_id)
                .Index(t => t.SoggettoGiuridico_id)
                .Index(t => t.Contatto_id);
            
            CreateTable(
                "dbo.tipo_riferimento",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        descrizione = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.stato",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        descrizione = c.String(nullable: false),
                        statoBase = c.Int(nullable: false),
                        entitaAssociata = c.Int(nullable: false),
                        gruppoLavorazione_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.gruppo_lavorazione", t => t.gruppoLavorazione_id)
                .Index(t => t.gruppoLavorazione_id);
            
            CreateTable(
                "dbo.gruppo_lavorazione",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nome = c.String(nullable: false),
                        utenti = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tipo_agenzia",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        descrizione = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tipo_natura_giuridica",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        sigla = c.String(nullable: false),
                        descrizione = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.amministrazione",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        partitaIva = c.String(nullable: false),
                        capitaleSociale = c.Single(nullable: false),
                        assumibilita_id = c.Int(nullable: false),
                        pagante_id = c.Int(),
                        soggettoGiuridico_id = c.Int(nullable: false),
                        stato_id = c.Int(nullable: false),
                        tipoCategoria_id = c.Int(nullable: false),
                        tipoNaturaGiuridica_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.tipo_assumibilita_amministrazione", t => t.assumibilita_id)
                .ForeignKey("dbo.amministrazione", t => t.pagante_id)
                .ForeignKey("dbo.soggetto_giuridico", t => t.soggettoGiuridico_id)
                .ForeignKey("dbo.stato", t => t.stato_id)
                .ForeignKey("dbo.tipo_categoria_amministrazione", t => t.tipoCategoria_id)
                .ForeignKey("dbo.tipo_natura_giuridica", t => t.tipoNaturaGiuridica_id)
                .Index(t => t.assumibilita_id)
                .Index(t => t.pagante_id)
                .Index(t => t.soggettoGiuridico_id)
                .Index(t => t.stato_id)
                .Index(t => t.tipoCategoria_id)
                .Index(t => t.tipoNaturaGiuridica_id);
            
            CreateTable(
                "dbo.tipo_assumibilita_amministrazione",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        descrizione = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tipo_categoria_amministrazione",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        descrizione = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.assegnazione",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        segnalazioneId = c.Int(nullable: false),
                        statoId = c.Int(nullable: false),
                        dataInserimento = c.DateTime(nullable: false),
                        login = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.segnalazione", t => t.segnalazioneId)
                .ForeignKey("dbo.stato", t => t.statoId)
                .Index(t => t.segnalazioneId)
                .Index(t => t.statoId);
            
            CreateTable(
                "dbo.segnalazione",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        importoRichiesto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        durataRichiesta = c.Int(nullable: false),
                        rataRichiesta = c.Decimal(nullable: false, precision: 18, scale: 2),
                        dataInserimento = c.DateTime(nullable: false),
                        utenteInserimento = c.String(),
                        preventivoConfermatoId = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        altroPrestito_id = c.Int(nullable: false),
                        campagnaPubblicitaria_id = c.Int(),
                        canaleAcquisizione_id = c.Int(),
                        contatto_id = c.Int(nullable: false),
                        fontePubblicitaria_id = c.Int(nullable: false),
                        prodottoRichiesto_id = c.Int(),
                        stato_id = c.Int(),
                        tipoAzienda_id = c.Int(),
                        tipoContatto_id = c.Int(),
                        tipoLuogoRitrovo_id = c.Int(),
                        cedente_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.tipo_prestito", t => t.altroPrestito_id)
                .ForeignKey("dbo.tipo_campagna_pubblicitaria", t => t.campagnaPubblicitaria_id)
                .ForeignKey("dbo.tipo_canale_acquisizione", t => t.canaleAcquisizione_id)
                .ForeignKey("dbo.contatto", t => t.contatto_id)
                .ForeignKey("dbo.tipo_fonte_pubblicitaria", t => t.fontePubblicitaria_id)
                .ForeignKey("dbo.tipo_prodotto", t => t.prodottoRichiesto_id)
                .ForeignKey("dbo.stato", t => t.stato_id)
                .ForeignKey("dbo.tipo_azienda", t => t.tipoAzienda_id)
                .ForeignKey("dbo.tipo_contatto", t => t.tipoContatto_id)
                .ForeignKey("dbo.tipo_luogo_ritrovo", t => t.tipoLuogoRitrovo_id)
                .ForeignKey("dbo.contatto", t => t.cedente_id)
                .Index(t => t.altroPrestito_id)
                .Index(t => t.campagnaPubblicitaria_id)
                .Index(t => t.canaleAcquisizione_id)
                .Index(t => t.contatto_id)
                .Index(t => t.fontePubblicitaria_id)
                .Index(t => t.prodottoRichiesto_id)
                .Index(t => t.stato_id)
                .Index(t => t.tipoAzienda_id)
                .Index(t => t.tipoContatto_id)
                .Index(t => t.tipoLuogoRitrovo_id)
                .Index(t => t.cedente_id);
            
            CreateTable(
                "dbo.tipo_prestito",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        descrizione = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tipo_campagna_pubblicitaria",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        descrizione = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tipo_canale_acquisizione",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        descrizione = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.contatto",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nome = c.String(nullable: false),
                        cognome = c.String(nullable: false),
                        sesso = c.Int(nullable: false),
                        dataNascita = c.DateTime(nullable: false),
                        statoCivile = c.Int(nullable: false),
                        codiceFiscale = c.String(nullable: false),
                        nazioneNascita = c.String(),
                        cittadinanza = c.String(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        comuneNascita_id = c.Int(nullable: false),
                        provinciaNascita_sigla = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.comune", t => t.comuneNascita_id)
                .ForeignKey("dbo.provincia", t => t.provinciaNascita_sigla)
                .Index(t => t.comuneNascita_id)
                .Index(t => t.provinciaNascita_sigla);
            
            CreateTable(
                "dbo.impiego",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        azienda = c.String(nullable: false),
                        aziendaSedeLavoro = c.String(),
                        stipendioNettoMensile = c.Decimal(precision: 18, scale: 2),
                        stipendioLordoMensile = c.Decimal(precision: 18, scale: 2),
                        stipendioLordoAnnuo = c.Decimal(precision: 18, scale: 2),
                        mensilita = c.Int(),
                        mansione = c.String(),
                        anticipiTFR = c.Decimal(precision: 18, scale: 2),
                        adesioneTFR = c.DateTime(),
                        dataAssunzione = c.DateTime(),
                        dataLicenziamento = c.DateTime(),
                        categoriaImpiego_id = c.Int(),
                        tipoImpiego_id = c.Int(),
                        Contatto_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.tipo_categoria_impiego", t => t.categoriaImpiego_id)
                .ForeignKey("dbo.tipo_contratto_impiego", t => t.tipoImpiego_id)
                .ForeignKey("dbo.contatto", t => t.Contatto_id)
                .Index(t => t.categoriaImpiego_id)
                .Index(t => t.tipoImpiego_id)
                .Index(t => t.Contatto_id);
            
            CreateTable(
                "dbo.tipo_categoria_impiego",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        descrizione = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tipo_contratto_impiego",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        descrizione = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.documento_identita",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        dataRilascio = c.DateTime(nullable: false),
                        dataScadenza = c.DateTime(nullable: false),
                        numeroDocumento = c.String(nullable: false),
                        comuneEnte_id = c.Int(nullable: false),
                        enteRilascio_id = c.Int(nullable: false),
                        provinciaEnte_sigla = c.String(nullable: false, maxLength: 128),
                        Cedente_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.comune", t => t.comuneEnte_id)
                .ForeignKey("dbo.tipo_ente_rilascio", t => t.enteRilascio_id)
                .ForeignKey("dbo.provincia", t => t.provinciaEnte_sigla)
                .ForeignKey("dbo.contatto", t => t.Cedente_id)
                .Index(t => t.comuneEnte_id)
                .Index(t => t.enteRilascio_id)
                .Index(t => t.provinciaEnte_sigla)
                .Index(t => t.Cedente_id);
            
            CreateTable(
                "dbo.tipo_ente_rilascio",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        descrizione = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.documento",
                c => new
                    {
                        id = c.Guid(nullable: false),
                        nome = c.String(nullable: false),
                        descrizione = c.String(),
                        dataInserimento = c.DateTime(),
                        SegnalazioneId = c.Int(nullable: false),
                        tipoDocumento_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.segnalazione", t => t.SegnalazioneId)
                .ForeignKey("dbo.tipo_documento", t => t.tipoDocumento_id)
                .Index(t => t.SegnalazioneId)
                .Index(t => t.tipoDocumento_id);
            
            CreateTable(
                "dbo.tipo_documento",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        descrizione = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tipo_fonte_pubblicitaria",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        descrizione = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.preventivo",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        assicurazioneVitaId = c.Int(nullable: false),
                        assicurazioneImpiegoId = c.Int(nullable: false),
                        finanziariaId = c.Int(nullable: false),
                        progressivo = c.Int(nullable: false),
                        nomeProdotto = c.String(nullable: false),
                        importoRata = c.Decimal(precision: 18, scale: 2),
                        durata = c.Int(nullable: false),
                        tabellaFinanziaria = c.String(nullable: false),
                        importoCoperturaVita = c.Decimal(nullable: false, precision: 18, scale: 2),
                        importoCoperturaImpego = c.Decimal(nullable: false, precision: 18, scale: 2),
                        dataInserimento = c.DateTime(),
                        dataDecorrenza = c.DateTime(nullable: false),
                        importoProvvigioni = c.Decimal(nullable: false, precision: 18, scale: 2),
                        montante = c.Decimal(nullable: false, precision: 18, scale: 2),
                        importoInteressi = c.Decimal(nullable: false, precision: 18, scale: 2),
                        speseAttivazione = c.Decimal(nullable: false, precision: 18, scale: 2),
                        speseIncasso = c.Decimal(nullable: false, precision: 18, scale: 2),
                        oneriFiscali = c.Decimal(nullable: false, precision: 18, scale: 2),
                        importoImpegniDaEstinguere = c.Decimal(nullable: false, precision: 18, scale: 2),
                        nettoCliente = c.Decimal(nullable: false, precision: 18, scale: 2),
                        tan = c.Decimal(nullable: false, precision: 18, scale: 2),
                        taeg = c.Decimal(nullable: false, precision: 18, scale: 2),
                        teg = c.Decimal(nullable: false, precision: 18, scale: 2),
                        dataConferma = c.DateTime(),
                        segnalazione_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.soggetto_giuridico", t => t.assicurazioneImpiegoId)
                .ForeignKey("dbo.soggetto_giuridico", t => t.assicurazioneVitaId)
                .ForeignKey("dbo.soggetto_giuridico", t => t.finanziariaId)
                .ForeignKey("dbo.segnalazione", t => t.segnalazione_id)
                .Index(t => t.assicurazioneVitaId)
                .Index(t => t.assicurazioneImpiegoId)
                .Index(t => t.finanziariaId)
                .Index(t => t.segnalazione_id);
            
            CreateTable(
                "dbo.tipo_prodotto",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        descrizione = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tipo_azienda",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        descrizione = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tipo_contatto",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        descrizione = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tipo_luogo_ritrovo",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        descrizione = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.events",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        start_date = c.DateTime(nullable: false),
                        end_date = c.DateTime(nullable: false),
                        testo = c.String(),
                        utente = c.String(),
                        segnalazione_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.segnalazione", t => t.segnalazione_id)
                .Index(t => t.segnalazione_id);
            
            CreateTable(
                "dbo.log_evento",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        operatoreInserimento = c.String(),
                        dataInserimento = c.DateTime(nullable: false),
                        tipoEvento = c.Int(nullable: false),
                        messaggio = c.String(),
                        idEntita = c.Int(nullable: false),
                        tipoEntitaRiferimento = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.parametro",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        key = c.String(nullable: false),
                        descrizione = c.String(nullable: false),
                        value = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tipo_documento_identita",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        descrizione = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tipo_erogazione",
                c => new
                    {
                        sigla = c.String(nullable: false, maxLength: 128),
                        descrizione = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.sigla);
            
            CreateTable(
                "dbo.StatoStatoes",
                c => new
                    {
                        Stato_id = c.Int(nullable: false),
                        Stato_id1 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Stato_id, t.Stato_id1 })
                .ForeignKey("dbo.stato", t => t.Stato_id)
                .ForeignKey("dbo.stato", t => t.Stato_id1)
                .Index(t => t.Stato_id)
                .Index(t => t.Stato_id1);



           


        }
        
        public override void Down()
        {
            DropForeignKey("dbo.events", "segnalazione_id", "dbo.segnalazione");
            DropForeignKey("dbo.assegnazione", "statoId", "dbo.stato");
            DropForeignKey("dbo.assegnazione", "segnalazioneId", "dbo.segnalazione");
            DropForeignKey("dbo.segnalazione", "cedente_id", "dbo.contatto");
            DropForeignKey("dbo.segnalazione", "tipoLuogoRitrovo_id", "dbo.tipo_luogo_ritrovo");
            DropForeignKey("dbo.segnalazione", "tipoContatto_id", "dbo.tipo_contatto");
            DropForeignKey("dbo.segnalazione", "tipoAzienda_id", "dbo.tipo_azienda");
            DropForeignKey("dbo.segnalazione", "stato_id", "dbo.stato");
            DropForeignKey("dbo.segnalazione", "prodottoRichiesto_id", "dbo.tipo_prodotto");
            DropForeignKey("dbo.preventivo", "segnalazione_id", "dbo.segnalazione");
            DropForeignKey("dbo.preventivo", "finanziariaId", "dbo.soggetto_giuridico");
            DropForeignKey("dbo.preventivo", "assicurazioneVitaId", "dbo.soggetto_giuridico");
            DropForeignKey("dbo.preventivo", "assicurazioneImpiegoId", "dbo.soggetto_giuridico");
            DropForeignKey("dbo.nota", "Segnalazione_id", "dbo.segnalazione");
            DropForeignKey("dbo.segnalazione", "fontePubblicitaria_id", "dbo.tipo_fonte_pubblicitaria");
            DropForeignKey("dbo.documento", "tipoDocumento_id", "dbo.tipo_documento");
            DropForeignKey("dbo.documento", "SegnalazioneId", "dbo.segnalazione");
            DropForeignKey("dbo.segnalazione", "contatto_id", "dbo.contatto");
            DropForeignKey("dbo.indirizzo", "Cedente_id", "dbo.contatto");
            DropForeignKey("dbo.documento_identita", "Cedente_id", "dbo.contatto");
            DropForeignKey("dbo.documento_identita", "provinciaEnte_sigla", "dbo.provincia");
            DropForeignKey("dbo.documento_identita", "enteRilascio_id", "dbo.tipo_ente_rilascio");
            DropForeignKey("dbo.documento_identita", "comuneEnte_id", "dbo.comune");
            DropForeignKey("dbo.riferimento", "Contatto_id", "dbo.contatto");
            DropForeignKey("dbo.contatto", "provinciaNascita_sigla", "dbo.provincia");
            DropForeignKey("dbo.impiego", "Contatto_id", "dbo.contatto");
            DropForeignKey("dbo.impiego", "tipoImpiego_id", "dbo.tipo_contratto_impiego");
            DropForeignKey("dbo.impiego", "categoriaImpiego_id", "dbo.tipo_categoria_impiego");
            DropForeignKey("dbo.contatto", "comuneNascita_id", "dbo.comune");
            DropForeignKey("dbo.segnalazione", "canaleAcquisizione_id", "dbo.tipo_canale_acquisizione");
            DropForeignKey("dbo.segnalazione", "campagnaPubblicitaria_id", "dbo.tipo_campagna_pubblicitaria");
            DropForeignKey("dbo.segnalazione", "altroPrestito_id", "dbo.tipo_prestito");
            DropForeignKey("dbo.amministrazione", "tipoNaturaGiuridica_id", "dbo.tipo_natura_giuridica");
            DropForeignKey("dbo.amministrazione", "tipoCategoria_id", "dbo.tipo_categoria_amministrazione");
            DropForeignKey("dbo.amministrazione", "stato_id", "dbo.stato");
            DropForeignKey("dbo.amministrazione", "soggettoGiuridico_id", "dbo.soggetto_giuridico");
            DropForeignKey("dbo.amministrazione", "pagante_id", "dbo.amministrazione");
            DropForeignKey("dbo.amministrazione", "assumibilita_id", "dbo.tipo_assumibilita_amministrazione");
            DropForeignKey("dbo.agenzia", "tipoNaturaGiuridica_id", "dbo.tipo_natura_giuridica");
            DropForeignKey("dbo.agenzia", "tipoAgenzia_id", "dbo.tipo_agenzia");
            DropForeignKey("dbo.agenzia", "stato_id", "dbo.stato");
            DropForeignKey("dbo.StatoStatoes", "Stato_id1", "dbo.stato");
            DropForeignKey("dbo.StatoStatoes", "Stato_id", "dbo.stato");
            DropForeignKey("dbo.stato", "gruppoLavorazione_id", "dbo.gruppo_lavorazione");
            DropForeignKey("dbo.agenzia", "soggettoGiuridico_id", "dbo.soggetto_giuridico");
            DropForeignKey("dbo.riferimento", "SoggettoGiuridico_id", "dbo.soggetto_giuridico");
            DropForeignKey("dbo.riferimento", "tipoRiferimento_id", "dbo.tipo_riferimento");
            DropForeignKey("dbo.nota", "SoggettoGiuridico_id", "dbo.soggetto_giuridico");
            DropForeignKey("dbo.indirizzo", "SoggettoGiuridico_id", "dbo.soggetto_giuridico");
            DropForeignKey("dbo.indirizzo", "toponimo_sigla", "dbo.toponimo");
            DropForeignKey("dbo.indirizzo", "tipoIndirizzo_id", "dbo.tipo_indirizzo");
            DropForeignKey("dbo.indirizzo", "provincia_sigla", "dbo.provincia");
            DropForeignKey("dbo.indirizzo", "comune_id", "dbo.comune");
            DropForeignKey("dbo.comune", "provincia_sigla", "dbo.provincia");
            DropIndex("dbo.StatoStatoes", new[] { "Stato_id1" });
            DropIndex("dbo.StatoStatoes", new[] { "Stato_id" });
            DropIndex("dbo.events", new[] { "segnalazione_id" });
            DropIndex("dbo.preventivo", new[] { "segnalazione_id" });
            DropIndex("dbo.preventivo", new[] { "finanziariaId" });
            DropIndex("dbo.preventivo", new[] { "assicurazioneImpiegoId" });
            DropIndex("dbo.preventivo", new[] { "assicurazioneVitaId" });
            DropIndex("dbo.documento", new[] { "tipoDocumento_id" });
            DropIndex("dbo.documento", new[] { "SegnalazioneId" });
            DropIndex("dbo.documento_identita", new[] { "Cedente_id" });
            DropIndex("dbo.documento_identita", new[] { "provinciaEnte_sigla" });
            DropIndex("dbo.documento_identita", new[] { "enteRilascio_id" });
            DropIndex("dbo.documento_identita", new[] { "comuneEnte_id" });
            DropIndex("dbo.impiego", new[] { "Contatto_id" });
            DropIndex("dbo.impiego", new[] { "tipoImpiego_id" });
            DropIndex("dbo.impiego", new[] { "categoriaImpiego_id" });
            DropIndex("dbo.contatto", new[] { "provinciaNascita_sigla" });
            DropIndex("dbo.contatto", new[] { "comuneNascita_id" });
            DropIndex("dbo.segnalazione", new[] { "cedente_id" });
            DropIndex("dbo.segnalazione", new[] { "tipoLuogoRitrovo_id" });
            DropIndex("dbo.segnalazione", new[] { "tipoContatto_id" });
            DropIndex("dbo.segnalazione", new[] { "tipoAzienda_id" });
            DropIndex("dbo.segnalazione", new[] { "stato_id" });
            DropIndex("dbo.segnalazione", new[] { "prodottoRichiesto_id" });
            DropIndex("dbo.segnalazione", new[] { "fontePubblicitaria_id" });
            DropIndex("dbo.segnalazione", new[] { "contatto_id" });
            DropIndex("dbo.segnalazione", new[] { "canaleAcquisizione_id" });
            DropIndex("dbo.segnalazione", new[] { "campagnaPubblicitaria_id" });
            DropIndex("dbo.segnalazione", new[] { "altroPrestito_id" });
            DropIndex("dbo.assegnazione", new[] { "statoId" });
            DropIndex("dbo.assegnazione", new[] { "segnalazioneId" });
            DropIndex("dbo.amministrazione", new[] { "tipoNaturaGiuridica_id" });
            DropIndex("dbo.amministrazione", new[] { "tipoCategoria_id" });
            DropIndex("dbo.amministrazione", new[] { "stato_id" });
            DropIndex("dbo.amministrazione", new[] { "soggettoGiuridico_id" });
            DropIndex("dbo.amministrazione", new[] { "pagante_id" });
            DropIndex("dbo.amministrazione", new[] { "assumibilita_id" });
            DropIndex("dbo.stato", new[] { "gruppoLavorazione_id" });
            DropIndex("dbo.riferimento", new[] { "Contatto_id" });
            DropIndex("dbo.riferimento", new[] { "SoggettoGiuridico_id" });
            DropIndex("dbo.riferimento", new[] { "tipoRiferimento_id" });
            DropIndex("dbo.nota", new[] { "Segnalazione_id" });
            DropIndex("dbo.nota", new[] { "SoggettoGiuridico_id" });
            DropIndex("dbo.comune", new[] { "provincia_sigla" });
            DropIndex("dbo.indirizzo", new[] { "Cedente_id" });
            DropIndex("dbo.indirizzo", new[] { "SoggettoGiuridico_id" });
            DropIndex("dbo.indirizzo", new[] { "toponimo_sigla" });
            DropIndex("dbo.indirizzo", new[] { "tipoIndirizzo_id" });
            DropIndex("dbo.indirizzo", new[] { "provincia_sigla" });
            DropIndex("dbo.indirizzo", new[] { "comune_id" });
            DropIndex("dbo.agenzia", new[] { "tipoNaturaGiuridica_id" });
            DropIndex("dbo.agenzia", new[] { "tipoAgenzia_id" });
            DropIndex("dbo.agenzia", new[] { "stato_id" });
            DropIndex("dbo.agenzia", new[] { "soggettoGiuridico_id" });
            DropTable("dbo.StatoStatoes");
            DropTable("dbo.tipo_erogazione");
            DropTable("dbo.tipo_documento_identita");
            DropTable("dbo.parametro");
            DropTable("dbo.log_evento");
            DropTable("dbo.events");
            DropTable("dbo.tipo_luogo_ritrovo");
            DropTable("dbo.tipo_contatto");
            DropTable("dbo.tipo_azienda");
            DropTable("dbo.tipo_prodotto");
            DropTable("dbo.preventivo");
            DropTable("dbo.tipo_fonte_pubblicitaria");
            DropTable("dbo.tipo_documento");
            DropTable("dbo.documento");
            DropTable("dbo.tipo_ente_rilascio");
            DropTable("dbo.documento_identita");
            DropTable("dbo.tipo_contratto_impiego");
            DropTable("dbo.tipo_categoria_impiego");
            DropTable("dbo.impiego");
            DropTable("dbo.contatto");
            DropTable("dbo.tipo_canale_acquisizione");
            DropTable("dbo.tipo_campagna_pubblicitaria");
            DropTable("dbo.tipo_prestito");
            DropTable("dbo.segnalazione");
            DropTable("dbo.assegnazione");
            DropTable("dbo.tipo_categoria_amministrazione");
            DropTable("dbo.tipo_assumibilita_amministrazione");
            DropTable("dbo.amministrazione");
            DropTable("dbo.tipo_natura_giuridica");
            DropTable("dbo.tipo_agenzia");
            DropTable("dbo.gruppo_lavorazione");
            DropTable("dbo.stato");
            DropTable("dbo.tipo_riferimento");
            DropTable("dbo.riferimento");
            DropTable("dbo.nota");
            DropTable("dbo.toponimo");
            DropTable("dbo.tipo_indirizzo");
            DropTable("dbo.provincia");
            DropTable("dbo.comune");
            DropTable("dbo.indirizzo");
            DropTable("dbo.soggetto_giuridico");
            DropTable("dbo.agenzia");
        }
    }
}
