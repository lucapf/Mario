namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update07 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.agenzia", "partitaIva", c => c.String(nullable: false, maxLength: 11));
            AlterColumn("dbo.persona_fisica", "codiceFiscale", c => c.String(nullable: false, maxLength: 16));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.persona_fisica", "codiceFiscale", c => c.String(nullable: false));
            AlterColumn("dbo.agenzia", "partitaIva", c => c.String(nullable: false));
        }
    }
}
