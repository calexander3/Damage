CREATE TABLE [dbo].[Users] (
	[UserId]   INT           IDENTITY (1, 1) NOT NULL,
	[UserName] NVARCHAR (56) NOT NULL,
	[CurrentOAuthAccessToken] NVARCHAR(100) NOT NULL DEFAULT '', 
	[OAuthAccessTokenExpiration] DATETIME2 NOT NULL DEFAULT '1/1/1900', 
	[EmailAddress] NVARCHAR(100) NOT NULL DEFAULT '', 
	[LastLoginTime] DATETIME2 NOT NULL DEFAULT getDate(), 
	[LayoutId] INT NOT NULL DEFAULT 0, 
	PRIMARY KEY CLUSTERED ([UserId] ASC),
	UNIQUE NONCLUSTERED ([UserName] ASC)
);

