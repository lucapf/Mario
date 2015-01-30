namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update08 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.istituto",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nome = c.String(nullable: false),
                        applicativo = c.String(nullable: false),
                        url = c.String(nullable: false),
                        dataInserimento = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
        }
        
        public override void Down()
        {
            DropTable("dbo.istituto");
        }
    }
}
