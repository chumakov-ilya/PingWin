CREATE TABLE [dbo].[Log] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Status]      INT            NOT NULL,
    [ShortData]   NVARCHAR (256) NULL,
    [FullData]    NVARCHAR (MAX) NULL,
    [StackTrace]  NVARCHAR (MAX) NULL,
    [JobRecordId] INT            NOT NULL,
    [DateTime]    DATETIME       NOT NULL,
    CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Log_JobRecord] FOREIGN KEY ([JobRecordId]) REFERENCES [dbo].[JobRecord] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Status_Job_Date]
    ON [dbo].[Log]([Status] ASC, [JobRecordId] ASC, [DateTime] ASC);

