

create procedure DeleteSnapshots
    @accountName varchar(24),
    @containerName varchar(63),
    @blobName nvarchar(256),
    @isDeletingOnlySnapshots BIT,
    @requiresNoSnapshots BIT
as
begin
    DECLARE @versionTimestamp DATETIME
    SET @versionTimestamp = '9999-12-31T23:59:59.997'

    IF ( (@requiresNoSnapshots = 1)
        AND EXISTS (
            SELECT 1 FROM dbo.Blob
            WHERE AccountName = @accountName
              AND ContainerName = @containerName
              AND BlobName = @blobName
              AND VersionTimestamp < @versionTimestamp) )
    BEGIN
        RAISERROR('BlobHasSnapshots', 16, 1)
        RETURN
    END

    IF (@isDeletingOnlySnapshots = 1)
    BEGIN
        DELETE FROM dbo.Blob
            WHERE AccountName = @accountName
              AND ContainerName = @containerName
              AND BlobName = @blobName
              AND VersionTimestamp < @versionTimestamp
              
        IF (@@ROWCOUNT = 0)
        BEGIN
            RAISERROR('BlobHasNoSnapshots', 16, 1)
        END
    END
    ELSE
    BEGIN
        DELETE FROM dbo.Blob
            WHERE AccountName = @accountName
              AND ContainerName = @containerName
              AND BlobName = @blobName
    END
end