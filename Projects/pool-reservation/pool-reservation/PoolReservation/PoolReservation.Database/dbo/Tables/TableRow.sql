CREATE TABLE [dbo].[TableRow] (
    [AccountName]  VARCHAR (24)   NOT NULL,
    [TableName]    VARCHAR (63)   COLLATE Latin1_General_BIN NOT NULL,
    [PartitionKey] NVARCHAR (256) COLLATE Latin1_General_BIN NOT NULL,
    [RowKey]       NVARCHAR (256) COLLATE Latin1_General_BIN NOT NULL,
    [Timestamp]    DATETIME       DEFAULT (getutcdate()) NOT NULL,
    [Data]         XML            NULL,
    CONSTRAINT [PK_dbo.TableRow] PRIMARY KEY CLUSTERED ([AccountName] ASC, [TableName] ASC, [PartitionKey] ASC, [RowKey] ASC),
    CONSTRAINT [TableContainer_TableRow] FOREIGN KEY ([AccountName], [TableName]) REFERENCES [dbo].[TableContainer] ([AccountName], [TableName]) ON DELETE CASCADE
);


GO


CREATE TRIGGER TableRow_OnUpdate 
   ON dbo.TableRow
   AFTER UPDATE
AS 
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

	DECLARE @rowLength INT
	SELECT @rowLength = (DATALENGTH(inserted.PartitionKey) + DATALENGTH(inserted.RowKey) + DATALENGTH(inserted.Timestamp) + DATALENGTH(inserted.Data))
	FROM inserted
	
	IF (@rowLength >= 1024 * 1024)
	BEGIN
        RAISERROR('EntityTooLarge', 16, 1)
        ROLLBACK TRANSACTION
        
        RETURN
	END
	
    -- Insert statements for trigger here
    UPDATE TableRow SET [Timestamp] = GETUTCDATE()
    FROM inserted JOIN TableRow
    ON  inserted.AccountName = TableRow.AccountName
      AND inserted.TableName = TableRow.TableName
      AND inserted.PartitionKey = TableRow.PartitionKey
      AND inserted.RowKey = TableRow.RowKey	  
END
GO


CREATE TRIGGER TableRow_OnInsert
   ON dbo.TableRow
   AFTER INSERT
AS 
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

	DECLARE @rowLength INT
	SELECT @rowLength = (DATALENGTH(inserted.PartitionKey) + DATALENGTH(inserted.RowKey) + DATALENGTH(inserted.Timestamp) + DATALENGTH(inserted.Data))
	FROM inserted
	
	IF (@rowLength >= 1024 * 1024)
	BEGIN
        RAISERROR('EntityTooLarge', 16, 1)
        ROLLBACK TRANSACTION
        
        RETURN
	END
END