<<<<<<< HEAD:DevCodeGroupCapstone/Migrations/201911141807321_resetModels.cs
﻿namespace DevCodeGroupCapstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class resetModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Lessons",
                c => new
                    {
                        LocationId = c.Int(),
                        LessonId = c.Int(nullable: false, identity: true),
                        subject = c.String(),
                        start = c.DateTime(nullable: false),
                        end = c.DateTime(nullable: false),
                        cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        teacherApproval = c.Boolean(nullable: false),
                        studentId = c.Int(),
                        teacherId = c.Int(),
                        Length = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LessonType = c.String(),
                        travelDuration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LessonId)
                .ForeignKey("dbo.Locations", t => t.LocationId)
                .ForeignKey("dbo.People", t => t.studentId)
                .ForeignKey("dbo.People", t => t.teacherId)
                .Index(t => t.LocationId)
                .Index(t => t.studentId)
                .Index(t => t.teacherId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationId = c.Int(nullable: false, identity: true),
                        lat = c.String(),
                        lng = c.String(),
                        address1 = c.String(),
                        address2 = c.String(),
                        city = c.String(),
                        state = c.Int(nullable: false),
                        zip = c.String(),
                        description = c.String(),
                    })
                .PrimaryKey(t => t.LocationId);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        PersonId = c.Int(nullable: false, identity: true),
                        firstName = c.String(),
                        lastName = c.String(),
                        subjects = c.String(),
                        phoneNumber = c.String(),
                        ApplicationId = c.String(maxLength: 128),
                        LocationId = c.Int(),
                    })
                .PrimaryKey(t => t.PersonId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationId)
                .ForeignKey("dbo.Locations", t => t.LocationId)
                .Index(t => t.ApplicationId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.MessageId);
            
            CreateTable(
                "dbo.TeacherPreferences",
                c => new
                    {
                        TeacherPreferenceId = c.Int(nullable: false, identity: true),
                        defaultLessonLength = c.Int(nullable: false),
                        distanceType = c.Int(nullable: false),
                        maxDistance = c.Int(nullable: false),
                        incrementalCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TimeBeforeCancellation = c.Int(nullable: false),
                        teacherId = c.Int(),
                    })
                .PrimaryKey(t => t.TeacherPreferenceId)
                .ForeignKey("dbo.People", t => t.teacherId)
                .Index(t => t.teacherId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.TeacherAvails",
                c => new
                    {
                        availId = c.Int(nullable: false, identity: true),
                        weekDay = c.Int(nullable: false),
                        start = c.DateTime(nullable: false),
                        end = c.DateTime(nullable: false),
                        PersonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.availId)
                .ForeignKey("dbo.People", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.PersonId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeacherAvails", "PersonId", "dbo.People");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.TeacherPreferences", "teacherId", "dbo.People");
            DropForeignKey("dbo.Lessons", "teacherId", "dbo.People");
            DropForeignKey("dbo.Lessons", "studentId", "dbo.People");
            DropForeignKey("dbo.People", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.People", "ApplicationId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Lessons", "LocationId", "dbo.Locations");
            DropIndex("dbo.TeacherAvails", new[] { "PersonId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.TeacherPreferences", new[] { "teacherId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.People", new[] { "LocationId" });
            DropIndex("dbo.People", new[] { "ApplicationId" });
            DropIndex("dbo.Lessons", new[] { "teacherId" });
            DropIndex("dbo.Lessons", new[] { "studentId" });
            DropIndex("dbo.Lessons", new[] { "LocationId" });
            DropTable("dbo.TeacherAvails");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.TeacherPreferences");
            DropTable("dbo.Messages");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.People");
            DropTable("dbo.Locations");
            DropTable("dbo.Lessons");
        }
    }
}
=======
﻿namespace DevCodeGroupCapstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Lessons",
                c => new
                    {
                        LocationId = c.Int(),
                        LessonId = c.Int(nullable: false, identity: true),
                        subject = c.String(),
                        start = c.DateTime(nullable: false),
                        end = c.DateTime(nullable: false),
                        cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        teacherApproval = c.Boolean(nullable: false),
                        studentId = c.Int(),
                        teacherId = c.Int(),
                        Length = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LessonType = c.String(),
                        travelDuration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LessonId)
                .ForeignKey("dbo.Locations", t => t.LocationId)
                .ForeignKey("dbo.People", t => t.studentId)
                .ForeignKey("dbo.People", t => t.teacherId)
                .Index(t => t.LocationId)
                .Index(t => t.studentId)
                .Index(t => t.teacherId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationId = c.Int(nullable: false, identity: true),
                        lat = c.String(),
                        lng = c.String(),
                        address1 = c.String(),
                        address2 = c.String(),
                        city = c.String(),
                        state = c.Int(nullable: false),
                        zip = c.String(),
                        description = c.String(),
                    })
                .PrimaryKey(t => t.LocationId);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        PersonId = c.Int(nullable: false, identity: true),
                        firstName = c.String(),
                        lastName = c.String(),
                        subjects = c.String(),
                        phoneNumber = c.String(),
                        ApplicationId = c.String(maxLength: 128),
                        LocationId = c.Int(),
                    })
                .PrimaryKey(t => t.PersonId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationId)
                .ForeignKey("dbo.Locations", t => t.LocationId)
                .Index(t => t.ApplicationId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.MessageId);
            
            CreateTable(
                "dbo.TeacherPreferences",
                c => new
                    {
                        TeacherPreferenceId = c.Int(nullable: false, identity: true),
                        defaultLessonLength = c.Int(nullable: false),
                        distanceType = c.Int(nullable: false),
                        maxDistance = c.Int(nullable: false),
                        incrementalCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TimeBeforeCancellation = c.Int(nullable: false),
                        teacherId = c.Int(),
                    })
                .PrimaryKey(t => t.TeacherPreferenceId)
                .ForeignKey("dbo.People", t => t.teacherId)
                .Index(t => t.teacherId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.TeacherAvails",
                c => new
                    {
                        availId = c.Int(nullable: false, identity: true),
                        weekDay = c.Int(nullable: false),
                        start = c.DateTime(nullable: false),
                        end = c.DateTime(nullable: false),
                        PersonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.availId)
                .ForeignKey("dbo.People", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.PersonId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeacherAvails", "PersonId", "dbo.People");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.TeacherPreferences", "teacherId", "dbo.People");
            DropForeignKey("dbo.Lessons", "teacherId", "dbo.People");
            DropForeignKey("dbo.Lessons", "studentId", "dbo.People");
            DropForeignKey("dbo.People", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.People", "ApplicationId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Lessons", "LocationId", "dbo.Locations");
            DropIndex("dbo.TeacherAvails", new[] { "PersonId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.TeacherPreferences", new[] { "teacherId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.People", new[] { "LocationId" });
            DropIndex("dbo.People", new[] { "ApplicationId" });
            DropIndex("dbo.Lessons", new[] { "teacherId" });
            DropIndex("dbo.Lessons", new[] { "studentId" });
            DropIndex("dbo.Lessons", new[] { "LocationId" });
            DropTable("dbo.TeacherAvails");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.TeacherPreferences");
            DropTable("dbo.Messages");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.People");
            DropTable("dbo.Locations");
            DropTable("dbo.Lessons");
        }
    }
}
>>>>>>> 0b5fcc19cb515c18fd19d984170abfb7de8a23de:DevCodeGroupCapstone/Migrations/201911141757139_Init.cs
