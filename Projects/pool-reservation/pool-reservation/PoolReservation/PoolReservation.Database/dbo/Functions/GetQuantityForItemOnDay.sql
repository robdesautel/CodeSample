CREATE FUNCTION [dbo].[GetQuantityForItemOnDay]
(
	@itemId int,
	@date datetime
)
RETURNS INT
AS
BEGIN
	if @itemId is null OR @date is null
	Begin
		Return 0;
	End

	--DECLARE @dateOnly datetime;

	--DECLARE @reservationCount int;

	--DECLARE @availableQuantity int;

	--SET @dateOnly = CAST(@date as DATE)

	

	--SET @reservationCount = ( SELECT COUNT(*) 
	--FROM ReserveItems ri
	--WHERE ri.ItemId = @itemId
	--AND ri.IgnoreQuantityRestrictions = 0
	--AND @dateOnly in (
	--	Select r.[Date]
	--	FROM Reservations r
	--	WHERE ri.ReservationId = r.Id
	--	AND  CAST(r.[Date] as DATE) = @dateOnly
	--));	
	
	--SET @availableQuantity = (SELECT vi.NormalQuantity
	--FROM VenueItems vi
	--WHERE vi.Id = @itemId);

	--RETURN (@availableQuantity - @reservationCount)

	Return -1

END
