-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION CheckItemInHotel
(
	-- Add the parameters for the function here
	@itemId int, @hotelId int
)
RETURNS bit
AS
BEGIN
	DECLARE @result int;

	if @itemId is null or @hotelId is null
	Begin
		SET @result = 0;
		Return @result;
	End

	

	SET @result = (SELECT COUNT(vi.Id)
	FROM [VenueItems] vi
	WHERE vi.Id = @itemId
	AND vi.VenueId IN (
		SELECT v.Id
		FROM Venues v
		WHERE v.HotelId = @hotelId
	));

	IF @result > 0
       BEGIN
		SET @result = 1
	   END
	ELSE 
       BEGIN
	   SET @result = 0
	   END

	-- Return the result of the function
	RETURN @result

END