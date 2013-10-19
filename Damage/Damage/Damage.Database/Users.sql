CREATE TABLE [dbo].[Users]
(
	[UserId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Username] NVARCHAR(50) NOT NULL, 
    [Password] NVARCHAR(MAX) NOT NULL, 
    [Salt] NVARCHAR(MAX) NOT NULL
)

GO

CREATE INDEX [IX_Users_Username] ON [dbo].[Users] ([Username])
