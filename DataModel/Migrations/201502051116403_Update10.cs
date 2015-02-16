namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update10 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.segnalazione", new[] { "stato_id" });
            RenameColumn(table: "dbo.segnalazione", name: "stato_id", newName: "statoId");
            AlterColumn("dbo.segnalazione", "statoId", c => c.Int(nullable: false));
            CreateIndex("dbo.segnalazione", "statoId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.segnalazione", new[] { "statoId" });
            AlterColumn("dbo.segnalazione", "statoId", c => c.Int());
            RenameColumn(table: "dbo.segnalazione", name: "statoId", newName: "stato_id");
            CreateIndex("dbo.segnalazione", "stato_id");
        }
    }
}
