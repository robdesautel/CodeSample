CREATE TABLE [dbo].[ReserveItems]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ItemId] INT NOT NULL, 
    [Quantity] INT NOT NULL, 
    [ReservationId] INT NOT NULL, 
    [PricePreTax] DECIMAL(18, 6) NOT NULL, 
    [TaxRate] DECIMAL(9, 8) NOT NULL, 
    [IgnoreQuantityRestrictions] BIT NOT NULL DEFAULT 0, 
    [IsDeleted] BIT NOT NULL DEFAULT 0, 
    [DeletedDate] DATETIME NULL, 
    [DateReservedFor] DATETIME NOT NULL, 
    [FinalPrice] DECIMAL(18, 6) NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_ReserveItems_To_Reservations] FOREIGN KEY ([ReservationId]) REFERENCES [ReservationGroup]([Id]) ON DELETE CASCADE, 
    CONSTRAINT [FK_ReserveItems_To_Items] FOREIGN KEY ([ItemId]) REFERENCES [VenueItems]([Id])
)

GO

