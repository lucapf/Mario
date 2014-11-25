namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update11 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.impiego", new[] { "Contatto_id" });
            RenameColumn(table: "dbo.impiego", name: "Contatto_id", newName: "contattoId");
            AlterColumn("dbo.impiego", "contattoId", c => c.Int(nullable: false));
            CreateIndex("dbo.impiego", "contattoId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.impiego", new[] { "contattoId" });
            AlterColumn("dbo.impiego", "contattoId", c => c.Int());
            RenameColumn(table: "dbo.impiego", name: "contattoId", newName: "Contatto_id");
            CreateIndex("dbo.impiego", "Contatto_id");
        }
    }
}
