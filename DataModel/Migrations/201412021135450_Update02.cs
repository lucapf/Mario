namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update02 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.segnalazione", "dataPromemoria", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.segnalazione", "dataPromemoria");
        }
    }
}
