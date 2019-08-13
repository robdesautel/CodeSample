CREATE TABLE [dbo].[TableContainer] (
    [AccountName]            VARCHAR (24)    NOT NULL,
    [TableName]              VARCHAR (63)    COLLATE Latin1_General_BIN NOT NULL,
    [LastModificationTime]   DATETIME        DEFAULT (getutcdate()) NOT NULL,
    [ServiceMetadata]        VARBINARY (MAX) NULL,
    [Metadata]               VARBINARY (MAX) NULL,
    [CasePreservedTableName] VARCHAR (63)    COLLATE Latin1_General_BIN NOT NULL,
    CONSTRAINT [PK_dbo.TableContainer] PRIMARY KEY CLUSTERED ([AccountName] ASC, [TableName] ASC),
    CONSTRAINT [Account_TableContainer] FOREIGN KEY ([AccountName]) REFERENCES [dbo].[Account] ([Name]) ON DELETE CASCADE
);

