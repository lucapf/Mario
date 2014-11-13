namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update04 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.impiego", name: "PersonaFisica_id", newName: "Contatto_id");
            RenameColumn(table: "dbo.riferimento", name: "PersonaFisica_id", newName: "Contatto_id");
            RenameColumn(table: "dbo.documento_identita", name: "PersonaFisica_id", newName: "Cedente_id");
            RenameColumn(table: "dbo.indirizzo", name: "PersonaFisica_id", newName: "Cedente_id");
            RenameIndex(table: "dbo.indirizzo", name: "IX_PersonaFisica_id", newName: "IX_Cedente_id");
            RenameIndex(table: "dbo.riferimento", name: "IX_PersonaFisica_id", newName: "IX_Contatto_id");
            RenameIndex(table: "dbo.impiego", name: "IX_PersonaFisica_id", newName: "IX_Contatto_id");
            RenameIndex(table: "dbo.documento_identita", name: "IX_PersonaFisica_id", newName: "IX_Cedente_id");
            AddColumn("dbo.segnalazione", "cedente_id", c => c.Int());
            CreateIndex("dbo.segnalazione", "cedente_id");
            AddForeignKey("dbo.segnalazione", "cedente_id", "dbo.persona_fisica", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.segnalazione", "cedente_id", "dbo.persona_fisica");
            DropIndex("dbo.segnalazione", new[] { "cedente_id" });
            DropColumn("dbo.segnalazione", "cedente_id");
            RenameIndex(table: "dbo.documento_identita", name: "IX_Cedente_id", newName: "IX_PersonaFisica_id");
            RenameIndex(table: "dbo.impiego", name: "IX_Contatto_id", newName: "IX_PersonaFisica_id");
            RenameIndex(table: "dbo.riferimento", name: "IX_Contatto_id", newName: "IX_PersonaFisica_id");
            RenameIndex(table: "dbo.indirizzo", name: "IX_Cedente_id", newName: "IX_PersonaFisica_id");
            RenameColumn(table: "dbo.indirizzo", name: "Cedente_id", newName: "PersonaFisica_id");
            RenameColumn(table: "dbo.documento_identita", name: "Cedente_id", newName: "PersonaFisica_id");
            RenameColumn(table: "dbo.riferimento", name: "Contatto_id", newName: "PersonaFisica_id");
            RenameColumn(table: "dbo.impiego", name: "Contatto_id", newName: "PersonaFisica_id");
        }
    }
}
