namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update11 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tipo_consenso_privacy",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        descrizione = c.String(nullable: false),
                        attivo = c.Boolean(nullable: false),
                        eliminabile = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tipo_consenso_privacy");
        }
    }
}
