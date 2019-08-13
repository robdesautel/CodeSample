

CREATE PROCEDURE DequeueMessages (
    @accountName VARCHAR(24),
    @queueName VARCHAR(63),
    @visibilityTimeout INT,
    @dequeueCount INT)
AS
BEGIN
    SET NOCOUNT ON
    
    DECLARE @currentTime DATETIME
    SET @currentTime = GETUTCDATE()

    DECLARE @newVisibilityTime DATETIME
    SET @newVisibilityTime = DATEADD(SS, @visibilityTimeout, GETUTCDATE())
    
    UPDATE TOP(@dequeueCount) QueueMessage
    SET VisibilityStartTime = @newVisibilityTime,
        DequeueCount = DequeueCount + 1
    OUTPUT inserted.*
    WHERE AccountName = @accountName
      AND QueueName = @queueName
      AND VisibilityStartTime <= @currentTime
      AND ExpiryTime >= @currentTime
END