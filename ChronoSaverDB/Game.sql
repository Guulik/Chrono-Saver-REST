﻿CREATE TABLE [dbo].[Game]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1, 1),

	[Uid] UNIQUEIDENTIFIER NOT NULL UNIQUE,

	[UserID] BIGINT NOT NULL FOREIGN KEY REFERENCES [dbo].[User]([Id]), 

	[Name] VARCHAR(256) NOT NULL,

	[SavePath] VARCHAR(512) NOT NULL
)
