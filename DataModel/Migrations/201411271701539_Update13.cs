namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update13 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StatoStatoes", "Stato_id", "dbo.stato");
            DropForeignKey("dbo.StatoStatoes", "Stato_id1", "dbo.stato");
            DropIndex("dbo.StatoStatoes", new[] { "Stato_id" });
            DropIndex("dbo.StatoStatoes", new[] { "Stato_id1" });
            DropTable("dbo.StatoStatoes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.StatoStatoes",
                c => new
                    {
                        Stato_id = c.Int(nullable: false),
                        Stato_id1 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Stato_id, t.Stato_id1 });
            
            CreateIndex("dbo.StatoStatoes", "Stato_id1");
            CreateIndex("dbo.StatoStatoes", "Stato_id");
            AddForeignKey("dbo.StatoStatoes", "Stato_id1", "dbo.stato", "id");
            AddForeignKey("dbo.StatoStatoes", "Stato_id", "dbo.stato", "id");
        }
    }
}
