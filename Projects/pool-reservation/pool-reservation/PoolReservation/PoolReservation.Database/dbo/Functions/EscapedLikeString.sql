

CREATE function EscapedLikeString(
    @prefix nvarchar(256)
    )
    returns nvarchar(256)
as
begin
    IF (@prefix IS NULL)
    BEGIN
        RETURN @prefix
    END

    DECLARE @prefixLength INT
    DECLARE @escapedLikeString NVARCHAR(256)
    SET @escapedLikeString = N''

    SET @prefixLength = LEN(@prefix)
    DECLARE @i INT;
    SET @i = 0

    DECLARE @c NVARCHAR

    WHILE (@i < @prefixLength)
    begin
        SET @c = SUBSTRING(@prefix, @i + 1, 1)

        IF (@c = '%' or @c = '_' or @c = '[')
        BEGIN
            SET @escapedLikeString = @escapedLikeString +'[' + @c + ']'
        END
        ELSE
        BEGIN
            SET @escapedLikeString = @escapedLikeString + @c
        END

        SET @i = @i + 1
    end

    return @escapedLikeString
end