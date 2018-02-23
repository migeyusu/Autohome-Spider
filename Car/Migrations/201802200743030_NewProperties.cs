namespace Car.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SubModels", "IsAvailable", c => c.Boolean(nullable: false));
            AddColumn("dbo.SubModels", "Year", c => c.Int(nullable: false));
            AddColumn("dbo.SubModels", "PropertyData", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SubModels", "PropertyData");
            DropColumn("dbo.SubModels", "Year");
            DropColumn("dbo.SubModels", "IsAvailable");
        }
    }
}
