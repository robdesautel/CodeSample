CREATE TABLE [dbo].[HotelUsers]
(
	[PermissionId] INT NOT NULL, 
    [UserId] NVARCHAR(128) NOT NULL, 
    [HotelId] INT NOT NULL, 
    CONSTRAINT [FK_HotelUsers_To_AspUsers] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers]([Id]), 
    CONSTRAINT [FK_HotelUsers_To_HotelId] FOREIGN KEY ([HotelId]) REFERENCES [Hotels]([Id]), 
    CONSTRAINT [PK_HotelUsers] PRIMARY KEY ([UserId], [HotelId]), 
    CONSTRAINT [FK_HotelUsers_To_Permissions] FOREIGN KEY ([PermissionId]) REFERENCES [HotelPermissions]([Id])
)
