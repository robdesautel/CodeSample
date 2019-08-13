

CREATE function ReplaceStringOnlyIfNotNullOrEmpty(
    @baseString nvarchar(260),
    @pattern nvarchar(260),
    @replaceString nvarchar(260)
)
    returns nvarchar(260)
AS
BEGIN

DECLARE @retStr as nvarchar(260)
SET @retStr = @baseString

IF (@pattern IS NOT NULL
    AND @pattern != ''
    AND @replaceString IS NOT NULL
    AND @replaceString != ''
    )
BEGIN
    SET @retStr = REPLACE( @baseString, @pattern,  @replaceString)
END

return @retStr

END