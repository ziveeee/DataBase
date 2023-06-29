CREATE TABLE [dbo].[Test]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NULL, 
    [Surname] NVARCHAR(50) NULL, 
    [Birthday] DATE NULL, 
    [Birthplace] NVARCHAR(50) NULL, 
    [Phone] NVARCHAR(50) NULL, 
    [Email] NVARCHAR(50) NULL
)
