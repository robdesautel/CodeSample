CREATE TABLE [dbo].[QueueContainer] (
    [AccountName]          VARCHAR (24)    NOT NULL,
    [QueueName]            VARCHAR (63)    NOT NULL,
    [LastModificationTime] DATETIME        DEFAULT (getutcdate()) NOT NULL,
    [ServiceMetadata]      VARBINARY (MAX) NULL,
    [Metadata]             VARBINARY (MAX) NULL,
    CONSTRAINT [PK_dbo.QueueContainer] PRIMARY KEY CLUSTERED ([AccountName] ASC, [QueueName] ASC),
    CONSTRAINT [Account_QueueContainer] FOREIGN KEY ([AccountName]) REFERENCES [dbo].[Account] ([Name]) ON DELETE CASCADE
);

