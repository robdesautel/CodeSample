CREATE TABLE [dbo].[VenueItemQuantity]
(
	[ItemId] INT NOT NULL , 
    [DateEffective] DATETIME NOT NULL, 
    [Quantity] INT NOT NULL, 
    [Id] INT NOT NULL IDENTITY, 
    CONSTRAINT [FK_VenueItemQuantity_To_VenueItems] FOREIGN KEY ([ItemId]) REFERENCES [VenueItems]([Id]), 
    PRIMARY KEY ([Id])
)
