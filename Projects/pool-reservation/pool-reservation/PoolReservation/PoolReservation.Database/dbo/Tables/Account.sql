CREATE TABLE [dbo].[Account] (
    [Name]                 VARCHAR (24)    NOT NULL,
    [SecretKey]            VARBINARY (256) NULL,
    [QueueServiceSettings] VARBINARY (MAX) NULL,
    [BlobServiceSettings]  VARBINARY (MAX) NULL,
    [TableServiceSettings] VARBINARY (MAX) NULL,
    [SecondaryKey]         VARBINARY (256) NULL,
    [SecondaryReadEnabled] BIT             DEFAULT ((1)) NULL,
    CONSTRAINT [PK_dbo.Account] PRIMARY KEY CLUSTERED ([Name] ASC)
);

