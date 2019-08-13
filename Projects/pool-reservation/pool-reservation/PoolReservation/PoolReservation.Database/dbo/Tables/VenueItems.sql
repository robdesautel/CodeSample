CREATE TABLE [dbo].[VenueItems]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [VenueId] INT NOT NULL, 
    [Type] INT NOT NULL, 
    [Name] NVARCHAR(128) NOT NULL, 
    [Price] DECIMAL(18, 6) NOT NULL DEFAULT 1.00, 
    [IsDeleted] BIT NOT NULL DEFAULT 0, 
    [IconId] INT NOT NULL DEFAULT 2, 
    [IsHidden] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_VenueItems_To_Venue] FOREIGN KEY ([VenueId]) REFERENCES [Venues]([Id]), 
    CONSTRAINT [FK_VenueItems_To_Types] FOREIGN KEY ([Type]) REFERENCES [ItemTypes]([Id]), 
    CONSTRAINT [AK_VenueItems_Column] UNIQUE ([Id], [VenueId]), 
    CONSTRAINT [CK_VenueItems_Column] CHECK ([dbo].[GetVenueTypeFromVenue]([VenueId]) = [dbo].[GetVenueTypeFromItemType]([Type])), 
    CONSTRAINT [FK_VenueItems_ToTable] FOREIGN KEY ([IconId]) REFERENCES [Icons]([Id]) 
)
