

create procedure InsertPageHelper
    @accountName varchar(24),
    @containerName varchar(63),
    @blobName nvarchar(256),
    @versionTimestamp DATETIME,
    @startOffset BIGINT,
    @endOffset BIGINT,
    @fileOffset BIGINT,
    @snapshotCount INT,
    @deletePage BIT
as
begin
    INSERT INTO Page
    SELECT @accountName, @containerName, @blobName, @versionTimestamp, @endOffset, EndOffset, FileOffset + (@endOffset - StartOffset), @snapshotCount
    FROM Page
    WHERE AccountName = @accountName
        AND ContainerName = @containerName
        AND BlobName = @blobName
        AND VersionTimestamp = @versionTimestamp
        AND StartOffset BETWEEN @startOffset AND @endOffset
        AND EndOffset > @endOffset
    
    UPDATE Page
    SET EndOffset = @startOffset
    WHERE AccountName = @accountName
        AND ContainerName = @containerName
        AND BlobName = @blobName
        AND VersionTimestamp = @versionTimestamp
        AND StartOffset < @startOffset
        AND EndOffset BETWEEN @startOffset AND @endOffset

    DELETE FROM Page
    WHERE AccountName = @accountName
        AND ContainerName = @containerName
        AND BlobName = @blobName
        AND VersionTimestamp = @versionTimestamp
        AND ((StartOffset >= @startOffset AND StartOffset < @endOffset)
            OR (EndOffset > @startOffset AND EndOffset <= @endOffset))
       
    IF (@deletePage <> 1)
    BEGIN
        INSERT INTO PAGE
        VALUES (@accountName, @containerName, @blobName, @versionTimestamp, @startOffset, @endOffset, @fileOffset, @snapshotCount)
    END
end