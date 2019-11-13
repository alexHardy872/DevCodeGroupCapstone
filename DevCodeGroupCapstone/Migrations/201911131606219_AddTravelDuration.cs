namespace DevCodeGroupCapstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTravelDuration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lessons", "travelDuration", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Lessons", "travelDuration");
        }
    }
}
