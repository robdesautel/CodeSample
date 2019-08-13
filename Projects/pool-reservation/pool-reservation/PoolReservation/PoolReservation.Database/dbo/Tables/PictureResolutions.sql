CREATE TABLE [dbo].[PictureResolutions]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PictureId] UNIQUEIDENTIFIER NOT NULL, 
    [Width] INT NOT NULL, 
    [Height] INT NOT NULL, 
    [Size] INT NOT NULL, 
    [FileName] NVARCHAR(128) NOT NULL, 
    [FileUrlId] INT NOT NULL, 
    CONSTRAINT [FK_PictureResolutions_To_Picture] FOREIGN KEY ([PictureId]) REFERENCES [Pictures]([Id]), 
    CONSTRAINT [FK_PictureResolutions_To_Urls] FOREIGN KEY ([FileUrlId]) REFERENCES [PictureUrls]([Id])
)
