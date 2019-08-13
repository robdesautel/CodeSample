CREATE TABLE [dbo].[Icons]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PictureId] UNIQUEIDENTIFIER NOT NULL, 
    [IsDeleted] BIT NOT NULL, 
    [DeletedDate] DATETIME NULL, 
    CONSTRAINT [FK_Icons_ToTable] FOREIGN KEY ([PictureId]) REFERENCES [Pictures]([Id]) On delete cascade on update cascade
)
