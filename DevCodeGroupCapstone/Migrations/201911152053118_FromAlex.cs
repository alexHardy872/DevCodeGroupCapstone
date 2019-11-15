namespace DevCodeGroupCapstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FromAlex : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lessons", "requiresMakeup", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Lessons", "requiresMakeup");
        }
    }
}
