CREATE PROCEDURE [dbo].[UpdateStatusDate]
	@resId int
AS
	UPDATE [dbo].[ReservationGroup]
	SET StatusDate = GETUTCDATE()
	WHERE [Id] = @resId
	AND StatusId = 1
RETURN 0
