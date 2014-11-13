namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update01 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.indirizzo", "interno", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.indirizzo", "interno", c => c.String(nullable: false, maxLength: 10));
        }
    }
}
