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


