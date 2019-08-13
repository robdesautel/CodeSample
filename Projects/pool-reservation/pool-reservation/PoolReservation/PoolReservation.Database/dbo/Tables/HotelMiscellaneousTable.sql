CREATE TABLE [dbo].[HotelMiscellaneousTable]
(
	[HotelId] INT NOT NULL , 
    [MiscellaneousId] INT NOT NULL, 
    CONSTRAINT [PK_HotelMiscellaneousTable] PRIMARY KEY ([HotelId], [MiscellaneousId])
)
