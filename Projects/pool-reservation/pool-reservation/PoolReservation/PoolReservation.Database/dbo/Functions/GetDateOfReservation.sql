CREATE FUNCTION [dbo].[GetDateOfReservation]
(
	@ReservationId int
)
RETURNS datetime
AS
BEGIN
	DECLARE @Date datetime;
	
	if @ReservationId is null
	Begin
		Return GETDATE();
	End

	

	SET @Date = (SELECT TOP(1) r.[Date]
	FROM Reservations r
	WHERE r.Id = @ReservationId);

	
	RETURN @Date
END
