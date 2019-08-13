CREATE TABLE [dbo].[BlobContainer] (
    [AccountName]          VARCHAR (24)     NOT NULL,
    [ContainerName]        VARCHAR (63)     NOT NULL,
    [LastModificationTime] DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [ServiceMetadata]      VARBINARY (MAX)  NULL,
    [Metadata]             VARBINARY (MAX)  NULL,
    [LeaseId]              UNIQUEIDENTIFIER NULL,
    [LeaseState]           INT              DEFAULT ((0)) NULL,
    [LeaseDuration]        BIGINT           DEFAULT ((0)) NULL,
    [LeaseEndTime]         DATETIME         NULL,
    [IsLeaseOp]            BIT              DEFAULT ((0)) NULL,
    CONSTRAINT [PK_dbo.BlobContainer] PRIMARY KEY CLUSTERED ([AccountName] ASC, [ContainerName] ASC),
    CONSTRAINT [Account_BlobContainer] FOREIGN KEY ([AccountName]) REFERENCES [dbo].[Account] ([Name]) ON DELETE CASCADE
);


GO
CREATE TRIGGER BlobContainer_OnUpdate
   ON dbo.BlobContainer 
   AFTER UPDATE
AS 
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
	
    IF (EXISTS (
        SELECT 1
        FROM inserted JOIN deleted
        ON (inserted.IsLeaseOp = 0 OR inserted.IsLeaseOp IS NULL)))		
	BEGIN
		-- Insert statements for trigger here
		UPDATE BlobContainer 
		SET LastModificationTime = GETUTCDATE()
		FROM inserted JOIN BlobContainer
		ON inserted.AccountName = BlobContainer.AccountName
		  AND inserted.ContainerName = BlobContainer.ContainerName
	END
	-- If Lease Operation, set back the lease op flag to not lease op
    ELSE
    BEGIN
        IF (EXISTS (
            SELECT 1
            FROM inserted JOIN deleted
            ON inserted.IsLeaseOp = 1))
        BEGIN
            UPDATE BlobContainer
		    SET IsLeaseOp = 0
		    FROM inserted JOIN BlobContainer
		    ON  inserted.AccountName = BlobContainer.AccountName
		        AND inserted.ContainerName = BlobContainer.ContainerName
        END
    END
END