USE [master]
GO
CREATE DATABASE [PaymentGateway]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PaymentGateway', FILENAME = N'{path}\PaymentGateway.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'PaymentGateway_log', FILENAME = N'{path}\PaymentGateway_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [PaymentGateway] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PaymentGateway].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PaymentGateway] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PaymentGateway] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PaymentGateway] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PaymentGateway] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PaymentGateway] SET ARITHABORT OFF 
GO
ALTER DATABASE [PaymentGateway] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [PaymentGateway] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PaymentGateway] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PaymentGateway] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PaymentGateway] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PaymentGateway] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PaymentGateway] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PaymentGateway] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PaymentGateway] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PaymentGateway] SET  DISABLE_BROKER 
GO
ALTER DATABASE [PaymentGateway] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PaymentGateway] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PaymentGateway] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PaymentGateway] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PaymentGateway] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PaymentGateway] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PaymentGateway] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PaymentGateway] SET RECOVERY FULL 
GO
ALTER DATABASE [PaymentGateway] SET  MULTI_USER 
GO
ALTER DATABASE [PaymentGateway] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PaymentGateway] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PaymentGateway] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PaymentGateway] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [PaymentGateway] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'PaymentGateway', N'ON'
GO
ALTER DATABASE [PaymentGateway] SET QUERY_STORE = OFF
GO
USE [PaymentGateway]
GO
CREATE USER [payment.gateway.user] FOR LOGIN [payment.gateway.user] WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE USER [NT AUTHORITY\SYSTEM] FOR LOGIN [NT AUTHORITY\SYSTEM] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [NT AUTHORITY\SYSTEM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BankPaymentResponse](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PaymentRequestId] [int] NOT NULL,
	[BankPaymentIdentifier] [uniqueidentifier] NULL,
	[Status] [nvarchar](255) NOT NULL,
	[DateTimeAdded] [datetime] NOT NULL,
 CONSTRAINT [PK_BankPaymentResponse] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Card](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Number] [nvarchar](19) NULL,
	[CVV] [nvarchar](4) NULL,
	[ExpiryDate] [date] NOT NULL,
 CONSTRAINT [PK_Card] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Currency](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](100) NOT NULL,
	[ShortDescription] [char](3) NOT NULL,
 CONSTRAINT [PK_Currency] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Merchant](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[ApiKey] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Merchant] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentRequest](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MerchantId] [int] NOT NULL,
	[CardId] [int] NOT NULL,
	[Amount] [decimal](20, 4) NOT NULL,
	[CurrencyId] [int] NOT NULL,
	[DateTimeAdded] [datetime] NOT NULL,
 CONSTRAINT [PK_PaymentRequest] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[BankPaymentResponse] ON 

INSERT [dbo].[BankPaymentResponse] ([Id], [PaymentRequestId], [BankPaymentIdentifier], [Status], [DateTimeAdded]) VALUES (2, 3, N'01ca6a6b-af59-4f8f-97f6-1087323c83a2', N'Successful', CAST(N'2020-05-24T09:08:31.923' AS DateTime))
INSERT [dbo].[BankPaymentResponse] ([Id], [PaymentRequestId], [BankPaymentIdentifier], [Status], [DateTimeAdded]) VALUES (3, 6, NULL, N'The remote server returned an error: (400) Bad Request.', CAST(N'2020-05-24T10:44:26.733' AS DateTime))
INSERT [dbo].[BankPaymentResponse] ([Id], [PaymentRequestId], [BankPaymentIdentifier], [Status], [DateTimeAdded]) VALUES (4, 6, NULL, N'The remote server returned an error: (400) Bad Request.', CAST(N'2020-05-24T10:46:11.780' AS DateTime))
INSERT [dbo].[BankPaymentResponse] ([Id], [PaymentRequestId], [BankPaymentIdentifier], [Status], [DateTimeAdded]) VALUES (5, 6, NULL, N'The remote server returned an error: (400) Bad Request.', CAST(N'2020-05-24T10:46:42.703' AS DateTime))
INSERT [dbo].[BankPaymentResponse] ([Id], [PaymentRequestId], [BankPaymentIdentifier], [Status], [DateTimeAdded]) VALUES (6, 6, NULL, N'The remote server returned an error: (400) Bad Request.', CAST(N'2020-05-24T10:52:47.763' AS DateTime))
INSERT [dbo].[BankPaymentResponse] ([Id], [PaymentRequestId], [BankPaymentIdentifier], [Status], [DateTimeAdded]) VALUES (7, 6, NULL, N'The remote server returned an error: (400) Bad Request.', CAST(N'2020-05-24T10:53:55.110' AS DateTime))
INSERT [dbo].[BankPaymentResponse] ([Id], [PaymentRequestId], [BankPaymentIdentifier], [Status], [DateTimeAdded]) VALUES (8, 6, NULL, N'The remote server returned an error: (400) Bad Request.', CAST(N'2020-05-25T17:34:32.423' AS DateTime))
SET IDENTITY_INSERT [dbo].[BankPaymentResponse] OFF
GO
SET IDENTITY_INSERT [dbo].[Card] ON 

INSERT [dbo].[Card] ([ID], [Number], [CVV], [ExpiryDate]) VALUES (1, N'1234567891234567', N'1234', CAST(N'2025-12-01' AS Date))
SET IDENTITY_INSERT [dbo].[Card] OFF
GO
SET IDENTITY_INSERT [dbo].[Currency] ON 

