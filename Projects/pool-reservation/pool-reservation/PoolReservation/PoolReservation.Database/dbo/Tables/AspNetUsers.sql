CREATE TABLE [dbo].[AspNetUsers] (
    [Id]                   NVARCHAR (128) NOT NULL,
    [FirstName]                 NVARCHAR (50)  NOT NULL,
    [Email]                NVARCHAR (256) NOT NULL,
    [EmailConfirmed]       BIT            NOT NULL,
    [PasswordHash]         NVARCHAR (MAX) NULL,
    [SecurityStamp]        NVARCHAR (MAX) NULL,
    [PhoneNumber]          NVARCHAR (MAX) NULL,
    [PhoneNumberConfirmed] BIT            NOT NULL,
    [TwoFactorEnabled]     BIT            NOT NULL,
    [LockoutEndDateUtc]    DATETIME       NULL,
    [LockoutEnabled]       BIT            NOT NULL,
    [AccessFailedCount]    INT            NOT NULL,
    [UserName]             NVARCHAR (256) NOT NULL,
    [DateCreated] DATETIME NOT NULL DEFAULT GETDATE(), 
    [SitePermissionsId] INT NOT NULL, 
    [ProfilePictureId] UNIQUEIDENTIFIER NULL, 
    [LastName] NVARCHAR(50) NOT NULL DEFAULT 'EmptyLastName', 
    CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_AspNetUsers_ToTable] FOREIGN KEY ([SitePermissionsId]) REFERENCES [SitePermissions]([Id]), 
    CONSTRAINT [FK_AspNetUsers_To_Pictures] FOREIGN KEY ([ProfilePictureId]) REFERENCES [Pictures]([Id])
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex]
    ON [dbo].[AspNetUsers]([UserName] ASC);

