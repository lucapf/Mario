namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update07 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.segnalazione", new[] { "altroPrestito_id" });
            DropIndex("dbo.segnalazione", new[] { "fontePubblicitaria_id" });
            AlterColumn("dbo.segnalazione", "altroPrestito_id", c => c.Int());
            AlterColumn("dbo.segnalazione", "fontePubblicitaria_id", c => c.Int());
            CreateIndex("dbo.segnalazione", "altroPrestito_id");
            CreateIndex("dbo.segnalazione", "fontePubblicitaria_id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.segnalazione", new[] { "fontePubblicitaria_id" });
            DropIndex("dbo.segnalazione", new[] { "altroPrestito_id" });
            AlterColumn("dbo.segnalazione", "fontePubblicitaria_id", c => c.Int(nullable: false));
            AlterColumn("dbo.segnalazione", "altroPrestito_id", c => c.Int(nullable: false));
            CreateIndex("dbo.segnalazione", "fontePubblicitaria_id");
            CreateIndex("dbo.segnalazione", "altroPrestito_id");
        }
    }
}
