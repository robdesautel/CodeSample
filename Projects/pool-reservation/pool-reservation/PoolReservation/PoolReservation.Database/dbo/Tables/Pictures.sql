CREATE TABLE [dbo].[Pictures]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY ROWGUIDCOL DEFAULT (newid()), 
    [OwnerId] NVARCHAR(128) NOT NULL, 
    [DateTaken] DATETIME NULL, 
    [DateUploaded] DATETIME NOT NULL, 
    [IsDeleted] BIT NOT NULL, 
    [DeletedBy] NVARCHAR(128) NULL, 
    [DateDeleted] DATETIME NULL, 
    CONSTRAINT [FK_Pictures_To_Users_Created] FOREIGN KEY ([OwnerId]) REFERENCES [AspNetUsers]([Id]), 
    CONSTRAINT [FK_Pictures_To_Users_Deleted] FOREIGN KEY ([DeletedBy]) REFERENCES [AspNetUsers]([Id])
)
