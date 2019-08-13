

CREATE PROCEDURE ClearQueue (
    @accountName VARCHAR(24),
    @queueName VARCHAR(63))
AS
BEGIN
    SET NOCOUNT ON
    
    DELETE FROM QueueMessage
    WHERE AccountName = @accountName
      AND QueueName = @queueName
END