CREATE TABLE [dbo].[Messages] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [Content]        NVARCHAR (MAX)   NOT NULL,
    [SendDate]       DATETIME2 (7)    NOT NULL,
    [UserId]         NVARCHAR (450)   NOT NULL,
    [ReceiverUserId] NVARCHAR (MAX)   DEFAULT (N'') NOT NULL,
    CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Messages_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Messages_UserId]
    ON [dbo].[Messages]([UserId] ASC);

