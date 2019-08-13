CREATE TABLE [dbo].[Page] (
    [AccountName]      VARCHAR (24)   NOT NULL,
    [ContainerName]    VARCHAR (63)   NOT NULL,
    [BlobName]         NVARCHAR (256) COLLATE Latin1_General_BIN NOT NULL,
    [VersionTimestamp] DATETIME       NOT NULL,
    [StartOffset]      BIGINT         NOT NULL,
    [EndOffset]        BIGINT         NULL,
    [FileOffset]       BIGINT         NULL,
    [SnapshotCount]    INT            NOT NULL,
    CONSTRAINT [PK_dbo.Page] PRIMARY KEY CLUSTERED ([AccountName] ASC, [ContainerName] ASC, [BlobName] ASC, [VersionTimestamp] ASC, [StartOffset] ASC),
    CONSTRAINT [PageBlob_Page] FOREIGN KEY ([AccountName], [ContainerName], [BlobName], [VersionTimestamp]) REFERENCES [dbo].[Blob] ([AccountName], [ContainerName], [BlobName], [VersionTimestamp]) ON DELETE CASCADE
);

