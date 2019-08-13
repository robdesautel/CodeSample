

CREATE PROCEDURE ClearUncommittedBlocks
    @accountName VARCHAR(24),
    @containerName VARCHAR(63),
    @blobName NVARCHAR(256)
AS
BEGIN
    DECLARE @versionTimestamp DATETIME
    SET @versionTimestamp = '9999-12-31T23:59:59.997'

    DELETE FROM BlockData
    WHERE AccountName = @accountName
      AND ContainerName = @containerName
      AND BlobName = @blobName
      AND IsCommitted = 0
      
    UPDATE Blob
    SET UncommittedBlockIdLength = NULL
    WHERE AccountName = @accountName
      AND ContainerName = @containerName
      AND BlobName = @blobName
      AND VersionTimestamp = @versionTimestamp
END