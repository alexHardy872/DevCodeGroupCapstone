namespace DevCodeGroupCapstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLessonTypeToLesson : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lessons", "LessonType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Lessons", "LessonType");
        }
    }
}
