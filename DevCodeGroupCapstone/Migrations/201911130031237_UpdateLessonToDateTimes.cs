namespace DevCodeGroupCapstone.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UpdateLessonToDateTimes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lessons", "start", c => c.DateTime(nullable: false));
            AddColumn("dbo.Lessons", "end", c => c.DateTime(nullable: false));
            DropColumn("dbo.Lessons", "startTime");
            DropColumn("dbo.Lessons", "endTime");
        }

        public override void Down()
        {
            AddColumn("dbo.Lessons", "endTime", c => c.Double(nullable: false));
            AddColumn("dbo.Lessons", "startTime", c => c.Double(nullable: false));
            DropColumn("dbo.Lessons", "end");
            DropColumn("dbo.Lessons", "start");
        }
    }
}
