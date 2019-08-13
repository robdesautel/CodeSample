CREATE TABLE [dbo].[Blob] (
    [AccountName]              VARCHAR (24)     NOT NULL,
    [ContainerName]            VARCHAR (63)     NOT NULL,
    [BlobName]                 NVARCHAR (256)   COLLATE Latin1_General_BIN NOT NULL,
    [VersionTimestamp]         DATETIME         DEFAULT ('9999-12-31T23:59:59.997') NOT NULL,
    [BlobType]                 INT              NULL,
    [CreationTime]             DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [LastModificationTime]     DATETIME         DEFAULT (getutcdate()) NULL,
    [ContentLength]            BIGINT           NOT NULL,
    [ContentType]              VARCHAR (128)    NULL,
    [ContentCrc64]             BIGINT           NULL,
    [ContentMD5]               BINARY (16)      NULL,
    [ServiceMetadata]          VARBINARY (MAX)  NULL,
    [Metadata]                 VARBINARY (MAX)  NULL,
    [LeaseId]                  UNIQUEIDENTIFIER NULL,
    [LeaseDuration]            BIGINT           DEFAULT ((0)) NULL,
    [LeaseEndTime]             DATETIME         NULL,
    [SequenceNumber]           BIGINT           NULL,
    [LeaseState]               INT              DEFAULT ((0)) NULL,
    [IsLeaseOp]                BIT              DEFAULT ((0)) NULL,
    [SnapshotCount]            INT              NOT NULL,
    [GenerationId]             NVARCHAR (4000)  NOT NULL,
    [MaxSize]                  BIGINT           NULL,
    [FileName]                 NVARCHAR (260)   NULL,
    [IsCommitted]              BIT              NULL,
    [HasBlock]                 BIT              NULL,
    [UncommittedBlockIdLength] INT              DEFAULT ((0)) NULL,
    [DirectoryPath]            NVARCHAR (260)   NULL,
    CONSTRAINT [PK_dbo.Blob] PRIMARY KEY CLUSTERED ([AccountName] ASC, [ContainerName] ASC, [BlobName] ASC, [VersionTimestamp] ASC),
    CONSTRAINT [BlobContainer_Blob] FOREIGN KEY ([AccountName], [ContainerName]) REFERENCES [dbo].[BlobContainer] ([AccountName], [ContainerName]) ON DELETE CASCADE
);


GO


CREATE TRIGGER Blob_OnUpdate 
   ON dbo.Blob
   AFTER UPDATE
AS 
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    IF (EXISTS (
        SELECT 1
        FROM inserted JOIN deleted
        ON	ISNULL(inserted.UncommittedBlockIdLength, -1) = ISNULL(deleted.UncommittedBlockIdLength, -1)
            AND ( inserted.IsLeaseOp = 0 OR inserted.IsLeaseOp IS NULL)))
    BEGIN
		UPDATE Blob
		SET LastModificationTime = GETUTCDATE()
		FROM inserted JOIN Blob
		ON  inserted.AccountName = Blob.AccountName
		  AND inserted.ContainerName = Blob.ContainerName
		  AND inserted.BlobName = Blob.BlobName
		  AND inserted.VersionTimestamp = Blob.VersionTimestamp
		WHERE inserted.IsCommitted = 1 OR inserted.IsCommitted IS NULL
    END
    -- If Lease Operation, set back the lease op flag to not lease op
    ELSE
    BEGIN
        IF (EXISTS (
            SELECT 1
            FROM inserted JOIN deleted
            ON inserted.IsLeaseOp = 1))
        BEGIN
            UPDATE Blob
		    SET IsLeaseOp = 0
		    FROM inserted JOIN Blob
		    ON  inserted.AccountName = Blob.AccountName
		        AND inserted.ContainerName = Blob.ContainerName
		        AND inserted.BlobName = Blob.BlobName
		        AND inserted.VersionTimestamp = Blob.VersionTimestamp
        END
    END
END
GO


CREATE TRIGGER Blob_OnDelete
   ON dbo.Blob
   AFTER DELETE
AS 
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Delete all blocks that are not referenced by any committed block.
    -- We could make this smarter by restricting the blocks to a time range.
    -- and adding an index on BlockVersion
	DELETE FROM BlockData
	FROM deleted
	WHERE  deleted.AccountName = BlockData.AccountName
	  AND deleted.ContainerName = BlockData.ContainerName
	  AND deleted.BlobName = BlockData.BlobName
	  AND NOT EXISTS (
		SELECT 1
		FROM CommittedBlock AS c
		WHERE BlockData.AccountName = c.AccountName
		  AND BlockData.ContainerName = c.ContainerName
		  AND BlockData.BlobName = c.BlobName
		  AND BlockData.VersionTimestamp = c.BlockVersion )
END
GO


CREATE TRIGGER Blob_OnInsert 
   ON dbo.Blob
   AFTER INSERT
AS 
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

	UPDATE Blob
	SET LastModificationTime = '1753-01-01'
	FROM inserted JOIN Blob
	ON  inserted.AccountName = Blob.AccountName
	  AND inserted.ContainerName = Blob.ContainerName
	  AND inserted.BlobName = Blob.BlobName
	  AND inserted.VersionTimestamp = Blob.VersionTimestamp
	WHERE inserted.IsCommitted = 0
		  
END