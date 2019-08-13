CREATE TABLE [dbo].[TransactionStripeDetails]
(
	[Id] INT NOT NULL PRIMARY KEY identity, 
    [TokenId] NVARCHAR(MAX) NOT NULL, 
    [ChargeId] NVARCHAR(MAX) NULL
)
