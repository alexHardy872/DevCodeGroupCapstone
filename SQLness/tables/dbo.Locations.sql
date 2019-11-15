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


