namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update12 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.preventivo", "Tipo", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.preventivo", "Tipo", c => c.String(maxLength: 128));
        }
    }
}
