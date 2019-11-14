namespace DevCodeGroupCapstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedTypoInTeacherPreferenceModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TeacherPreferences", "incrementalCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.TeacherPreferences", "incrementalCost");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TeacherPreferences", "incrementalCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.TeacherPreferences", "incrementalCost");
        }
    }
}
