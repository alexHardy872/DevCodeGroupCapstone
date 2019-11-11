namespace DevCodeGroupCapstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adjustPersonIdToInt : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Lessons", "studentId", "dbo.People");
            DropForeignKey("dbo.Lessons", "teacherId", "dbo.People");
            DropIndex("dbo.Lessons", new[] { "studentId" });
            DropIndex("dbo.Lessons", new[] { "teacherId" });
            DropPrimaryKey("dbo.People");
            AlterColumn("dbo.Lessons", "studentId", c => c.Int());
            AlterColumn("dbo.Lessons", "teacherId", c => c.Int());
            AlterColumn("dbo.People", "PersonId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.People", "PersonId");
            CreateIndex("dbo.Lessons", "studentId");
            CreateIndex("dbo.Lessons", "teacherId");
            AddForeignKey("dbo.Lessons", "studentId", "dbo.People", "PersonId");
            AddForeignKey("dbo.Lessons", "teacherId", "dbo.People", "PersonId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Lessons", "teacherId", "dbo.People");
            DropForeignKey("dbo.Lessons", "studentId", "dbo.People");
            DropIndex("dbo.Lessons", new[] { "teacherId" });
            DropIndex("dbo.Lessons", new[] { "studentId" });
            DropPrimaryKey("dbo.People");
            AlterColumn("dbo.People", "PersonId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Lessons", "teacherId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Lessons", "studentId", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.People", "PersonId");
            CreateIndex("dbo.Lessons", "teacherId");
            CreateIndex("dbo.Lessons", "studentId");
            AddForeignKey("dbo.Lessons", "teacherId", "dbo.People", "PersonId");
            AddForeignKey("dbo.Lessons", "studentId", "dbo.People", "PersonId");
        }
    }
}
