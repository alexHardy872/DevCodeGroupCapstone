namespace DevCodeGroupCapstone.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class numberProximalLessonsOnSchedule : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TeacherPreferences", "NumberOfProximalLessons", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.TeacherPreferences", "NumberOfProximalLessons");
        }
    }
}
