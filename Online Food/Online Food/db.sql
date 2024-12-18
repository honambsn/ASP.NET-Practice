use [C:\USERS\HONAM\DOCUMENTS\ONLINE FOOD.MDF]

create table [dbo].[tblUser]
(
	[UserID] int identity(1,1) primary key not null,
	[Name] [varchar](50) null,
	[UserName] varchar(50) null unique,
	[Mobile] varchar(50) null,
	[Email] varchar(50) null unique,
	[Address] varchar(max) null,
	[PostCode] varchar(50) null,
	[Password] varchar(50) null,
	[ImageUrl] varchar(50) null,
	[CreatedDate] datetime null
)


create table [dbo].[tblContact]
(
	[ContactID] int identity(1,1) primary key not null,
	[Name] [varchar](50) null,
	[Email] varchar(50) null,
	[Subject] varchar(200) null,
	[Message] varchar(max) null,
	[CreatedDate] datetime null
)

create table [dbo].[tblCategory]
(
	[CategoryID] int identity(1,1) primary key not null,
	[CategoryName] varchar(50) null,
	[ImageUrl] varchar(50) null,
	[IsActive] [bit] null,
	[CreatedDate] datetime null
)

create table [dbo].[tblProduct]
(
	[ProductID] int identity(1,1) primary key not null,
	[ProductName] varchar(50) null,
	[Description] varchar(max) null,
	[Price] decimal(18,2) null,
	[Quantity] int null,
	[ImageUrl] varchar(max) null,
	[CategoryID] int null,
	[IsActive] [bit] null,
	[CreatedDate] datetime null
)


create table [dbo].[tblOrder]
(
	[OrderDetailsID] int identity(1,1) primary key not null,
	[OrderNo] varchar(max) null,
	[ProductID] int null,
	[Quantity] int null,
	[UserID] int null,
	[Status] varchar(50) null,
	[PaymentID] int null,
	[OrderDate] datetime null
)


create table [dbo].[tblPayment]
(
	[PaymentID] int identity(1,1) primary key not null,
	[Name] varchar(50) null,
	[CardNo] varchar(50) null,
	[ExpiryDate] varchar(50) null,
	[CvvNo] int null,
	[Address] varchar(max) null,
	[PaymentMode] varchar(50) null,
)

create table [dbo].[Cart]
(
	[CartID] int identity(1,1) primary key not null,
	[ProductID] int null,
	[Quantity] int null,
	[UserID] int null
)

select * from tblUser
select * from tblContact
select * from tblCategory
select * from tblProduct
select * from tblOrder
select * from tblPayment
