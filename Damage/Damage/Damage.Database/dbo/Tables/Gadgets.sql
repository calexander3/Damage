CREATE TABLE [dbo].[Gadgets]
(
	[GadgetId] INT NOT NULL PRIMARY KEY IDENTITY, 
	[GadgetName] NVARCHAR(50) NOT NULL, 
	[GadgetVersion] NVARCHAR(10) NOT NULL, 
	[AssemblyPresent] BIT NOT NULL 
)
