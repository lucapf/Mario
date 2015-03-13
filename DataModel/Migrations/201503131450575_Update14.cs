namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update14 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.coordinate_erogazione",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        coordinata = c.String(nullable: false),
                        cedenteId = c.Int(),
                        tipoCoordinataErogazione_sigla = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.persona_fisica", t => t.cedenteId)
                .ForeignKey("dbo.tipo_coordinata_erogazione", t => t.tipoCoordinataErogazione_sigla)
                .Index(t => t.cedenteId)
                .Index(t => t.tipoCoordinataErogazione_sigla);
            
            CreateTable(
                "dbo.tipo_coordinata_erogazione",
                c => new
                    {
                        sigla = c.String(nullable: false, maxLength: 128),
                        descrizione = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.sigla);
            
            CreateTable(
                "dbo.erogazione",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        tipoErogazioneId = c.String(nullable: false),
                        coordinataErogazioneId = c.Int(nullable: false),
                        praticaId = c.Int(nullable: false),
                        dataValuta = c.DateTime(),
                        dataPagamento = c.DateTime(),
                        importo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        nota_id = c.Int(),
                        tipoErogazione_sigla = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.coordinate_erogazione", t => t.coordinataErogazioneId)
                .ForeignKey("dbo.nota", t => t.nota_id)
                .ForeignKey("dbo.segnalazione", t => t.praticaId)
                .ForeignKey("dbo.tipo_erogazione", t => t.tipoErogazione_sigla)
                .Index(t => t.coordinataErogazioneId)
                .Index(t => t.praticaId)
                .Index(t => t.nota_id)
                .Index(t => t.tipoErogazione_sigla);
            
            AddColumn("dbo.segnalazione", "dataLiquidazione", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.erogazione", "tipoErogazione_sigla", "dbo.tipo_erogazione");
            DropForeignKey("dbo.erogazione", "praticaId", "dbo.segnalazione");
            DropForeignKey("dbo.erogazione", "nota_id", "dbo.nota");
            DropForeignKey("dbo.erogazione", "coordinataErogazioneId", "dbo.coordinate_erogazione");
            DropForeignKey("dbo.coordinate_erogazione", "tipoCoordinataErogazione_sigla", "dbo.tipo_coordinata_erogazione");
            DropForeignKey("dbo.coordinate_erogazione", "cedenteId", "dbo.persona_fisica");
            DropIndex("dbo.erogazione", new[] { "tipoErogazione_sigla" });
            DropIndex("dbo.erogazione", new[] { "nota_id" });
            DropIndex("dbo.erogazione", new[] { "praticaId" });
            DropIndex("dbo.erogazione", new[] { "coordinataErogazioneId" });
            DropIndex("dbo.coordinate_erogazione", new[] { "tipoCoordinataErogazione_sigla" });
            DropIndex("dbo.coordinate_erogazione", new[] { "cedenteId" });
            DropColumn("dbo.segnalazione", "dataLiquidazione");
            DropTable("dbo.erogazione");
            DropTable("dbo.tipo_coordinata_erogazione");
            DropTable("dbo.coordinate_erogazione");
        }
    }
}
