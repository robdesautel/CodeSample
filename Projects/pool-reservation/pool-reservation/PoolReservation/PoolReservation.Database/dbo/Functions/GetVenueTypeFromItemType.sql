CREATE FUNCTION [dbo].[GetVenueTypeFromItemType]
(
	@ItemTypeId int
)
RETURNS INT
AS
BEGIN
	DECLARE @VenueTypeId int;
	
	if @ItemTypeId is null
	Begin
		Return 0;
	End

	

	SET @VenueTypeId = (SELECT TOP(1) it.VenueTypeId
	FROM ItemTypes it
	WHERE it.Id = @ItemTypeId);

	
	RETURN @VenueTypeId
END
