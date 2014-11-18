namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update071 : DbMigration
    {
        public override void Up()
        {
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
        }
        
        public override void Down()
        {
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
        }
    }
}
