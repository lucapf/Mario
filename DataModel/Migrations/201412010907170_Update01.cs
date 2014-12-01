namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update01 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.contatto", newName: "persona_fisica");
            DropForeignKey("dbo.stato", "gruppoLavorazione_id", "dbo.gruppo_lavorazione");
            DropForeignKey("dbo.StatoStatoes", "Stato_id", "dbo.stato");
            DropForeignKey("dbo.StatoStatoes", "Stato_id1", "dbo.stato");
            DropIndex("dbo.stato", new[] { "gruppoLavorazione_id" });
            DropIndex("dbo.segnalazione", new[] { "altroPrestito_id" });
            DropIndex("dbo.segnalazione", new[] { "contatto_id" });
            DropIndex("dbo.segnalazione", new[] { "fontePubblicitaria_id" });
            DropIndex("dbo.impiego", new[] { "Contatto_id" });
            DropIndex("dbo.preventivo", new[] { "assicurazioneVitaId" });
            DropIndex("dbo.preventivo", new[] { "assicurazioneImpiegoId" });
            DropIndex("dbo.preventivo", new[] { "finanziariaId" });
            DropIndex("dbo.StatoStatoes", new[] { "Stato_id" });
            DropIndex("dbo.StatoStatoes", new[] { "Stato_id1" });
            RenameColumn(table: "dbo.impiego", name: "Contatto_id", newName: "contattoId");
            RenameColumn(table: "dbo.persona_fisica", name: "Discriminator", newName: "tipoPersonaFisica");
            AddColumn("dbo.stato", "gruppoId", c => c.Int());
            AddColumn("dbo.segnalazione", "preventivoConfermato_id", c => c.Int());
            AddColumn("dbo.preventivo", "Tipo", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.indirizzo", "interno", c => c.String(maxLength: 10));
            AlterColumn("dbo.segnalazione", "altroPrestito_id", c => c.Int());
            AlterColumn("dbo.segnalazione", "contatto_id", c => c.Int());
            AlterColumn("dbo.segnalazione", "fontePubblicitaria_id", c => c.Int());
            AlterColumn("dbo.impiego", "contattoId", c => c.Int(nullable: false));
            AlterColumn("dbo.preventivo", "assicurazioneVitaId", c => c.Int());
            AlterColumn("dbo.preventivo", "assicurazioneImpiegoId", c => c.Int());
            AlterColumn("dbo.preventivo", "finanziariaId", c => c.Int());
            AlterColumn("dbo.preventivo", "nomeProdotto", c => c.String());
            AlterColumn("dbo.preventivo", "tabellaFinanziaria", c => c.String());
            AlterColumn("dbo.preventivo", "importoCoperturaVita", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.preventivo", "importoCoperturaImpego", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.preventivo", "dataDecorrenza", c => c.DateTime());
            AlterColumn("dbo.preventivo", "importoProvvigioni", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.preventivo", "importoInteressi", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.preventivo", "speseAttivazione", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.preventivo", "speseIncasso", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.preventivo", "oneriFiscali", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.preventivo", "importoImpegniDaEstinguere", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.preventivo", "teg", c => c.Decimal(precision: 18, scale: 2));
            CreateIndex("dbo.segnalazione", "altroPrestito_id");
            CreateIndex("dbo.segnalazione", "contatto_id");
            CreateIndex("dbo.segnalazione", "fontePubblicitaria_id");
            CreateIndex("dbo.segnalazione", "preventivoConfermato_id");
            CreateIndex("dbo.impiego", "contattoId");
            CreateIndex("dbo.preventivo", "assicurazioneVitaId");
            CreateIndex("dbo.preventivo", "assicurazioneImpiegoId");
            CreateIndex("dbo.preventivo", "finanziariaId");
            AddForeignKey("dbo.segnalazione", "preventivoConfermato_id", "dbo.preventivo", "id");
            DropColumn("dbo.stato", "gruppoLavorazione_id");
            DropTable("dbo.gruppo_lavorazione");
            DropTable("dbo.StatoStatoes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.StatoStatoes",
                c => new
                    {
                        Stato_id = c.Int(nullable: false),
                        Stato_id1 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Stato_id, t.Stato_id1 });
            
            CreateTable(
                "dbo.gruppo_lavorazione",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nome = c.String(nullable: false),
                        utenti = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            AddColumn("dbo.stato", "gruppoLavorazione_id", c => c.Int());
            DropForeignKey("dbo.segnalazione", "preventivoConfermato_id", "dbo.preventivo");
            DropIndex("dbo.preventivo", new[] { "finanziariaId" });
            DropIndex("dbo.preventivo", new[] { "assicurazioneImpiegoId" });
            DropIndex("dbo.preventivo", new[] { "assicurazioneVitaId" });
            DropIndex("dbo.impiego", new[] { "contattoId" });
            DropIndex("dbo.segnalazione", new[] { "preventivoConfermato_id" });
            DropIndex("dbo.segnalazione", new[] { "fontePubblicitaria_id" });
            DropIndex("dbo.segnalazione", new[] { "contatto_id" });
            DropIndex("dbo.segnalazione", new[] { "altroPrestito_id" });
            AlterColumn("dbo.preventivo", "teg", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.preventivo", "importoImpegniDaEstinguere", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.preventivo", "oneriFiscali", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.preventivo", "speseIncasso", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.preventivo", "speseAttivazione", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.preventivo", "importoInteressi", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.preventivo", "importoProvvigioni", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.preventivo", "dataDecorrenza", c => c.DateTime(nullable: false));
            AlterColumn("dbo.preventivo", "importoCoperturaImpego", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.preventivo", "importoCoperturaVita", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.preventivo", "tabellaFinanziaria", c => c.String(nullable: false));
            AlterColumn("dbo.preventivo", "nomeProdotto", c => c.String(nullable: false));
            AlterColumn("dbo.preventivo", "finanziariaId", c => c.Int(nullable: false));
            AlterColumn("dbo.preventivo", "assicurazioneImpiegoId", c => c.Int(nullable: false));
            AlterColumn("dbo.preventivo", "assicurazioneVitaId", c => c.Int(nullable: false));
            AlterColumn("dbo.impiego", "contattoId", c => c.Int());
            AlterColumn("dbo.segnalazione", "fontePubblicitaria_id", c => c.Int(nullable: false));
            AlterColumn("dbo.segnalazione", "contatto_id", c => c.Int(nullable: false));
            AlterColumn("dbo.segnalazione", "altroPrestito_id", c => c.Int(nullable: false));
            AlterColumn("dbo.indirizzo", "interno", c => c.String(nullable: false, maxLength: 10));
            DropColumn("dbo.preventivo", "Tipo");
            DropColumn("dbo.segnalazione", "preventivoConfermato_id");
            DropColumn("dbo.stato", "gruppoId");
            RenameColumn(table: "dbo.persona_fisica", name: "tipoPersonaFisica", newName: "Discriminator");
            RenameColumn(table: "dbo.impiego", name: "contattoId", newName: "Contatto_id");
            CreateIndex("dbo.StatoStatoes", "Stato_id1");
            CreateIndex("dbo.StatoStatoes", "Stato_id");
            CreateIndex("dbo.preventivo", "finanziariaId");
            CreateIndex("dbo.preventivo", "assicurazioneImpiegoId");
            CreateIndex("dbo.preventivo", "assicurazioneVitaId");
            CreateIndex("dbo.impiego", "Contatto_id");
            CreateIndex("dbo.segnalazione", "fontePubblicitaria_id");
            CreateIndex("dbo.segnalazione", "contatto_id");
            CreateIndex("dbo.segnalazione", "altroPrestito_id");
            CreateIndex("dbo.stato", "gruppoLavorazione_id");
            AddForeignKey("dbo.StatoStatoes", "Stato_id1", "dbo.stato", "id");
            AddForeignKey("dbo.StatoStatoes", "Stato_id", "dbo.stato", "id");
            AddForeignKey("dbo.stato", "gruppoLavorazione_id", "dbo.gruppo_lavorazione", "id");
            RenameTable(name: "dbo.persona_fisica", newName: "contatto");
        }
    }
}
