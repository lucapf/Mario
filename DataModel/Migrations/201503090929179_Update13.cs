namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update13 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.consenso_privacy", new[] { "Segnalazione_id" });
            RenameColumn(table: "dbo.consenso_privacy", name: "Segnalazione_id", newName: "segnalazioneId");
            AlterColumn("dbo.consenso_privacy", "segnalazioneId", c => c.Int(nullable: false));
            CreateIndex("dbo.consenso_privacy", "segnalazioneId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.consenso_privacy", new[] { "segnalazioneId" });
            AlterColumn("dbo.consenso_privacy", "segnalazioneId", c => c.Int());
            RenameColumn(table: "dbo.consenso_privacy", name: "segnalazioneId", newName: "Segnalazione_id");
            CreateIndex("dbo.consenso_privacy", "Segnalazione_id");
        }
    }
}
