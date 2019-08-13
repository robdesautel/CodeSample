CREATE TABLE [dbo].[PrivacyPolicy]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [DateEffective] DATETIME NOT NULL, 
    [MiscellaneousId] INT NOT NULL, 
    CONSTRAINT [FK_PrivacyPolicy_To_Miscellaneous] FOREIGN KEY ([MiscellaneousId]) REFERENCES [MiscellaneousHtmlTable]([Id])
)
