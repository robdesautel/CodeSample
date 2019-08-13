

create procedure SnapshotPageBlob
    @accountName varchar(24),
    @containerName varchar(63),
    @blobName nvarchar(256),
    @metadata VARBINARY(MAX),
    @pageFileName nvarchar(260),
    @snapshotCount INT,
    @snapshotTimestamp DATETIME OUTPUT,
    @snapshotLastModificationTime DATETIME OUTPUT
as
begin
    
    DECLARE @maxDateTime DATETIME
    SET @maxDateTime = '9999-12-31T23:59:59.997'
    DECLARE @newSnapshotCount INT
    SET @newSnapshotCount = @snapshotCount + 1

    SET @snapshotTimestamp = GETUTCDATE()
    -- Start by making the new entry for the snapshot blob in the Blob table.
    INSERT INTO Blob (
           [AccountName]
          ,[ContainerName]
          ,[BlobName]
          ,[VersionTimestamp]
          ,[BlobType]
          ,[CreationTime]
          ,[LastModificationTime]
          ,[ContentLength]
          ,[ContentType]
          ,[ContentMD5]
          ,[ServiceMetadata]
          ,[Metadata]
          ,[LeaseId]
          ,[LeaseState]
          ,[LeaseDuration]
          ,[LeaseEndTime]
          ,[SequenceNumber]
          ,[MaxSize]
          ,[FileName]
          ,[SnapshotCount]
          ,[GenerationId]
          )
    SELECT [AccountName]
          ,[ContainerName]
          ,[BlobName]
          ,@snapshotTimestamp
          ,[BlobType]
          ,[CreationTime]
          ,[LastModificationTime]
          ,[ContentLength]
          ,[ContentType]
          ,[ContentMD5]
          ,[ServiceMetadata]
          ,[Metadata]
          ,[LeaseId]
          ,[LeaseState]
          ,[LeaseDuration]
          ,[LeaseEndTime]
          ,[SequenceNumber]
          ,[MaxSize]
          ,@pageFileName
          ,[SnapshotCount]
          ,[GenerationId]
    FROM Blob
    WHERE AccountName = @accountName
      AND ContainerName = @containerName
      AND BlobName = @blobName
      AND VersionTimestamp = @maxDateTime

      -- Copy pages from root blob pages table to snapshot blob pages table.
    INSERT INTO Page
    SELECT [AccountName]
          ,[ContainerName]
          ,[BlobName]
          ,@snapshotTimestamp
          ,[StartOffset]
          ,[EndOffset]
          ,[StartOffset] -- FileOffset is the same as StartOffset, since we copy the entire file.
          ,[SnapshotCount]
    FROM CurrentPage
    WHERE AccountName = @accountName
      AND ContainerName = @containerName
      AND BlobName = @blobName
      AND VersionTimestamp = @maxDateTime
          
    IF (@metadata IS NOT NULL)
    BEGIN
        UPDATE Blob
        SET Metadata = @metadata
        WHERE AccountName = @accountName
          AND ContainerName = @containerName
          AND BlobName = @blobName
          AND VersionTimestamp = @snapshotTimestamp

        SELECT @snapshotLastModificationTime = LastModificationTime
        FROM Blob
        WHERE AccountName = @accountName
          AND ContainerName = @containerName
          AND BlobName = @blobName
          AND VersionTimestamp = @snapshotTimestamp
    END

    --Update snapshot count on root blob.
    UPDATE Blob
    SET SnapshotCount = @newSnapshotCount, IsLeaseOp = 1 -- Reuse IsLeaseOp to keep LMT from changing.
    WHERE AccountName = @accountName
        AND ContainerName = @containerName
        AND BlobName = @blobName
        AND VersionTimestamp = @maxDateTime

    -- This should be dead code. 
    -- Copy the pages
    INSERT INTO Page
    SELECT [AccountName]
          ,[ContainerName]
          ,[BlobName]
          ,@snapshotTimestamp
          ,[StartOffset]
          ,[EndOffset]
          ,[StartOffset] -- FileOffset is the same as StartOffset, since we copy the entire file.
          ,[SnapshotCount]
    FROM Page
    WHERE AccountName = @accountName
      AND ContainerName = @containerName
      AND BlobName = @blobName
      AND VersionTimestamp = @maxDateTime

end