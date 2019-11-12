namespace DevCodeGroupCapstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTimeBeforeCancellationToTeacherPreferences : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TeacherPreferences", "TimeBeforeCancellation", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TeacherPreferences", "TimeBeforeCancellation");
        }
    }
}
