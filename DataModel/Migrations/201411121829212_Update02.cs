namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update02 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.contatto", newName: "persona_fisica");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.persona_fisica", newName: "contatto");
        }
    }
}
