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


