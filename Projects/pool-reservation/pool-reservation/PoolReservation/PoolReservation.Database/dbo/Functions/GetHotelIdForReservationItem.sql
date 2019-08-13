CREATE FUNCTION [dbo].[GetHotelIdForReservationItem]
(
	@ReservationId int
)
RETURNS INT
AS
BEGIN
	DECLARE @hotelId int;
	
	if @ReservationId is null
	Begin
		Return 0;
	End

	

	SET @hotelId = (SELECT TOP(1) rg.HotelId
	FROM ReservationGroup rg, Reservations r
	WHERE r.Id = @ReservationId
	AND rg.Id = r.ReservationGroupId);

	
	RETURN @hotelId
END
