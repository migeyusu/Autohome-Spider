namespace Car.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Brands",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Code = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Series",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Code = c.Int(nullable: false),
                        Name = c.String(),
                        Brand_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Brands", t => t.Brand_Id,cascadeDelete:true)
                .Index(t => t.Brand_Id);
            
            CreateTable(
                "dbo.Models",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Code = c.Int(nullable: false),
                        Name = c.String(),
                        Series_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Series", t => t.Series_Id,cascadeDelete:true)
                .Index(t => t.Series_Id);
            
            CreateTable(
                "dbo.SubModels",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Code = c.Int(nullable: false),
                        Name = c.String(),
                        Model_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Models", t => t.Model_Id,cascadeDelete:true)
                .Index(t => t.Model_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Series", "Brand_Id", "dbo.Brands");
            DropForeignKey("dbo.Models", "Series_Id", "dbo.Series");
            DropForeignKey("dbo.SubModels", "Model_Id", "dbo.Models");
            DropIndex("dbo.SubModels", new[] { "Model_Id" });
            DropIndex("dbo.Models", new[] { "Series_Id" });
            DropIndex("dbo.Series", new[] { "Brand_Id" });
            DropTable("dbo.SubModels");
            DropTable("dbo.Models");
            DropTable("dbo.Series");
            DropTable("dbo.Brands");
        }
    }
}
