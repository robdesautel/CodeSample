

create procedure InsertCurrentPage
    @accountName varchar(24),
    @containerName varchar(63),
    @blobName nvarchar(256),
    @versionTimestamp DATETIME,
    @startOffset BIGINT,
    @endOffset BIGINT,
    @snapshotCount INT
as
begin
    -- If an existing page has a Start before InsertingStart and an End beyond InsertingStart (that is, they overlap or are adjacent where the existing page is on the left
    -- then coalesce the page we're inserting to include the existing page (move the InsertingStart value to the left). Only coalesce if the page has the same snapshot count for GetPageRangesDiff reasons.
    SELECT @startOffset = StartOffset
    FROM CurrentPage
    WHERE AccountName = @accountName
        AND ContainerName = @containerName
        AND BlobName = @blobName
        AND StartOffset < @startOffset
        AND EndOffset >= @startOffset
        AND SnapshotCount = @snapshotCount

    -- If an existing page has a Start before InsertingEnd and an End beyond InsertingEnd (that is, they overlap or are adjacent where the existing page is on the right
    -- then coalesce the page we're inserting to include the existing page (move the InsertingEnd value to the right). Only coalesce if the page has the same snapshot count for GetPageRangesDiff reasons.
    SELECT @endOffset = EndOffset
    FROM CurrentPage
    WHERE AccountName = @accountName
        AND ContainerName = @containerName
        AND BlobName = @blobName
        AND StartOffset <= @endOffset
        AND EndOffset > @endOffset
        AND SnapshotCount = @snapshotCount

    -- If an existing page has a Start before InsertingEnd and an End beyond InsertingEnd (that is, they overlap where the existing page is on the right), then trim the existing page to be
    -- adjacent (move the ExistingStart value to the right). Only trim if the page does not have the same snapshot count for GetPageRangesDiff reasons (otherwise, they should have been coalesced).
    UPDATE CurrentPage
    SET StartOffset = @endOffset
    WHERE AccountName = @accountName
        AND ContainerName = @containerName
        AND BlobName = @blobName
        AND VersionTimestamp = @versionTimestamp
        AND StartOffset >= @startOffset 
        AND StartOffset < @endOffset
        AND EndOffset > @endOffset
        AND SnapshotCount <> @snapshotCount

    -- If an existing page has a Start before InsertingStart and an End beyond InsertingStart (that is, they overlap where the existing page is on the left), then trim the existing page to be
    -- adjacent (move the ExistingEnd value to the left). Only trim if the page does not have the same snapshot count for GetPageRangesDiff reasons (otherwise, they should have been coalesced).
    UPDATE CurrentPage
    SET EndOffset = @startOffset
    WHERE AccountName = @accountName
        AND ContainerName = @containerName
        AND BlobName = @blobName
        AND VersionTimestamp = @versionTimestamp
        AND StartOffset < @startOffset
        AND EndOffset > @startOffset 
        AND EndOffset <= @endOffset
        AND SnapshotCount <> @snapshotCount

    -- This is the most confusing case -- an insert of a page wholly within another larger page where the snapshotcount is unequal, so we need to split the existing range into two bookend ranges.
    IF (EXISTS (
            SELECT 1 FROM CurrentPage
            WHERE AccountName = @accountName
            AND ContainerName = @containerName
            AND BlobName = @blobName
            AND StartOffset < @startOffset 
            AND EndOffset > @endOffset
            AND SnapshotCount <> @snapshotCount ) )
    BEGIN
        DECLARE @previousEndOffset BIGINT
        DECLARE @oldSnapshotCount INT
        SELECT @previousEndOffset = EndOffset, @oldSnapshotCount = SnapshotCount
        FROM CurrentPage
        WHERE AccountName = @accountName
            AND ContainerName = @containerName
            AND BlobName = @blobName
            AND StartOffset < @startOffset 
            AND EndOffset > @endOffset
            AND SnapshotCount <> @snapshotCount 

        -- Trim the existing page to be the left bookend first
        UPDATE CurrentPage
        SET EndOffset = @startOffset
        WHERE AccountName = @accountName
            AND ContainerName = @containerName
            AND BlobName = @blobName
            AND VersionTimestamp = @versionTimestamp
            AND StartOffset < @startOffset
            AND EndOffset > @endOffset
            AND SnapshotCount <> @snapshotCount

        -- Next, insert right bookend
        INSERT INTO CurrentPage(AccountName, ContainerName, BlobName, VersionTimestamp, StartOffset, EndOffset, SnapshotCount)
        VALUES(@accountName, @containerName, @blobName, @versionTimestamp, @endOffset, @previousEndOffset, @oldSnapshotCount)
    END 

    -- Clean up any existing pages that are overridden by coalesce operation.
    DELETE FROM CurrentPage
    WHERE AccountName = @accountName
        AND ContainerName = @containerName
        AND BlobName = @blobName
        AND StartOffset >= @startOffset 
        AND StartOffset < @endOffset
    
    -- Insert new, possibly coalesced page.
    INSERT INTO CurrentPage (AccountName, ContainerName, BlobName, VersionTimestamp, StartOffset, EndOffset, SnapshotCount)
    VALUES (@accountName, @containerName, @blobName, @versionTimestamp, @startOffset, @endOffset, @snapshotCount)
    
    -- Update LMT.
    UPDATE Blob
    SET LastModificationTime = GETUTCDATE()
    WHERE AccountName = @accountName
        AND ContainerName = @containerName
        AND BlobName = @blobName
        AND VersionTimestamp = @versionTimestamp
end