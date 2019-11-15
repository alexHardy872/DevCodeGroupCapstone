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


