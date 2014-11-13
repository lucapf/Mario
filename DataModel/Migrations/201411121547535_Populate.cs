namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Populate : DbMigration
    {
        public override void Up()
        {
            mediatori.Initializer.Populate.init(); 
        }
        
        public override void Down()
        {
        }
    }
}
