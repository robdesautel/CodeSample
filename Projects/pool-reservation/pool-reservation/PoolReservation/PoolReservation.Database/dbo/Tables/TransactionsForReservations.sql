CREATE TABLE [dbo].[TransactionsForReservations]
(
	[ReservationGroupId] INT NOT NULL, 
    [TransactionId] UNIQUEIDENTIFIER NOT NULL, 
    [DateLinked] DATETIME NOT NULL, 
    CONSTRAINT [PK_TransactionsForReservations] PRIMARY KEY ([ReservationGroupId], [TransactionId]), 
    CONSTRAINT [FK_TransactionsForReservations_To_Reservations] FOREIGN KEY ([ReservationGroupId]) REFERENCES [ReservationGroup]([Id]),
	CONSTRAINT [FK_TransactionsForReservations_To_Transactions] FOREIGN KEY ([TransactionId]) REFERENCES [ReservationTransaction]([Id])
)
