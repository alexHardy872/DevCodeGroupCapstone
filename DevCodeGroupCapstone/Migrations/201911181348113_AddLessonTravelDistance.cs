namespace DevCodeGroupCapstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLessonTravelDistance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lessons", "travelDistance", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Lessons", "travelDistance");
        }
    }
}
