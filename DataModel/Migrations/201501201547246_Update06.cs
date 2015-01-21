namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update06 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.segnalazione", new[] { "contatto_id" });
            RenameColumn(table: "dbo.indirizzo", name: "SoggettoGiuridico_id", newName: "soggettoGiuridicoId");
            RenameColumn(table: "dbo.riferimento", name: "SoggettoGiuridico_id", newName: "soggettoGiuridicoId");
            RenameColumn(table: "dbo.amministrazione", name: "soggettoGiuridico_id", newName: "soggettoGiuridicoId");
            RenameColumn(table: "dbo.segnalazione", name: "contatto_id", newName: "contattoId");
            RenameColumn(table: "dbo.riferimento", name: "Contatto_id", newName: "contattoId");
            RenameColumn(table: "dbo.indirizzo", name: "Cedente_id", newName: "cedenteId");
            RenameIndex(table: "dbo.indirizzo", name: "IX_Cedente_id", newName: "IX_cedenteId");
            RenameIndex(table: "dbo.indirizzo", name: "IX_SoggettoGiuridico_id", newName: "IX_soggettoGiuridicoId");
            RenameIndex(table: "dbo.amministrazione", name: "IX_soggettoGiuridico_id", newName: "IX_soggettoGiuridicoId");
            RenameIndex(table: "dbo.riferimento", name: "IX_Contatto_id", newName: "IX_contattoId");
            RenameIndex(table: "dbo.riferimento", name: "IX_SoggettoGiuridico_id", newName: "IX_soggettoGiuridicoId");
            AddColumn("dbo.agenzia", "isEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.amministrazione", "isEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.impiego", "amministrazioneId", c => c.Int(nullable: false));
            AddColumn("dbo.impiego", "amministrazione_descrizione", c => c.String());
            AlterColumn("dbo.segnalazione", "contattoId", c => c.Int(nullable: false));
            CreateIndex("dbo.impiego", "amministrazioneId");
            CreateIndex("dbo.segnalazione", "contattoId");
            AddForeignKey("dbo.impiego", "amministrazioneId", "dbo.amministrazione", "id");
            DropColumn("dbo.agenzia", "disabled");
            DropColumn("dbo.amministrazione", "disabled");
            DropColumn("dbo.impiego", "azienda");
        }
        
        public override void Down()
        {
            AddColumn("dbo.impiego", "azienda", c => c.String(nullable: false));
            AddColumn("dbo.amministrazione", "disabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.agenzia", "disabled", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.impiego", "amministrazioneId", "dbo.amministrazione");
            DropIndex("dbo.segnalazione", new[] { "contattoId" });
            DropIndex("dbo.impiego", new[] { "amministrazioneId" });
            AlterColumn("dbo.segnalazione", "contattoId", c => c.Int());
            DropColumn("dbo.impiego", "amministrazione_descrizione");
            DropColumn("dbo.impiego", "amministrazioneId");
            DropColumn("dbo.amministrazione", "isEnabled");
            DropColumn("dbo.agenzia", "isEnabled");
            RenameIndex(table: "dbo.riferimento", name: "IX_soggettoGiuridicoId", newName: "IX_SoggettoGiuridico_id");
            RenameIndex(table: "dbo.riferimento", name: "IX_contattoId", newName: "IX_Contatto_id");
            RenameIndex(table: "dbo.amministrazione", name: "IX_soggettoGiuridicoId", newName: "IX_soggettoGiuridico_id");
            RenameIndex(table: "dbo.indirizzo", name: "IX_soggettoGiuridicoId", newName: "IX_SoggettoGiuridico_id");
            RenameIndex(table: "dbo.indirizzo", name: "IX_cedenteId", newName: "IX_Cedente_id");
            RenameColumn(table: "dbo.indirizzo", name: "cedenteId", newName: "Cedente_id");
            RenameColumn(table: "dbo.riferimento", name: "contattoId", newName: "Contatto_id");
            RenameColumn(table: "dbo.segnalazione", name: "contattoId", newName: "contatto_id");
            RenameColumn(table: "dbo.amministrazione", name: "soggettoGiuridicoId", newName: "soggettoGiuridico_id");
            RenameColumn(table: "dbo.riferimento", name: "soggettoGiuridicoId", newName: "SoggettoGiuridico_id");
            RenameColumn(table: "dbo.indirizzo", name: "soggettoGiuridicoId", newName: "SoggettoGiuridico_id");
            CreateIndex("dbo.segnalazione", "contatto_id");
        }
    }
}
