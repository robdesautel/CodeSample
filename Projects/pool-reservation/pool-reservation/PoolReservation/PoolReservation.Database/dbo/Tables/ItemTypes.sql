CREATE TABLE [dbo].[ItemTypes] (
    [Id]             INT NOT NULL IDENTITY,
    [Name] NVARCHAR(50) NOT NULL, 
    [VenueTypeId] INT NOT NULL , 
    [Price] DECIMAL(18, 6) NOT NULL , 
    CONSTRAINT [PK__VenueIte__3214EC076DD8F69B] PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_ItemTypes_ToVenueType] FOREIGN KEY ([VenueTypeId]) REFERENCES [VenueTypes]([Id])
);

