CREATE TABLE [dbo].[SitePermissions]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Hotel] INT NOT NULL, 
    [Price] INT NOT NULL, 
    [OtherReservations] INT NOT NULL, 
    [Icon] INT NOT NULL, 
    [PersonalReservation] INT NOT NULL, 
    [Users] INT NOT NULL DEFAULT 4, 
    CONSTRAINT [FK_SitePermissions_Hotel_To_Permissions] FOREIGN KEY ([Hotel]) REFERENCES [Permissions]([Id]),
	CONSTRAINT [FK_SitePermissions_Price_To_Permissions] FOREIGN KEY ([Price]) REFERENCES [Permissions]([Id]),
	CONSTRAINT [FK_SitePermissions_OtherReservations_To_Permissions] FOREIGN KEY ([OtherReservations]) REFERENCES [Permissions]([Id]),
	CONSTRAINT [FK_SitePermissions_Icon_To_Permissions] FOREIGN KEY ([Icon]) REFERENCES [Permissions]([Id]),
	CONSTRAINT [FK_SitePermissions_PersonalReservation_To_Permissions] FOREIGN KEY ([PersonalReservation]) REFERENCES [Permissions]([Id]), 
    CONSTRAINT [FK_SitePermissions_Users_To_Permissions] FOREIGN KEY ([Users]) REFERENCES [Permissions]([Id])
)
