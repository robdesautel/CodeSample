CREATE TABLE [dbo].[InboxMessages]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [DateSent] DATETIME NOT NULL, 
    [MiscellaneousId] INT NOT NULL, 
	[UserForId] NVARCHAR(128) NOT NULL,
    [UserSentById] NVARCHAR(128) NOT NULL , 
    CONSTRAINT [FK_InboxMessages_To_Miscellaneous] FOREIGN KEY ([MiscellaneousId]) REFERENCES [MiscellaneousHtmlTable]([Id]), 
    CONSTRAINT [FK_InboxMessages_To_Users] FOREIGN KEY ([UserForId]) REFERENCES [AspNetUsers]([Id]), 
    CONSTRAINT [FK_InboxMessages_ToTable] FOREIGN KEY ([UserSentById]) REFERENCES [AspNetUsers]([Id])
)
