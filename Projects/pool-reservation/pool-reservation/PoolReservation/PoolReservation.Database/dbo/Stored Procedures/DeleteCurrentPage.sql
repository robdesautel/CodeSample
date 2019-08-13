

create procedure DeleteCurrentPage
    @accountName varchar(24),
    @containerName varchar(63),
    @blobName nvarchar(256),
    @versionTimestamp DATETIME,
    @startOffset BIGINT,
    @endOffset BIGINT
as
begin
    -- If both the start and end are within a single current page,
    -- the page needs to be split, so make a copy
    INSERT INTO CurrentPage
    SELECT AccountName, ContainerName, BlobName, VersionTimestamp, @endOffset, EndOffset, SnapshotCount
    FROM CurrentPage
    WHERE AccountName = @accountName
        AND ContainerName = @containerName
        AND BlobName = @blobName
        AND StartOffset < @startOffset
        AND EndOffset > @endOffset
    
    -- If the end offset of some existing page is in the middle of this page, trim the right end to clear the space where we're deleting
    UPDATE CurrentPage
    SET EndOffset = @startOffset
    WHERE AccountName = @accountName
        AND ContainerName = @containerName
        AND BlobName = @blobName
        AND StartOffset < @startOffset
        AND EndOffset > @startOffset
    
    -- If the start offset of some page is in the middle of this page, trim the left end to clear the space where we're deleting
    UPDATE CurrentPage
    SET StartOffset = @endOffset
    WHERE AccountName = @accountName
        AND ContainerName = @containerName
        AND BlobName = @blobName
        AND StartOffset < @endOffset
        AND EndOffset > @endOffset
    
    -- Remove anything that starts between @startOffset AND @endOffset (any pages fully contained within the deletion range)
    DELETE FROM CurrentPage
    WHERE AccountName = @accountName
        AND ContainerName = @containerName
        AND BlobName = @blobName
        AND StartOffset >= @startOffset
        AND EndOffset <= @endOffset

    UPDATE Blob
    SET LastModificationTime = GETUTCDATE()
    WHERE AccountName = @accountName
        AND ContainerName = @containerName
        AND BlobName = @blobName
        AND VersionTimestamp = @versionTimestamp
end