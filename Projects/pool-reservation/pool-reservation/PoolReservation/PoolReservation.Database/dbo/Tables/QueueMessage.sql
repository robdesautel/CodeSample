CREATE TABLE [dbo].[QueueMessage] (
    [AccountName]         VARCHAR (24)     NOT NULL,
    [QueueName]           VARCHAR (63)     NOT NULL,
    [VisibilityStartTime] DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [MessageId]           UNIQUEIDENTIFIER NOT NULL,
    [ExpiryTime]          DATETIME         NOT NULL,
    [InsertionTime]       DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [DequeueCount]        INT              NULL,
    [Data]                VARBINARY (MAX)  NOT NULL,
    CONSTRAINT [PK_dbo.QueueMessage] PRIMARY KEY CLUSTERED ([AccountName] ASC, [QueueName] ASC, [VisibilityStartTime] ASC, [MessageId] ASC),
    CONSTRAINT [QueueContainer_QueueMessage] FOREIGN KEY ([AccountName], [QueueName]) REFERENCES [dbo].[QueueContainer] ([AccountName], [QueueName]) ON DELETE CASCADE
);

