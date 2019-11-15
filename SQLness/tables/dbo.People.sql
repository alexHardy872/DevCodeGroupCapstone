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


