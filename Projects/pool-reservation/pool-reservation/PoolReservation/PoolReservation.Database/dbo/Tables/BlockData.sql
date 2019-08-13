CREATE TABLE [dbo].[BlockData] (
    [AccountName]      VARCHAR (24)   NOT NULL,
    [ContainerName]    VARCHAR (63)   NOT NULL,
    [BlobName]         NVARCHAR (256) COLLATE Latin1_General_BIN NOT NULL,
    [VersionTimestamp] DATETIME       NOT NULL,
    [IsCommitted]      BIT            NOT NULL,
    [BlockId]          VARCHAR (128)  NOT NULL,
    [Length]           BIGINT         NULL,
    [StartOffset]      BIGINT         NOT NULL,
    [FilePath]         NVARCHAR (260) NULL,
    CONSTRAINT [PK_dbo.BlockData] PRIMARY KEY CLUSTERED ([AccountName] ASC, [ContainerName] ASC, [BlobName] ASC, [VersionTimestamp] ASC, [IsCommitted] ASC, [BlockId] ASC)
);


GO


CREATE TRIGGER BlockData_OnUpdate 
   ON dbo.BlockData
   AFTER INSERT, UPDATE
AS 
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    DECLARE @oldLength INT
    SELECT @oldLength = Blob.UncommittedBlockIdLength
    FROM Blob
    JOIN inserted
      ON Blob.AccountName = inserted.AccountName
      AND Blob.ContainerName = inserted.ContainerName
      AND Blob.BlobName = inserted.BlobName
      AND inserted.VersionTimestamp = Blob.VersionTimestamp
    WHERE inserted.IsCommitted = 0
      AND Blob.UncommittedBlockIdLength IS NOT NULL
      AND Blob.UncommittedBlockIdLength <> DATALENGTH(inserted.BlockId)
    
    IF (@oldLength IS NOT NULL)
    BEGIN
        RAISERROR('BlockIdMismatch', 16, 1)
        ROLLBACK TRANSACTION
    END

    UPDATE Blob
    SET UncommittedBlockIdLength = DATALENGTH(inserted.BlockId)
    FROM inserted
    WHERE Blob.AccountName = inserted.AccountName
      AND Blob.ContainerName = inserted.ContainerName
      AND Blob.BlobName = inserted.BlobName
      AND Blob.VersionTimestamp = inserted.VersionTimestamp
      AND inserted.IsCommitted = 0
      AND UncommittedBlockIdLength IS NULL
            
END