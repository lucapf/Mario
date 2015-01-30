namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update09 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.segnalazione", name: "cedente_id", newName: "cedenteId");
            RenameIndex(table: "dbo.segnalazione", name: "IX_cedente_id", newName: "IX_cedenteId");
            AddColumn("dbo.segnalazione", "dataRinnovo", c => c.DateTime());
            AddColumn("dbo.segnalazione", "dataDecorrenza", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.segnalazione", "dataDecorrenza");
            DropColumn("dbo.segnalazione", "dataRinnovo");
            RenameIndex(table: "dbo.segnalazione", name: "IX_cedenteId", newName: "IX_cedente_id");
            RenameColumn(table: "dbo.segnalazione", name: "cedenteId", newName: "cedente_id");
        }
    }
}
