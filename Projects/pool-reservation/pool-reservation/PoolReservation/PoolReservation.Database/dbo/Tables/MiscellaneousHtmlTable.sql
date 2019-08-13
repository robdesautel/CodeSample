CREATE TABLE [dbo].[MiscellaneousHtmlTable]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PageName] NVARCHAR(50) NOT NULL, 
    [PageData] NVARCHAR(MAX) NOT NULL, 
    [DateCreated] DATETIME NOT NULL
)
