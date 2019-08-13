CREATE TABLE [dbo].[Hotels] (
    [Id]                INT NOT NULL Identity,
    [Name]              NVARCHAR (128) NOT NULL,
    [Address] NVARCHAR(1024) NOT NULL, 
    [Latitude] FLOAT NOT NULL, 
    [Longitude] FLOAT NOT NULL, 
    [TaxRate] DECIMAL(9, 8) NOT NULL DEFAULT 0, 
    [IsDeleted] BIT NOT NULL DEFAULT 0, 
    [PictureId] UNIQUEIDENTIFIER NULL, 
    [IsHidden] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK__Hotels__3214EC07C3067840] PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_Hotels_ToTable] FOREIGN KEY ([PictureId]) REFERENCES [Pictures]([Id]),
);

