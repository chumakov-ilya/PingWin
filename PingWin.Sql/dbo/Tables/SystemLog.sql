CREATE TABLE [dbo].[SystemLog] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [DateTime]   DATETIME       NOT NULL,
    [Message]    NVARCHAR (256) NULL,
    [FullData]   NVARCHAR (MAX) NULL,
    [StackTrace] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_SystemLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

