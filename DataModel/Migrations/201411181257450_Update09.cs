namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update09 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.preventivo", "Tipo", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.preventivo", "Tipo");
        }
    }
}
