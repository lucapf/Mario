namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update13 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.impiego", new[] { "categoriaImpiego_id" });
            DropIndex("dbo.impiego", new[] { "tipoImpiego_id" });
            DropIndex("dbo.consenso_privacy", new[] { "Segnalazione_id" });
            RenameColumn(table: "dbo.impiego", name: "categoriaImpiego_id", newName: "categoriaImpiegoId");
            RenameColumn(table: "dbo.impiego", name: "tipoImpiego_id", newName: "tipoImpiegoId");
            RenameColumn(table: "dbo.consenso_privacy", name: "Segnalazione_id", newName: "segnalazioneId");
            AddColumn("dbo.tipo_consenso_privacy", "isSystem", c => c.Boolean(nullable: false));
            AlterColumn("dbo.impiego", "categoriaImpiegoId", c => c.Int(nullable: false));
            AlterColumn("dbo.impiego", "tipoImpiegoId", c => c.Int(nullable: false));
            AlterColumn("dbo.consenso_privacy", "segnalazioneId", c => c.Int(nullable: false));
            CreateIndex("dbo.impiego", "tipoImpiegoId");
            CreateIndex("dbo.impiego", "categoriaImpiegoId");
            CreateIndex("dbo.consenso_privacy", "segnalazioneId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.consenso_privacy", new[] { "segnalazioneId" });
            DropIndex("dbo.impiego", new[] { "categoriaImpiegoId" });
            DropIndex("dbo.impiego", new[] { "tipoImpiegoId" });
            AlterColumn("dbo.consenso_privacy", "segnalazioneId", c => c.Int());
            AlterColumn("dbo.impiego", "tipoImpiegoId", c => c.Int());
            AlterColumn("dbo.impiego", "categoriaImpiegoId", c => c.Int());
            DropColumn("dbo.tipo_consenso_privacy", "isSystem");
            RenameColumn(table: "dbo.consenso_privacy", name: "segnalazioneId", newName: "Segnalazione_id");
            RenameColumn(table: "dbo.impiego", name: "tipoImpiegoId", newName: "tipoImpiego_id");
            RenameColumn(table: "dbo.impiego", name: "categoriaImpiegoId", newName: "categoriaImpiego_id");
            CreateIndex("dbo.consenso_privacy", "Segnalazione_id");
            CreateIndex("dbo.impiego", "tipoImpiego_id");
            CreateIndex("dbo.impiego", "categoriaImpiego_id");
        }
    }
}
