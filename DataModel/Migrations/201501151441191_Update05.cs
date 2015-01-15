namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update05 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.preventivo", "costoFinanziamento", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.preventivo", "commissioniFinanziarie", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.preventivo", "commissioniIntermediazione", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.preventivo", "importoCommissioni", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.preventivo", "importoCommissioni");
            DropColumn("dbo.preventivo", "commissioniIntermediazione");
            DropColumn("dbo.preventivo", "commissioniFinanziarie");
            DropColumn("dbo.preventivo", "costoFinanziamento");
        }
    }
}
