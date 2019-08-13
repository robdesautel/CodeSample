CREATE PROCEDURE [dbo].[ChangeProcessingStatusToPending]
	@resId int
AS

	DECLARE @proccessingId uniqueidentifier = newid();

	UPDATE [dbo].[ReservationGroup]
	SET StatusId = 1, StatusDate = GETUTCDATE(), StatusGuid = NULL
	WHERE [Id] = @resId
	AND StatusId = 2
		 


