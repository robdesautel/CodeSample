CREATE TABLE [dbo].[ReservationGroup]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] NVARCHAR(128) NOT NULL, 
    [HotelId] INT NOT NULL, 
    [DateCreated] DATETIME NOT NULL, 
    [StatusId] INT NOT NULL, 
    [StatusDate] DATETIME NULL, 
    [StatusGuid] UNIQUEIDENTIFIER NULL, 
    CONSTRAINT [FK_ReservationGroup_To_Users] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers]([Id]), 
    CONSTRAINT [FK_ReservationGroup_To_Hotels] FOREIGN KEY ([HotelId]) REFERENCES [Hotels]([Id]), 
    CONSTRAINT [FK_ReservationGroup_ToTable] FOREIGN KEY ([StatusId]) REFERENCES [ReservationGroupStatus]([Id])
)
