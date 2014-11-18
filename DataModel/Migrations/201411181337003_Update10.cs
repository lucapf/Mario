namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.segnalazione", "preventivoConfermato_id", c => c.Int());
            CreateIndex("dbo.segnalazione", "preventivoConfermato_id");
            AddForeignKey("dbo.segnalazione", "preventivoConfermato_id", "dbo.preventivo", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.segnalazione", "preventivoConfermato_id", "dbo.preventivo");
            DropIndex("dbo.segnalazione", new[] { "preventivoConfermato_id" });
            DropColumn("dbo.segnalazione", "preventivoConfermato_id");
        }
    }
}
