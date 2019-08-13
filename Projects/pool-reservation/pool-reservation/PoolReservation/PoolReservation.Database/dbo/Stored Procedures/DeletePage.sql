

create procedure DeletePage
    @accountName varchar(24),
    @containerName varchar(63),
    @blobName nvarchar(256),
    @versionTimestamp DATETIME,
    @startOffset BIGINT,
    @endOffset BIGINT,
    @fileOffset BIGINT,
    @snapshotCount INT
as
begin
    EXEC InsertPageHelper @accountName, @containerName, @blobName, @versionTimestamp, @startOffset, @endOffset, @fileOffset, @snapshotCount, 1
end