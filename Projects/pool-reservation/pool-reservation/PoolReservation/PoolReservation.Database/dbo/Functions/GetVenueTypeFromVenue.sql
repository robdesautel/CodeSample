CREATE FUNCTION [dbo].[GetVenueTypeFromVenue]
(
	@VenueId int
)
RETURNS INT
AS
BEGIN
	DECLARE @VenueTypeId int;
	
	if @VenueId is null
	Begin
		Return 0;
	End

	

	SET @VenueTypeId = (SELECT TOP(1) v.[Type]
	FROM Venues v
	WHERE v.Id = @VenueId);

	
	RETURN @VenueTypeId
END