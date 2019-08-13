

create procedure SnapshotBlockBlob
    @accountName varchar(24),
    @containerName varchar(63),
    @blobName nvarchar(256),
    @metadata VARBINARY(MAX),
    @snapshotTimestamp DATETIME OUTPUT,
    @snapshotLastModificationTime DATETIME OUTPUT
as
begin
    
    DECLARE @maxDateTime DATETIME
    SET @maxDateTime = '9999-12-31T23:59:59.997'

    SET @snapshotTimestamp = GETUTCDATE()
    
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
          ,[IsCommitted]
          ,[HasBlock]
          ,[UncommittedBlockIdLength]
          ,[DirectoryPath]
          ,[SnapshotCount]
          ,[GenerationId])
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
          ,[IsCommitted]
          ,[HasBlock]
          ,[UncommittedBlockIdLength]
          ,[DirectoryPath]
          ,[SnapshotCount]
          ,[GenerationId]
    FROM Blob
    WHERE AccountName = @accountName
      AND ContainerName = @containerName
      AND BlobName = @blobName
      AND VersionTimestamp = @maxDateTime

    UPDATE BlockData
    SET VersionTimestamp = @snapshotTimestamp
    WHERE AccountName = @accountName
      AND ContainerName = @containerName
      AND BlobName = @blobName
      AND VersionTimestamp = @maxDateTime
      AND IsCommitted = 1

    UPDATE CommittedBlock
    SET BlockVersion = @snapshotTimestamp
    WHERE AccountName = @accountName
      AND ContainerName = @containerName
      AND BlobName = @blobName
      AND VersionTimestamp = @maxDateTime
      AND BlockVersion = @maxDateTime

    INSERT INTO CommittedBlock
    SELECT [AccountName]
          ,[ContainerName]
          ,[BlobName]
          ,@snapshotTimestamp
          ,[Offset]
          ,[BlockId]
          ,[Length]
          ,[BlockVersion]
    FROM CommittedBlock
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
end