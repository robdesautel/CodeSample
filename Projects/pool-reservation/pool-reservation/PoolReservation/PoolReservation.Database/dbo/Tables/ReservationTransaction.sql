CREATE TABLE [dbo].[ReservationTransaction] (
    [Id]                           UNIQUEIDENTIFIER ROWGUIDCOL NOT NULL DEFAULT (newid()),
    [UserTransactionIsForId]       NVARCHAR (128)   NOT NULL,
    [UserTransactionCompletedById] NVARCHAR (128)   NOT NULL,
    [DateCreated]                  DATETIME         NOT NULL,
    [DateCompleted]                DATETIME         NOT NULL,
    [TransactionType]              INT              NOT NULL,
    [TransactionPaymentDetailsId] INT NOT NULL, 
    [AmountCharged] DECIMAL(18, 2) NOT NULL, 
    [TransactionStatusId] INT NOT NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReservationTransaction_To_Type] FOREIGN KEY ([TransactionType]) REFERENCES [dbo].[TransactionType] ([Id]),
    CONSTRAINT [FK_ReservationTransaction_To_UsersBy] FOREIGN KEY ([UserTransactionCompletedById]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_ReservationTransaction_To_UsersFor] FOREIGN KEY ([UserTransactionIsForId]) REFERENCES [dbo].[AspNetUsers] ([Id]), 
    CONSTRAINT [FK_ReservationTransaction_ToTable] FOREIGN KEY ([TransactionPaymentDetailsId]) REFERENCES [TransactionPaymentDetails]([Id]), 
    CONSTRAINT [FK_ReservationTransaction_To_Status] FOREIGN KEY ([TransactionStatusId]) REFERENCES [TransactionStatus]([Id]) 
);


