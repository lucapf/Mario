namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update03 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.segnalazione", "dataPromemoria", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.segnalazione", "dataPromemoria", c => c.DateTime(nullable: false));
        }
    }
}
