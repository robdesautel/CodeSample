CREATE TABLE [dbo].[HotelPermissions]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Items] INT NOT NULL, 
    [PersonalReservations] INT NOT NULL, 
    [OtherReservations] INT NOT NULL, 
    [Hotel] INT NOT NULL , 
    [Users] INT NOT NULL DEFAULT 4, 
    CONSTRAINT [FK_HotelPermissions_Items_To_Permissions] FOREIGN KEY ([Items]) REFERENCES [Permissions]([Id]),
	CONSTRAINT [FK_HotelPermissions_Users_To_Permissions] FOREIGN KEY ([Users]) REFERENCES [Permissions]([Id]),
	CONSTRAINT [FK_HotelPermissions_PersonalReservations_To_Permissions] FOREIGN KEY ([PersonalReservations]) REFERENCES [Permissions]([Id]),
	CONSTRAINT [FK_HotelPermissions_OtherReservations_To_Permissions] FOREIGN KEY ([OtherReservations]) REFERENCES [Permissions]([Id]), 
    CONSTRAINT [FK_HotelPermissions_Hotel_To_Permissions] FOREIGN KEY ([Hotel]) REFERENCES [Permissions]([Id])
)
