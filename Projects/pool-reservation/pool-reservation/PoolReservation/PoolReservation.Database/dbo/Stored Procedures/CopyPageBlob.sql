

create procedure CopyPageBlob
    @sourceAccountName varchar(24),
    @accountName varchar(24),
    @sourceContainerName varchar(63),
    @sourceBlobName nvarchar(256),
    @sourceVersionTimestamp DATETIME,
    @containerName varchar(63),
    @blobName nvarchar(256)
as
begin
    DECLARE @maxDateTime DATETIME
    SET @maxDateTime = '9999-12-31T23:59:59.997'

    IF (@sourceAccountName = @accountName and @sourceContainerName = @containerName and @sourceBlobName = @blobName and @sourceVersionTimestamp = @maxDateTime)
    BEGIN
        UPDATE Blob
        SET LastModificationTime = GETUTCDATE()
        WHERE AccountName = @accountName
          AND ContainerName = @containerName
          AND BlobName = @blobName
          AND VersionTimestamp = @maxDateTime

        RETURN
    END
    
    DELETE FROM CurrentPage
    WHERE AccountName = @accountName
      AND ContainerName = @containerName
      AND BlobName = @blobName
      AND VersionTimestamp = @maxDateTime
    
    INSERT INTO CurrentPage
    SELECT @accountName, @containerName, @blobName, @maxDateTime, StartOffset, EndOffset, 0
    FROM CurrentPage
    WHERE AccountName = @sourceAccountName
      AND ContainerName = @sourceContainerName
      AND BlobName = @sourceBlobName
      AND VersionTimestamp = @sourceVersionTimestamp
end