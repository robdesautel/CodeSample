CREATE TABLE [dbo].[Calendar]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [VenueId] INT NOT NULL, 
    [StartDate] DATETIME NOT NULL, 
    [EndDate] DATETIME NOT NULL, 
    CONSTRAINT [FK_Calendar_To_Venue] FOREIGN KEY ([VenueId]) REFERENCES [Venues]([Id])
)
