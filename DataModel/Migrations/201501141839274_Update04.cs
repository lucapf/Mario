namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update04 : DbMigration
    {
        public override void Up()
        {
            string directory = AppDomain.CurrentDomain.BaseDirectory;

            System.IO.FileInfo update = new System.IO.FileInfo(directory + "..\\SQL\\Update04.sql");

            if (update.Exists)
            {
                this.Sql(System.IO.File.ReadAllText(update.FullName));
            }

            DropForeignKey("dbo.agenzia", "stato_id", "dbo.stato");
            DropForeignKey("dbo.amministrazione", "stato_id", "dbo.stato");
            DropIndex("dbo.agenzia", new[] { "stato_id" });
            DropIndex("dbo.amministrazione", new[] { "stato_id" });
            DropIndex("dbo.preventivo", new[] { "segnalazione_id" });
            RenameColumn(table: "dbo.preventivo", name: "segnalazione_id", newName: "segnalazioneId");
            AddColumn("dbo.agenzia", "disabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.amministrazione", "disabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.preventivo", "bySimulatore", c => c.Boolean(nullable: false));
            AddColumn("dbo.preventivo", "importoCoperturaImpiego", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.amministrazione", "capitaleSociale", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.impiego", "dataAssunzione", c => c.DateTime(nullable: false));
            AlterColumn("dbo.preventivo", "segnalazioneId", c => c.Int(nullable: false));
            CreateIndex("dbo.preventivo", "segnalazioneId");
            DropColumn("dbo.agenzia", "stato_id");
            DropColumn("dbo.amministrazione", "stato_id");
            DropColumn("dbo.preventivo", "importoCoperturaImpego");
        }
        
        public override void Down()
        {
            AddColumn("dbo.preventivo", "importoCoperturaImpego", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.amministrazione", "stato_id", c => c.Int(nullable: false));
            AddColumn("dbo.agenzia", "stato_id", c => c.Int());
            DropIndex("dbo.preventivo", new[] { "segnalazioneId" });
            AlterColumn("dbo.preventivo", "segnalazioneId", c => c.Int());
            AlterColumn("dbo.impiego", "dataAssunzione", c => c.DateTime());
            AlterColumn("dbo.amministrazione", "capitaleSociale", c => c.Single(nullable: false));
            DropColumn("dbo.preventivo", "importoCoperturaImpiego");
            DropColumn("dbo.preventivo", "bySimulatore");
            DropColumn("dbo.amministrazione", "disabled");
            DropColumn("dbo.agenzia", "disabled");
            RenameColumn(table: "dbo.preventivo", name: "segnalazioneId", newName: "segnalazione_id");
            CreateIndex("dbo.preventivo", "segnalazione_id");
            CreateIndex("dbo.amministrazione", "stato_id");
            CreateIndex("dbo.agenzia", "stato_id");
            AddForeignKey("dbo.amministrazione", "stato_id", "dbo.stato", "id");
            AddForeignKey("dbo.agenzia", "stato_id", "dbo.stato", "id");
        }
    }
}
