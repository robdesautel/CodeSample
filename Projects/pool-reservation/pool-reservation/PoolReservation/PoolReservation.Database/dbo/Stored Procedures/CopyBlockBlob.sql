

create procedure CopyBlockBlob
    @sourceAccountName varchar(24),
    @accountName varchar(24),
    @sourceContainerName varchar(63),
    @sourceBlobName nvarchar(256),
    @sourceVersionTimestamp DATETIME,
    @containerName varchar(63),
    @blobName nvarchar(256),
    @oldDataDirectory nvarchar(260),
    @newDataDirectory nvarchar(260)
as
begin
    DECLARE @maxDateTime DATETIME
    SET @maxDateTime = '9999-12-31T23:59:59.997'

    IF (@sourceAccountName = @accountName and @sourceContainerName = @containerName and @sourceBlobName = @blobName and @sourceVersionTimestamp = @maxDateTime)
    BEGIN

        -- Delete all the uncommitted blocks for the blob
        DELETE FROM BlockData
        WHERE AccountName = @accountName
            AND ContainerName = @containerName
            AND BlobName = @blobName
            AND VersionTimestamp = @maxDateTime
            AND IsCommitted = 0

        UPDATE Blob
        SET LastModificationTime = GETUTCDATE()
        WHERE AccountName = @accountName
          AND ContainerName = @containerName
          AND BlobName = @blobName
          AND VersionTimestamp = @maxDateTime

        RETURN
    END
    
    
    -- Delete all the old blocks for the blob
    DELETE FROM BlockData
    WHERE AccountName = @accountName
      AND ContainerName = @containerName
      AND BlobName = @blobName
      AND VersionTimestamp = @maxDateTime

    -- Delete the committed blocks for the target
    DELETE FROM CommittedBlock
    WHERE AccountName = @accountName
      AND ContainerName = @containerName
      AND BlobName = @blobName
      AND VersionTimestamp = @maxDateTime

    -- Update the directory of the blob, if a new directory holds the data
    IF( @newDataDirectory IS NOT NULL
        AND @newDataDirectory != '' )
    BEGIN
        UPDATE Blob
        SET DirectoryPath = @newDataDirectory
        WHERE AccountName = @accountName
            AND ContainerName = @containerName
            AND BlobName = @blobName
            AND VersionTimestamp = @maxDateTime
    END

    -- Copy the blocks from the old snapshot to the new root.
    -- TODO: There is no sharing of blocks.
    -- Change the directory to new directory only if non null and non empty directory is given as input
    INSERT INTO BlockData
    SELECT @accountName, @containerName, @blobName, @maxDateTime, b.IsCommitted, b.BlockId, b.Length,b.StartOffset, dbo.ReplaceStringOnlyIfNotNullOrEmpty(b.FilePath,@oldDataDirectory,@newDataDirectory)
    FROM BlockData AS b
    WHERE b.AccountName = @sourceAccountName
      AND b.ContainerName = @sourceContainerName
      AND b.BlobName = @sourceBlobName
      AND b.IsCommitted = 1
      AND EXISTS (
            SELECT 1
            FROM CommittedBlock AS c
            WHERE c.AccountName = @sourceAccountName
              AND c.ContainerName = @sourceContainerName
              AND c.BlobName = @sourceBlobName
              AND c.VersionTimestamp = @sourceVersionTimestamp
              AND c.BlockId = b.BlockId
              AND c.BlockVersion = b.VersionTimestamp)
            

    -- Copy/create the committed block rows.
    INSERT INTO CommittedBlock
    SELECT @accountName, @containerName, @blobName, @maxDateTime, Offset, BlockId, Length, @maxDateTime
    FROM CommittedBlock
    WHERE AccountName = @sourceAccountName
      AND ContainerName = @sourceContainerName
      AND BlobName = @sourceBlobName
      AND VersionTimestamp = @sourceVersionTimestamp
end