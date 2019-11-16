USE [aspnet-DevCodeGroupCapstone-20191110123838]
GO

/****** Object: Table [dbo].[AspNetUserClaims] Script Date: 11/14/2019 4:38:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [UserId]     NVARCHAR (128) NOT NULL,
    [ClaimType]  NVARCHAR (MAX) NULL,
    [ClaimValue] NVARCHAR (MAX) NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[AspNetUserClaims]([UserId] ASC);


GO
ALTER TABLE [dbo].[AspNetUserClaims]
    ADD CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC);


GO
ALTER TABLE [dbo].[AspNetUserClaims]
    ADD CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE;



USE [aspnet-DevCodeGroupCapstone-20191110123838]
GO

/****** Object: Table [dbo].[AspNetUsers] Script Date: 11/14/2019 4:40:12 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AspNetUsers] (
    [Id]                   NVARCHAR (128) NOT NULL,
    [Email]                NVARCHAR (256) NULL,
    [EmailConfirmed]       BIT            NOT NULL,
    [PasswordHash]         NVARCHAR (MAX) NULL,
    [SecurityStamp]        NVARCHAR (MAX) NULL,
    [PhoneNumber]          NVARCHAR (MAX) NULL,
    [PhoneNumberConfirmed] BIT            NOT NULL,
    [TwoFactorEnabled]     BIT            NOT NULL,
    [LockoutEndDateUtc]    DATETIME       NULL,
    [LockoutEnabled]       BIT            NOT NULL,
    [AccessFailedCount]    INT            NOT NULL,
    [UserName]             NVARCHAR (256) NOT NULL
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex]
    ON [dbo].[AspNetUsers]([UserName] ASC);


GO
ALTER TABLE [dbo].[AspNetUsers]
    ADD CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC);




USE [aspnet-DevCodeGroupCapstone-20191110123838]
GO




/****** Object: Table [dbo].[AspNetUserRoles] Script Date: 11/14/2019 4:39:41 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AspNetUserRoles] (
    [UserId] NVARCHAR (128) NOT NULL,
    [RoleId] NVARCHAR (128) NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[AspNetUserRoles]([UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RoleId]
    ON [dbo].[AspNetUserRoles]([RoleId] ASC);


GO
ALTER TABLE [dbo].[AspNetUserRoles]
    ADD CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC);


GO
ALTER TABLE [dbo].[AspNetUserRoles]
    ADD CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE;


GO
ALTER TABLE [dbo].[AspNetUserRoles]
    ADD CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE;





USE [aspnet-DevCodeGroupCapstone-20191110123838]
GO




/****** Object: Table [dbo].[AspNetRoles] Script Date: 11/14/2019 8:36:58 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AspNetRoles] (
    [Id]   NVARCHAR (128) NOT NULL,
    [Name] NVARCHAR (256) NOT NULL
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex]
    ON [dbo].[AspNetRoles]([Name] ASC);


GO
ALTER TABLE [dbo].[AspNetRoles]
    ADD CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED ([Id] ASC);





USE [aspnet-DevCodeGroupCapstone-20191110123838]
GO




/****** Object: Table [dbo].[AspNetUserLogins] Script Date: 11/14/2019 4:39:26 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] NVARCHAR (128) NOT NULL,
    [ProviderKey]   NVARCHAR (128) NOT NULL,
    [UserId]        NVARCHAR (128) NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[AspNetUserLogins]([UserId] ASC);


GO
ALTER TABLE [dbo].[AspNetUserLogins]
    ADD CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC, [UserId] ASC);


GO
ALTER TABLE [dbo].[AspNetUserLogins]
    ADD CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE;






USE [aspnet-DevCodeGroupCapstone-20191110123838]
GO

/****** Object: Table [dbo].[People] Script Date: 11/14/2019 4:41:05 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[People] (
    [PersonId]      INT            IDENTITY (1, 1) NOT NULL,
    [firstName]     NVARCHAR (MAX) NULL,
    [lastName]      NVARCHAR (MAX) NULL,
    [subjects]      NVARCHAR (MAX) NULL,
    [phoneNumber]   NVARCHAR (MAX) NULL,
    [ApplicationId] NVARCHAR (128) NULL,
    [LocationId]    INT            NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_ApplicationId]
    ON [dbo].[People]([ApplicationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_LocationId]
    ON [dbo].[People]([LocationId] ASC);


GO
ALTER TABLE [dbo].[People]
    ADD CONSTRAINT [PK_dbo.People] PRIMARY KEY CLUSTERED ([PersonId] ASC);


GO
ALTER TABLE [dbo].[People]
    ADD CONSTRAINT [FK_dbo.People_dbo.AspNetUsers_ApplicationId] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[AspNetUsers] ([Id]);


GO
ALTER TABLE [dbo].[People]
    ADD CONSTRAINT [FK_dbo.People_dbo.Locations_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Locations] ([LocationId]);



USE [aspnet-DevCodeGroupCapstone-20191110123838]
GO

/****** Object: Table [dbo].[Locations] Script Date: 11/14/2019 4:40:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Locations] (
    [LocationId]  INT            IDENTITY (1, 1) NOT NULL,
    [lat]         NVARCHAR (MAX) NULL,
    [lng]         NVARCHAR (MAX) NULL,
    [address1]    NVARCHAR (MAX) NULL,
    [address2]    NVARCHAR (MAX) NULL,
    [city]        NVARCHAR (MAX) NULL,
    [state]       INT            NOT NULL,
    [zip]         NVARCHAR (MAX) NULL,
    [description] NVARCHAR (MAX) NULL
);




USE [aspnet-DevCodeGroupCapstone-20191110123838]
GO

/****** Object: Table [dbo].[TeacherPreferences] Script Date: 11/14/2019 4:41:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TeacherPreferences] (
    [TeacherPreferenceId]     INT             IDENTITY (1, 1) NOT NULL,
    [defaultLessonLength]     INT             NOT NULL,
    [distanceType]            INT             NOT NULL,
    [maxDistance]             INT             NOT NULL,
    [incrementalCost]         DECIMAL (18, 2) NOT NULL,
    [TimeBeforeCancellation]  INT             NOT NULL,
    [teacherId]               INT             NULL,
    [NumberOfProximalLessons] INT             NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_teacherId]
    ON [dbo].[TeacherPreferences]([teacherId] ASC);


GO
ALTER TABLE [dbo].[TeacherPreferences]
    ADD CONSTRAINT [PK_dbo.TeacherPreferences] PRIMARY KEY CLUSTERED ([TeacherPreferenceId] ASC);


GO
ALTER TABLE [dbo].[TeacherPreferences]
    ADD CONSTRAINT [FK_dbo.TeacherPreferences_dbo.People_teacherId] FOREIGN KEY ([teacherId]) REFERENCES [dbo].[People] ([PersonId]);




USE [aspnet-DevCodeGroupCapstone-20191110123838]
GO

/****** Object: Table [dbo].[TeacherAvails] Script Date: 11/14/2019 4:41:15 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TeacherAvails] (
    [availId]  INT      IDENTITY (1, 1) NOT NULL,
    [weekDay]  INT      NOT NULL,
    [start]    DATETIME NOT NULL,
    [end]      DATETIME NOT NULL,
    [PersonId] INT      NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_PersonId]
    ON [dbo].[TeacherAvails]([PersonId] ASC);


GO
ALTER TABLE [dbo].[TeacherAvails]
    ADD CONSTRAINT [PK_dbo.TeacherAvails] PRIMARY KEY CLUSTERED ([availId] ASC);


GO
ALTER TABLE [dbo].[TeacherAvails]
    ADD CONSTRAINT [FK_dbo.TeacherAvails_dbo.People_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[People] ([PersonId]) ON DELETE CASCADE;



USE [aspnet-DevCodeGroupCapstone-20191110123838]
GO

/****** Object: Table [dbo].[Lessons] Script Date: 11/14/2019 4:40:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Lessons] (
    [LocationId]      INT             NULL,
    [LessonId]        INT             IDENTITY (1, 1) NOT NULL,
    [subject]         NVARCHAR (MAX)  NULL,
    [start]           DATETIME        NOT NULL,
    [end]             DATETIME        NOT NULL,
    [cost]            DECIMAL (18, 2) NOT NULL,
    [teacherApproval] BIT             NOT NULL,
    [studentId]       INT             NULL,
    [teacherId]       INT             NULL,
    [Length]          INT             NOT NULL,
    [Price]           DECIMAL (18, 2) NOT NULL,
    [LessonType]      NVARCHAR (MAX)  NULL,
    [travelDuration]  INT             NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_LocationId]
    ON [dbo].[Lessons]([LocationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_studentId]
    ON [dbo].[Lessons]([studentId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_teacherId]
    ON [dbo].[Lessons]([teacherId] ASC);


GO
ALTER TABLE [dbo].[Lessons]
    ADD CONSTRAINT [PK_dbo.Lessons] PRIMARY KEY CLUSTERED ([LessonId] ASC);


GO
ALTER TABLE [dbo].[Lessons]
    ADD CONSTRAINT [FK_dbo.Lessons_dbo.Locations_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Locations] ([LocationId]);


GO
ALTER TABLE [dbo].[Lessons]
    ADD CONSTRAINT [FK_dbo.Lessons_dbo.People_studentId] FOREIGN KEY ([studentId]) REFERENCES [dbo].[People] ([PersonId]);


GO
ALTER TABLE [dbo].[Lessons]
    ADD CONSTRAINT [FK_dbo.Lessons_dbo.People_teacherId] FOREIGN KEY ([teacherId]) REFERENCES [dbo].[People] ([PersonId]);



USE [aspnet-DevCodeGroupCapstone-20191110123838]
GO

/****** Object: Table [dbo].[Messages] Script Date: 11/14/2019 4:40:50 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Messages] (
    [MessageId] INT IDENTITY (1, 1) NOT NULL
);


