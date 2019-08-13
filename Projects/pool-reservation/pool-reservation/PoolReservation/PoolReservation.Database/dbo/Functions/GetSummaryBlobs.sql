

CREATE FUNCTION GetSummaryBlobs(
    @accountName nvarchar(24),
    @containerName nvarchar(63),
    @prefix nvarchar(256),
    @delimiter nvarchar(256)
    ) RETURNS TABLE
AS
RETURN (
    SELECT Blob.*
    FROM (
        select distinct SummaryBlobName, MIN(BlobName) AS BlobName
        from (
            select dbo.MatchingDelimiter(BlobName, @prefix, @delimiter) as SummaryBlobName, BlobName
            from Blob
            WHERE AccountName = @accountName
              AND ContainerName = @containerName
              AND BlobName LIKE dbo.EscapedLikeString(ISNULL(@prefix, '')) + '%'
            ) as Summary
        group by SummaryBlobName) AS Summary
    JOIN Blob
    on Summary.BlobName = Blob.BlobName
    WHERE AccountName = @accountName
      AND ContainerName = @containerName
)