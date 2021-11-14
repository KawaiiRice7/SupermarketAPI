﻿IF db_id('SupermarketAPI') IS NULL
	CREATE DATABASE SupermarketAPI;
GO

USE SupermarketAPI

IF object_id('Category', 'U') IS NULL
BEGIN
	CREATE TABLE [dbo].[Category]
	(
		[Id] [nvarchar](36) NOT NULL,
		[Name] [nvarchar](200) NOT NULL,
		CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	)
END

IF NOT EXISTS (SELECT * FROM [dbo].[Category])
BEGIN
	INSERT INTO [dbo].[Category] VALUES 
	('548F1D4C-2B35-4028-9410-533D84995EEC', 'Flower'), 
	('08E717DC-22B4-4B6F-9E54-888C2C1AD574', 'Chips'), 
	('F15B1C78-1E1F-4D4E-982C-D7F64EE4A33D', 'Ice Cream')
END

IF object_id('Product', 'U') IS NULL
BEGIN
CREATE TABLE [dbo].[Product](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Cost] [decimal](18, 2) NOT NULL,
	[CategoryId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END

IF NOT EXISTS(SELECT * FROM [dbo].[Product])
BEGIN
	INSERT INTO [dbo].[Product] VALUES
	('A3E5C8D8-2685-4946-9B76-E1AC7242876C', 'Rose', 1.23, '548F1D4C-2B35-4028-9410-533D84995EEC'),
	('4187B144-54D5-4B53-9791-490AFD512ED1', 'Lays Classic', 5.99, '08E717DC-22B4-4B6F-9E54-888C2C1AD574'),
	('4CFDC88F-6F1E-4F1B-8C5A-9BED96A2A7E5', 'Lays BBQ', 4.56, '08E717DC-22B4-4B6F-9E54-888C2C1AD574'),
	('D437580A-7931-4EB0-A0D1-FCBE4FD17B88', 'Vanilla', 4.99, 'F15B1C78-1E1F-4D4E-982C-D7F64EE4A33D'),
	('F1C0CB01-7BD6-4F0D-B601-77752FC00237', 'Napolean', 4.99, 'F15B1C78-1E1F-4D4E-982C-D7F64EE4A33D'),
	('6D1E42B3-72E7-4AF3-88AA-F9A0A01AECF3', 'Coffee', 5.12, 'F15B1C78-1E1F-4D4E-982C-D7F64EE4A33D')
END