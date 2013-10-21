CREATE TABLE [dbo].[UserGadgets]
(
	[UserGadgetId] INT NOT NULL PRIMARY KEY IDENTITY, 
	[UserId] INT NOT NULL, 
	[GadgetId] INT NOT NULL,
	[GadgetSettings] NVARCHAR(MAX) NOT NULL, 
	[DisplayColumn] INT NOT NULL DEFAULT 1, 
	[DisplayOrdinal] INT NOT NULL DEFAULT 0, 
	CONSTRAINT [FK_UserGadgets_Gadgets] FOREIGN KEY ([GadgetId]) REFERENCES [Gadgets]([GadgetId]), 
	CONSTRAINT [FK_UserGadgets_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([UserId])
)

GO

CREATE INDEX [IX_UserGadgets_UserId] ON [dbo].[UserGadgets] ([UserId])
