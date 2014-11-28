namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update14 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.stato", "gruppoLavorazione_id", "dbo.gruppo_lavorazione");
            DropIndex("dbo.stato", new[] { "gruppoLavorazione_id" });
            AddColumn("dbo.stato", "gruppoId", c => c.Int(nullable: false));
            DropColumn("dbo.stato", "gruppoLavorazione_id");
            DropTable("dbo.gruppo_lavorazione");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.gruppo_lavorazione",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nome = c.String(nullable: false),
                        utenti = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            AddColumn("dbo.stato", "gruppoLavorazione_id", c => c.Int());
            DropColumn("dbo.stato", "gruppoId");
            CreateIndex("dbo.stato", "gruppoLavorazione_id");
            AddForeignKey("dbo.stato", "gruppoLavorazione_id", "dbo.gruppo_lavorazione", "id");
        }
    }
}
