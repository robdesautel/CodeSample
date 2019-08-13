

CREATE FUNCTION GetSummaryBlobContainers(
    @accountName nvarchar(24),
    @prefix nvarchar(256),
    @delimiter nvarchar(256)
    ) RETURNS TABLE
AS
RETURN (
    SELECT BlobContainer.*
    FROM (
        select distinct SummaryContainerName, MIN(ContainerName) AS ContainerName
        from (
            select dbo.MatchingDelimiter(ContainerName, @prefix, @delimiter) as SummaryContainerName, ContainerName
            from BlobContainer
            WHERE AccountName = @accountName
              AND ContainerName LIKE ISNULL(@prefix, '') + '%'
            ) as Summary
        group by SummaryContainerName) AS Summary
    JOIN BlobContainer
    on Summary.ContainerName = BlobContainer.ContainerName
    WHERE AccountName = @accountName
)