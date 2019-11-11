namespace DevCodeGroupCapstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LocationIdNullableOnLessonsTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Lessons", "LocationId", "dbo.Locations");
            DropIndex("dbo.Lessons", new[] { "LocationId" });
            AlterColumn("dbo.Lessons", "LocationId", c => c.Int());
            CreateIndex("dbo.Lessons", "LocationId");
            AddForeignKey("dbo.Lessons", "LocationId", "dbo.Locations", "LocationId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Lessons", "LocationId", "dbo.Locations");
            DropIndex("dbo.Lessons", new[] { "LocationId" });
            AlterColumn("dbo.Lessons", "LocationId", c => c.Int(nullable: false));
            CreateIndex("dbo.Lessons", "LocationId");
            AddForeignKey("dbo.Lessons", "LocationId", "dbo.Locations", "LocationId", cascadeDelete: true);
        }
    }
}
