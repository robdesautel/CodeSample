

CREATE function MatchingDelimiter(
    @value nvarchar(256),
    @prefix nvarchar(256),
    @delimiter nvarchar(256)
    )
    returns nvarchar(256)
as
begin
    DECLARE @prefixLength INT
    SET @prefixLength = 0
    IF (@prefix IS NOT NULL)
    BEGIN
        SET @prefixLength = len(@prefix)
    END
    
    if ((@prefix IS NULL) OR (left(@value, @prefixLength) = @prefix))
    begin
        declare @index int
        set @index = charindex(@delimiter, @value, @prefixLength + 1)
        if (@index > 0)
        begin
            return left(@value, @index + len(@delimiter) - 1)
        end
    end
    return @value
end