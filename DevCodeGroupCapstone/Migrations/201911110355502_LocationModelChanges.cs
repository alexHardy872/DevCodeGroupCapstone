namespace DevCodeGroupCapstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LocationModelChanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Locations", "lat", c => c.String());
            AlterColumn("dbo.Locations", "lng", c => c.String());
            AlterColumn("dbo.Locations", "state", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Locations", "state", c => c.String());
            AlterColumn("dbo.Locations", "lng", c => c.Double(nullable: false));
            AlterColumn("dbo.Locations", "lat", c => c.Double(nullable: false));
        }
    }
}