INSERT [dbo].[Currency] ([ID], [Description], [ShortDescription]) VALUES (1, N'United States Dollar', N'USD')
SET IDENTITY_INSERT [dbo].[Currency] OFF
GO
INSERT [dbo].[Merchant] ([ID], [Name], [Description], [ApiKey]) VALUES (1, N'Amazon', N'Online Shopping', N'f1076038-ac22-4287-8976-4d5079775f0d')
GO
SET IDENTITY_INSERT [dbo].[PaymentRequest] ON 

INSERT [dbo].[PaymentRequest] ([ID], [MerchantId], [CardId], [Amount], [CurrencyId], [DateTimeAdded]) VALUES (3, 1, 1, CAST(12345.6000 AS Decimal(20, 4)), 1, CAST(N'2020-05-24T08:21:06.660' AS DateTime))
INSERT [dbo].[PaymentRequest] ([ID], [MerchantId], [CardId], [Amount], [CurrencyId], [DateTimeAdded]) VALUES (4, 1, 1, CAST(12345.6000 AS Decimal(20, 4)), 1, CAST(N'2020-05-24T09:06:26.713' AS DateTime))
INSERT [dbo].[PaymentRequest] ([ID], [MerchantId], [CardId], [Amount], [CurrencyId], [DateTimeAdded]) VALUES (5, 1, 1, CAST(12345.6000 AS Decimal(20, 4)), 1, CAST(N'2020-05-24T09:08:31.730' AS DateTime))
INSERT [dbo].[PaymentRequest] ([ID], [MerchantId], [CardId], [Amount], [CurrencyId], [DateTimeAdded]) VALUES (6, 1, 1, CAST(12349995.6900 AS Decimal(20, 4)), 1, CAST(N'2020-05-24T10:40:57.660' AS DateTime))
INSERT [dbo].[PaymentRequest] ([ID], [MerchantId], [CardId], [Amount], [CurrencyId], [DateTimeAdded]) VALUES (7, 1, 1, CAST(12349995.6900 AS Decimal(20, 4)), 1, CAST(N'2020-05-24T10:41:17.507' AS DateTime))
INSERT [dbo].[PaymentRequest] ([ID], [MerchantId], [CardId], [Amount], [CurrencyId], [DateTimeAdded]) VALUES (8, 1, 1, CAST(12349995.6900 AS Decimal(20, 4)), 1, CAST(N'2020-05-24T10:43:53.823' AS DateTime))
INSERT [dbo].[PaymentRequest] ([ID], [MerchantId], [CardId], [Amount], [CurrencyId], [DateTimeAdded]) VALUES (9, 1, 1, CAST(12349995.6900 AS Decimal(20, 4)), 1, CAST(N'2020-05-24T10:45:34.433' AS DateTime))
INSERT [dbo].[PaymentRequest] ([ID], [MerchantId], [CardId], [Amount], [CurrencyId], [DateTimeAdded]) VALUES (10, 1, 1, CAST(12349995.6900 AS Decimal(20, 4)), 1, CAST(N'2020-05-24T10:46:36.793' AS DateTime))
INSERT [dbo].[PaymentRequest] ([ID], [MerchantId], [CardId], [Amount], [CurrencyId], [DateTimeAdded]) VALUES (11, 1, 1, CAST(12349995.6900 AS Decimal(20, 4)), 1, CAST(N'2020-05-24T10:52:36.530' AS DateTime))
INSERT [dbo].[PaymentRequest] ([ID], [MerchantId], [CardId], [Amount], [CurrencyId], [DateTimeAdded]) VALUES (12, 1, 1, CAST(12349995.6900 AS Decimal(20, 4)), 1, CAST(N'2020-05-24T10:53:31.637' AS DateTime))
INSERT [dbo].[PaymentRequest] ([ID], [MerchantId], [CardId], [Amount], [CurrencyId], [DateTimeAdded]) VALUES (13, 1, 1, CAST(12349995.6900 AS Decimal(20, 4)), 1, CAST(N'2020-05-25T17:34:22.573' AS DateTime))
SET IDENTITY_INSERT [dbo].[PaymentRequest] OFF
GO
ALTER TABLE [dbo].[BankPaymentResponse]  WITH CHECK ADD  CONSTRAINT [FK_BankPaymentResponse_PaymentRequest] FOREIGN KEY([PaymentRequestId])
REFERENCES [dbo].[PaymentRequest] ([ID])
GO
ALTER TABLE [dbo].[BankPaymentResponse] CHECK CONSTRAINT [FK_BankPaymentResponse_PaymentRequest]
GO
ALTER TABLE [dbo].[PaymentRequest]  WITH CHECK ADD  CONSTRAINT [FK_PaymentRequest_Card] FOREIGN KEY([CardId])
REFERENCES [dbo].[Card] ([ID])
GO
ALTER TABLE [dbo].[PaymentRequest] CHECK CONSTRAINT [FK_PaymentRequest_Card]
GO
ALTER TABLE [dbo].[PaymentRequest]  WITH CHECK ADD  CONSTRAINT [FK_PaymentRequest_Currency] FOREIGN KEY([CurrencyId])
REFERENCES [dbo].[Currency] ([ID])
GO
ALTER TABLE [dbo].[PaymentRequest] CHECK CONSTRAINT [FK_PaymentRequest_Currency]
GO
ALTER TABLE [dbo].[PaymentRequest]  WITH CHECK ADD  CONSTRAINT [FK_PaymentRequest_Merchant] FOREIGN KEY([MerchantId])
REFERENCES [dbo].[Merchant] ([ID])
GO
ALTER TABLE [dbo].[PaymentRequest] CHECK CONSTRAINT [FK_PaymentRequest_Merchant]
GO
USE [master]
GO
ALTER DATABASE [PaymentGateway] SET  READ_WRITE 
GO
