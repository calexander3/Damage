CREATE TABLE [dbo].[Gadgets]
(
	[GadgetId] INT NOT NULL PRIMARY KEY IDENTITY, 
	[GadgetName] NVARCHAR(50) NOT NULL,
	[GadgetTitle] NVARCHAR(50) NOT NULL,
	[GadgetDescription] NVARCHAR(MAX) NOT NULL ,   
	[GadgetVersion] NVARCHAR(10) NOT NULL, 
	[InBeta] BIT NOT NULL, 
	[AssemblyPresent] BIT NOT NULL, 
	[DefaultSettings] NVARCHAR(MAX) NOT NULL, 
	[SettingsSchema] NVARCHAR(MAX) NOT NULL
)
