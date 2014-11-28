namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update15 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.stato", "gruppoId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.stato", "gruppoId", c => c.Int(nullable: false));
        }
    }
}
