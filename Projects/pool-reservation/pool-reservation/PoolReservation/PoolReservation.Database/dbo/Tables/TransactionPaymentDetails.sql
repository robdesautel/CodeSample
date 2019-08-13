CREATE TABLE [dbo].[TransactionPaymentDetails]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [StripeId] INT NULL, 
    CONSTRAINT [FK_TransactionPaymentDetails_ToTable] FOREIGN KEY ([StripeId]) REFERENCES [TransactionStripeDetails]([Id]), 
    CONSTRAINT [CK_TransactionPaymentDetails_Column] CHECK ([StripeId] is not null) /* add more to ensure that only one payment type is chosen. */
)
