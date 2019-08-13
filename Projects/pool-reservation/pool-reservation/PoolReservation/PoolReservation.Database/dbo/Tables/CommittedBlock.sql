CREATE TABLE [dbo].[CommittedBlock] (
    [AccountName]      VARCHAR (24)   NOT NULL,
    [ContainerName]    VARCHAR (63)   NOT NULL,
    [BlobName]         NVARCHAR (256) COLLATE Latin1_General_BIN NOT NULL,
    [VersionTimestamp] DATETIME       NOT NULL,
    [Offset]           BIGINT         NOT NULL,
    [BlockId]          VARCHAR (128)  NOT NULL,
    [Length]           BIGINT         NULL,
    [BlockVersion]     DATETIME       NULL,
    CONSTRAINT [PK_dbo.CommittedBlock] PRIMARY KEY CLUSTERED ([AccountName] ASC, [ContainerName] ASC, [BlobName] ASC, [VersionTimestamp] ASC, [Offset] ASC),
    CONSTRAINT [BlockBlob_CommittedBlock] FOREIGN KEY ([AccountName], [ContainerName], [BlobName], [VersionTimestamp]) REFERENCES [dbo].[Blob] ([AccountName], [ContainerName], [BlobName], [VersionTimestamp]) ON DELETE CASCADE
);

