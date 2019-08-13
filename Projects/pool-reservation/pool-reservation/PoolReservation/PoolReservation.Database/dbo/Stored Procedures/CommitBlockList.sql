

create procedure CommitBlockList
    @accountName varchar(24),
    @containerName varchar(63),
    @blobName nvarchar(256),
    @blocks varchar(max),
    @lastModificationTime DATETIME OUTPUT
as
begin
    BEGIN TRANSACTION
    
    DECLARE @versionTimestamp DATETIME
    SET @versionTimestamp = '9999-12-31T23:59:59.997'

    -- The ordered list of committed blocks, put in a table for joining later.
    declare @newBlocks TABLE (
        SequenceNumber INT IDENTITY,
        BlockId VARCHAR(128),
        BlockSource INT
        )
        
    -- Deserialize the blocks argument to get a list of blocks to commit.
    DECLARE @blockOffset INT
    SET @blockOffset = 1
    DECLARE @blockIdSize INT
    DECLARE @blockId VARCHAR(128)
    DECLARE @blockSource INT
    
    DECLARE @blocksLength INT
    SET @blocksLength = DATALENGTH(@blocks)
    WHILE (@blockOffset <= @blocksLength)
    BEGIN
        SET @blockIdSize = ASCII(SUBSTRING(@blocks, @blockOffset, 1)) - 32
        SET @blockOffset = @blockOffset + 1
        
        SET @blockId = SUBSTRING(@blocks, @blockOffset, @blockIdSize * 2)
        SET @blockOffset = @blockOffset + @blockIdSize * 2
        
        SET @blockSource = ASCII(SUBSTRING(@blocks, @blockOffset, 1)) - 48
        SET @blockOffset = @blockOffset + 1

        INSERT INTO @newBlocks(BlockId, BlockSource) VALUES (@blockId, @blockSource)
    END

    -- Change all blocks with a source of latest to uncommitted first
    -- and convert the remaining to committed
    UPDATE @newBlocks
    SET BlockSource = 1 -- Uncommitted
    WHERE BlockSource = 3 -- Latest
      AND BlockId in (
            SELECT old.BlockId
            FROM BlockData AS old
            WHERE old.AccountName = @accountName
                AND old.ContainerName = @containerName
                AND old.BlobName = @blobName
                AND old.VersionTimestamp = @versionTimestamp
                AND old.IsCommitted = 0)
                
    UPDATE @newBlocks
    SET BlockSource = 2 -- Committed
    WHERE BlockSource = 3
            
    -- Delete all blocks not in the new block list.
    DELETE FROM BlockData
    WHERE BlockData.AccountName = @accountName
        AND BlockData.ContainerName = @containerName
        AND BlockData.BlobName = @blobName
        AND BlockData.VersionTimestamp = @versionTimestamp
        AND NOT EXISTS (
            SELECT 1
            FROM @newBlocks AS new
            WHERE BlockData.BlockId = new.BlockId
                AND BlockData.IsCommitted = (CASE new.BlockSource WHEN 1 THEN 0 ELSE 1 END) )
        
    -- All the new blocks are going to be committed, so set the IsCommitted flag.
    UPDATE BlockData
    SET IsCommitted = 1
    WHERE AccountName = @accountName
        AND ContainerName = @containerName
        AND BlobName = @blobName
        AND VersionTimestamp = @versionTimestamp
        AND IsCommitted = 0
        AND BlockId IN (SELECT BlockId FROM @newBlocks)


    -- Now, go over each of the blocks to be committed to get the starting offset of the block.
    DECLARE newBlockCursor CURSOR FOR
        SELECT new.BlockId, old.Length
        FROM @newBlocks AS new
        LEFT OUTER JOIN (
            SELECT * FROM BlockData AS old
            WHERE old.AccountName = @accountName
                AND old.ContainerName = @containerName
                AND old.BlobName = @blobName
                AND old.VersionTimestamp = @versionTimestamp) AS old
        ON new.BlockId = old.BlockId
        ORDER BY SequenceNumber
    
    DECLARE @length BIGINT
    DECLARE @offset BIGINT
    SET @offset = 0
    
    OPEN newBlockCursor
    
    DELETE FROM CommittedBlock
    WHERE AccountName = @accountName
        AND ContainerName = @containerName
        AND BlobName = @blobName
        AND VersionTimestamp = @versionTimestamp
        
    FETCH NEXT FROM newBlockCursor INTO @blockId, @length
    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF @length IS NULL
        BEGIN
            RAISERROR('InvalidBlockList', 16, 1)
            ROLLBACK TRANSACTION
            
            RETURN
        END
        
        INSERT INTO CommittedBlock (AccountName, ContainerName, BlobName, VersionTimestamp, Offset, BlockId, Length, BlockVersion)
        VALUES (@accountName, @containerName, @blobName, @versionTimestamp, @offset, @blockId, @length, @versionTimestamp)
        
        set @offset = @offset + @length
        FETCH NEXT FROM newBlockCursor INTO @blockId, @length
    END
    
    CLOSE newBlockCursor

    -- Set this separately, since it does not update the LastModificationTime
    UPDATE Blob
    SET UncommittedBlockIdLength = NULL
    WHERE AccountName = @accountName
        AND ContainerName = @containerName
        AND BlobName = @blobName
        AND VersionTimestamp = @versionTimestamp

    UPDATE Blob
    SET IsCommitted = 1, ContentLength = @offset
    WHERE AccountName = @accountName
        AND ContainerName = @containerName
        AND BlobName = @blobName
        AND VersionTimestamp = @versionTimestamp

    SELECT @lastModificationTime = LastModificationTime
    FROM Blob
    WHERE AccountName = @accountName
        AND ContainerName = @containerName
        AND BlobName = @blobName
        AND VersionTimestamp = @versionTimestamp  
        
    COMMIT TRANSACTION  
end