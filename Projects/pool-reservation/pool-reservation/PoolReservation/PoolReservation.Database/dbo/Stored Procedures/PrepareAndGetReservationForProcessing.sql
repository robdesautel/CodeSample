CREATE PROCEDURE [dbo].[PrepareAndGetReservationForProcessing]
	@resId int
AS

	DECLARE @proccessingId uniqueidentifier = newid();

	UPDATE [dbo].[ReservationGroup]
	SET StatusId = 2, StatusDate = GETUTCDATE(), StatusGuid = @proccessingId
	WHERE [Id] = @resId
	AND StatusId = 1
	
	

	SELECT *
	FROM [dbo].[ReservationGroup]
	WHERE [Id] = @resId
		AND StatusId = 2
		AND StatusGuid = @proccessingId
		 


