CREATE PROCEDURE [dbo].[CancelProcessing]
	@resId int
AS
	--UPDATE [dbo].[ReservationGroup]
	--SET IsProcessing = 0, ProcessingLastCheckIn = null, PendingLastCheckIn = GETUTCDATE()
	--WHERE [Id] = @resId
	--AND [IsProcessing] = 1
	--AND [IsPending] = 1