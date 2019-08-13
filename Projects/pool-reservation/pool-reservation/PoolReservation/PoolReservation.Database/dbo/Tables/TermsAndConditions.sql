CREATE TABLE [dbo].[TermsAndConditions]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [DateEffective] DATETIME NOT NULL, 
    [MiscellaneousId] INT NOT NULL, 
    CONSTRAINT [FK_TermsAndConditions_To_Miscellaneous] FOREIGN KEY ([MiscellaneousId]) REFERENCES [MiscellaneousHtmlTable]([Id])
)
