CREATE TABLE [dbo].[DeletedAccount] (
    [Name]         VARCHAR (24) NOT NULL,
    [DeletionTime] DATETIME     NULL,
    CONSTRAINT [PK_DeletedAccount] PRIMARY KEY CLUSTERED ([Name] ASC)
);

