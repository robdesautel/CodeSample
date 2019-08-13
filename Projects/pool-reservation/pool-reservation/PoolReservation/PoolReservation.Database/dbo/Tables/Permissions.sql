CREATE TABLE [dbo].[Permissions]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Add] BIT NOT NULL, 
    [Delete] BIT NOT NULL, 
    [Edit] BIT NOT NULL, 
    [View] BIT NOT NULL
)
