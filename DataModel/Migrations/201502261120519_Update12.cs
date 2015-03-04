namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update12 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.consenso_privacy",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        acconsento = c.Boolean(nullable: false),
                        nonAcconsento = c.Boolean(nullable: false),
                        dataInserimento = c.DateTime(nullable: false),
                        untenteInserimento = c.String(nullable: false),
                        tipoConsensoPrivacy_id = c.Int(nullable: false),
                        Segnalazione_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.tipo_consenso_privacy", t => t.tipoConsensoPrivacy_id)
                .ForeignKey("dbo.segnalazione", t => t.Segnalazione_id)
                .Index(t => t.tipoConsensoPrivacy_id)
                .Index(t => t.Segnalazione_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.consenso_privacy", "Segnalazione_id", "dbo.segnalazione");
            DropForeignKey("dbo.consenso_privacy", "tipoConsensoPrivacy_id", "dbo.tipo_consenso_privacy");
            DropIndex("dbo.consenso_privacy", new[] { "Segnalazione_id" });
            DropIndex("dbo.consenso_privacy", new[] { "tipoConsensoPrivacy_id" });
            DropTable("dbo.consenso_privacy");
        }
    }
}
