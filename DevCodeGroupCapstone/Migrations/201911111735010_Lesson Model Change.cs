namespace DevCodeGroupCapstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LessonModelChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lessons", "Length", c => c.Int(nullable: false));
            AddColumn("dbo.Lessons", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Lessons", "cost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Lessons", "cost", c => c.Double(nullable: false));
            DropColumn("dbo.Lessons", "Price");
            DropColumn("dbo.Lessons", "Length");
        }
    }
}
