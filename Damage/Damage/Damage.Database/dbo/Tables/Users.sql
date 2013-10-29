CREATE TABLE [dbo].[Users] (
    [UserId]   INT           IDENTITY (1, 1) NOT NULL,
    [UserName] NVARCHAR (56) NOT NULL,
    [CurrentOAuthAccessToken] NVARCHAR(100) NOT NULL DEFAULT '', 
    [EmailAddress] NVARCHAR(100) NOT NULL DEFAULT '', 
    PRIMARY KEY CLUSTERED ([UserId] ASC),
    UNIQUE NONCLUSTERED ([UserName] ASC)
);

