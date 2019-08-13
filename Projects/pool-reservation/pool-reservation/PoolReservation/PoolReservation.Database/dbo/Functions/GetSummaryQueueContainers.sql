

CREATE FUNCTION GetSummaryQueueContainers(
    @accountName nvarchar(24),
    @prefix nvarchar(256),
    @delimiter nvarchar(256)
    ) RETURNS TABLE
AS
RETURN (
    SELECT QueueContainer.*
    FROM (
        select distinct SummaryQueueName, MIN(QueueName) AS QueueName
        from (
            select dbo.MatchingDelimiter(QueueName, @prefix, @delimiter) as SummaryQueueName, QueueName
            from QueueContainer
            WHERE AccountName = @accountName
              AND QueueName LIKE ISNULL(@prefix, '') + '%'
            ) as Summary
        group by SummaryQueueName) AS Summary
    JOIN QueueContainer
    on Summary.QueueName = QueueContainer.QueueName
    WHERE AccountName = @accountName
)