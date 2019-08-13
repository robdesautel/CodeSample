CREATE TABLE [dbo].[Venues] (
    [Id]           INT NOT NULL IDENTITY,
    [HotelId]     INT NOT NULL,
    [Name]    NVARCHAR (50) NOT NULL,
    [Type]     INT NOT NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0, 
    [StartupMessage] INT NULL, 
    [IsHidden] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_Venues] PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_Venues_To_Hotel] FOREIGN KEY ([HotelId]) REFERENCES [Hotels]([Id]), 
    CONSTRAINT [FK_Venues_ToTable] FOREIGN KEY ([Type]) REFERENCES [VenueTypes]([Id]), 
    CONSTRAINT [AK_Venues_Column] UNIQUE ([Id], [HotelId]), 
    CONSTRAINT [FK_Venues_ToTable_1] FOREIGN KEY ([StartupMessage]) REFERENCES [MiscellaneousHtmlTable]([Id])
);

