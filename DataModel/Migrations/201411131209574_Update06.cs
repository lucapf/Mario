namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update06 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.persona_fisica", "tipoPersonaFisica", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.persona_fisica", "tipoPersonaFisica", c => c.String(maxLength: 128));
        }
    }
}