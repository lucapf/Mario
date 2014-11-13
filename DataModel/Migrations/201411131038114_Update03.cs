namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update03 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.segnalazione", "cedente_id", "dbo.contatto");
            DropForeignKey("dbo.segnalazione", "cedente_id", "dbo.cedente");
            DropForeignKey("dbo.segnalazione", "cedente_id", "dbo.persona_fisica");
            DropIndex("dbo.segnalazione", new[] { "contatto_id" });
            DropIndex("dbo.segnalazione", new[] { "cedente_id" });
            RenameColumn(table: "dbo.impiego", name: "Contatto_id", newName: "PersonaFisica_id");
            RenameColumn(table: "dbo.riferimento", name: "Contatto_id", newName: "PersonaFisica_id");
            RenameColumn(table: "dbo.documento_identita", name: "Cedente_id", newName: "PersonaFisica_id");
            RenameColumn(table: "dbo.indirizzo", name: "Cedente_id", newName: "PersonaFisica_id");
            RenameColumn(table: "dbo.persona_fisica", name: "Discriminator", newName: "tipoPersonaFisica");
            RenameIndex(table: "dbo.indirizzo", name: "IX_Cedente_id", newName: "IX_PersonaFisica_id");
            RenameIndex(table: "dbo.riferimento", name: "IX_Contatto_id", newName: "IX_PersonaFisica_id");
            RenameIndex(table: "dbo.documento_identita", name: "IX_Cedente_id", newName: "IX_PersonaFisica_id");
            RenameIndex(table: "dbo.impiego", name: "IX_Contatto_id", newName: "IX_PersonaFisica_id");
            AlterColumn("dbo.segnalazione", "contatto_id", c => c.Int());
            AlterColumn("dbo.persona_fisica", "tipoPersonaFisica", c => c.String(maxLength: 4000));
            CreateIndex("dbo.segnalazione", "contatto_id");
            DropColumn("dbo.segnalazione", "cedente_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.segnalazione", "cedente_id", c => c.Int());
            DropIndex("dbo.segnalazione", new[] { "contatto_id" });
            AlterColumn("dbo.persona_fisica", "tipoPersonaFisica", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.segnalazione", "contatto_id", c => c.Int(nullable: false));
            RenameIndex(table: "dbo.impiego", name: "IX_PersonaFisica_id", newName: "IX_Contatto_id");
            RenameIndex(table: "dbo.documento_identita", name: "IX_PersonaFisica_id", newName: "IX_Cedente_id");
            RenameIndex(table: "dbo.riferimento", name: "IX_PersonaFisica_id", newName: "IX_Contatto_id");
            RenameIndex(table: "dbo.indirizzo", name: "IX_PersonaFisica_id", newName: "IX_Cedente_id");
            RenameColumn(table: "dbo.persona_fisica", name: "tipoPersonaFisica", newName: "Discriminator");
            RenameColumn(table: "dbo.indirizzo", name: "PersonaFisica_id", newName: "Cedente_id");
            RenameColumn(table: "dbo.documento_identita", name: "PersonaFisica_id", newName: "Cedente_id");
            RenameColumn(table: "dbo.riferimento", name: "PersonaFisica_id", newName: "Contatto_id");
            RenameColumn(table: "dbo.impiego", name: "PersonaFisica_id", newName: "Contatto_id");
            CreateIndex("dbo.segnalazione", "cedente_id");
            CreateIndex("dbo.segnalazione", "contatto_id");
            AddForeignKey("dbo.segnalazione", "cedente_id", "dbo.persona_fisica", "id");
        }
    }
}
