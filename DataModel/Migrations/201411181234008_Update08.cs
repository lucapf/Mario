namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update08 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.preventivo", new[] { "assicurazioneVitaId" });
            DropIndex("dbo.preventivo", new[] { "assicurazioneImpiegoId" });
            DropIndex("dbo.preventivo", new[] { "finanziariaId" });
            AlterColumn("dbo.preventivo", "assicurazioneVitaId", c => c.Int());
            AlterColumn("dbo.preventivo", "assicurazioneImpiegoId", c => c.Int());
            AlterColumn("dbo.preventivo", "finanziariaId", c => c.Int());
            CreateIndex("dbo.preventivo", "assicurazioneVitaId");
            CreateIndex("dbo.preventivo", "assicurazioneImpiegoId");
            CreateIndex("dbo.preventivo", "finanziariaId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.preventivo", new[] { "finanziariaId" });
            DropIndex("dbo.preventivo", new[] { "assicurazioneImpiegoId" });
            DropIndex("dbo.preventivo", new[] { "assicurazioneVitaId" });
            AlterColumn("dbo.preventivo", "finanziariaId", c => c.Int(nullable: false));
            AlterColumn("dbo.preventivo", "assicurazioneImpiegoId", c => c.Int(nullable: false));
            AlterColumn("dbo.preventivo", "assicurazioneVitaId", c => c.Int(nullable: false));
            CreateIndex("dbo.preventivo", "finanziariaId");
            CreateIndex("dbo.preventivo", "assicurazioneImpiegoId");
            CreateIndex("dbo.preventivo", "assicurazioneVitaId");
        }
    }
}
