USE [master]
GO
/****** Object:  Database [NotesMarketPlace]    Script Date: 10-04-2021 22:28:06 ******/
CREATE DATABASE [NotesMarketPlace]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'NotesMarketPlace', FILENAME = N'F:\Microsoft sql server 2019\MSSQL15.MSSQLSERVER\MSSQL\DATA\NotesMarketPlace.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'NotesMarketPlace_log', FILENAME = N'F:\Microsoft sql server 2019\MSSQL15.MSSQLSERVER\MSSQL\DATA\NotesMarketPlace_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [NotesMarketPlace] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [NotesMarketPlace].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [NotesMarketPlace] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET ARITHABORT OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [NotesMarketPlace] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [NotesMarketPlace] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET  DISABLE_BROKER 
GO
ALTER DATABASE [NotesMarketPlace] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [NotesMarketPlace] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET RECOVERY FULL 
GO
ALTER DATABASE [NotesMarketPlace] SET  MULTI_USER 
GO
ALTER DATABASE [NotesMarketPlace] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [NotesMarketPlace] SET DB_CHAINING OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [NotesMarketPlace] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [NotesMarketPlace] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [NotesMarketPlace] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'NotesMarketPlace', N'ON'
GO
ALTER DATABASE [NotesMarketPlace] SET QUERY_STORE = OFF
GO
USE [NotesMarketPlace]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 10-04-2021 22:28:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [varchar](100) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContactUs]    Script Date: 10-04-2021 22:28:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContactUs](
	[ContactID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [varchar](50) NOT NULL,
	[EmailAddress] [varchar](50) NOT NULL,
	[Subject] [varchar](100) NOT NULL,
	[Comments] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_ContactUs] PRIMARY KEY CLUSTERED 
(
	[ContactID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Country]    Script Date: 10-04-2021 22:28:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Country](
	[CountryID] [int] IDENTITY(1,1) NOT NULL,
	[CountryName] [varchar](100) NOT NULL,
	[CountryCode] [varchar](100) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED 
(
	[CountryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Downloads]    Script Date: 10-04-2021 22:28:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Downloads](
	[DownloadID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[Seller] [int] NOT NULL,
	[Downloader] [int] NOT NULL,
	[IsSellerHasAllowedDownload] [bit] NOT NULL,
	[AttachmentPath] [nvarchar](260) NULL,
	[IsAttachmentDownloaded] [bit] NOT NULL,
	[AttachmentDownloadedDate] [datetime] NULL,
	[SellFor] [varchar](20) NOT NULL,
	[PurchasedPrice] [decimal](18, 0) NULL,
	[NoteTitle] [varchar](100) NOT NULL,
	[NoteCategory] [int] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Downloads] PRIMARY KEY CLUSTERED 
(
	[DownloadID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notes]    Script Date: 10-04-2021 22:28:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notes](
	[NoteID] [int] IDENTITY(1,1) NOT NULL,
	[NotesDp] [varchar](max) NULL,
	[SellerID] [int] NOT NULL,
	[NotesTitle] [varchar](100) NOT NULL,
	[NotesCategory] [int] NOT NULL,
	[NotesType] [int] NOT NULL,
	[UniversityInformation] [varchar](100) NOT NULL,
	[Country] [int] NOT NULL,
	[CourseName] [varchar](100) NOT NULL,
	[CourseCode] [int] NOT NULL,
	[Professor] [varchar](50) NOT NULL,
	[SellType] [varchar](50) NOT NULL,
	[SellPrice] [decimal](18, 0) NULL,
	[PreviewUpload] [nvarchar](max) NULL,
	[NoOfPages] [int] NULL,
	[Description] [varchar](max) NOT NULL,
	[NotesPublishedDateTime] [datetime] NULL,
	[AdminRemarks] [varchar](max) NULL,
	[IsInappropriate] [bit] NULL,
	[Status] [int] NOT NULL,
	[ActionedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Notes] PRIMARY KEY CLUSTERED 
(
	[NoteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotesStatus]    Script Date: 10-04-2021 22:28:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotesStatus](
	[StatusID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [varchar](20) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_NotesStatus] PRIMARY KEY CLUSTERED 
(
	[StatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reference]    Script Date: 10-04-2021 22:28:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reference](
	[ReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[Value] [varchar](100) NOT NULL,
	[DataValue] [varchar](100) NOT NULL,
	[ReferenceCategory] [varchar](100) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Reference] PRIMARY KEY CLUSTERED 
(
	[ReferenceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerNoteAttachment]    Script Date: 10-04-2021 22:28:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerNoteAttachment](
	[NoteAttachmentID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[FileName] [varchar](100) NOT NULL,
	[FilePath] [nvarchar](260) NOT NULL,
	[FileSize] [float] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SellerNoteAttachment] PRIMARY KEY CLUSTERED 
(
	[NoteAttachmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SpamReports]    Script Date: 10-04-2021 22:28:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SpamReports](
	[SpamID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[DownloadID] [int] NOT NULL,
	[Remarks] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SpamReports] PRIMARY KEY CLUSTERED 
(
	[SpamID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemConfigurations]    Script Date: 10-04-2021 22:28:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemConfigurations](
	[SystemConfigurationID] [int] IDENTITY(1,1) NOT NULL,
	[SupportEmail] [varchar](100) NOT NULL,
	[SupportContactNo] [varchar](15) NOT NULL,
	[EmailAdresses] [varchar](100) NULL,
	[FacebookURL] [varchar](max) NULL,
	[TwitterURL] [varchar](max) NULL,
	[LinkedInURL] [varchar](max) NULL,
	[DefaultNoteDp] [image] NOT NULL,
	[DefaultDp] [image] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SystemConfigurations] PRIMARY KEY CLUSTERED 
(
	[SystemConfigurationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Type]    Script Date: 10-04-2021 22:28:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Type](
	[TypeID] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [varchar](100) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Type] PRIMARY KEY CLUSTERED 
(
	[TypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 10-04-2021 22:28:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](100) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 10-04-2021 22:28:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [int] NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[EmailId] [varchar](100) NOT NULL,
	[Password] [varchar](24) NOT NULL,
	[IsEmailVerified] [bit] NOT NULL,
	[PhoneNoCountryCode] [varchar](5) NULL,
	[PhoneNo] [varchar](20) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsersProfile]    Script Date: 10-04-2021 22:28:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsersProfile](
	[ProfileID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[EmailID] [varchar](50) NOT NULL,
	[DOB] [datetime] NULL,
	[Gender] [int] NOT NULL,
	[SecondaryEmailAddress] [varchar](100) NULL,
	[PhoneCountryCode] [varchar](5) NOT NULL,
	[PhoneNumber] [varchar](20) NOT NULL,
	[ProfilePic] [varchar](max) NULL,
	[AddressLine1] [varchar](100) NULL,
	[AddressLine2] [varchar](100) NULL,
	[City] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[ZipCode] [varchar](50) NULL,
	[Country] [int] NOT NULL,
	[University] [varchar](100) NULL,
	[College] [varchar](100) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_UsersProfile] PRIMARY KEY CLUSTERED 
(
	[ProfileID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsersReviews]    Script Date: 10-04-2021 22:28:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsersReviews](
	[ReviewID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[ReviewedBy] [int] NOT NULL,
	[AgainstDownload] [int] NOT NULL,
	[Ratings] [decimal](2, 1) NOT NULL,
	[Comments] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_UsersReviews] PRIMARY KEY CLUSTERED 
(
	[ReviewID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'Computer Science', N'All Computer science and engineering related notes', CAST(N'2021-04-02T15:40:29.683' AS DateTime), 15, CAST(N'2021-04-02T15:40:29.683' AS DateTime), 15, 0)
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, N'Politics', N'Politics related notes', CAST(N'2021-04-02T15:40:37.020' AS DateTime), 15, CAST(N'2021-04-02T15:40:37.020' AS DateTime), 15, 1)
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3, N'Chess', N'Notes giving knowledge on chess game', CAST(N'2021-04-02T15:40:43.470' AS DateTime), 15, CAST(N'2021-04-02T15:40:43.470' AS DateTime), 15, 0)
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1002, N'Finance & Accounts', N'Finance and accounts related books.', CAST(N'2021-04-06T19:25:29.463' AS DateTime), 15, CAST(N'2021-04-06T19:25:29.463' AS DateTime), 15, 1)
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2002, N'Novel & Story Books', N'All Novels and story books can be included', CAST(N'2021-04-09T08:19:51.920' AS DateTime), 15, CAST(N'2021-04-09T08:19:51.920' AS DateTime), 15, 0)
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2003, N'Communication Skills', N'Giving Best fluency of communication', CAST(N'2021-04-09T12:39:47.690' AS DateTime), 16, NULL, NULL, 1)
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2004, N'Maths', N'Maths books', CAST(N'2021-04-09T12:42:18.707' AS DateTime), 16, NULL, NULL, 1)
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2005, N'Programming', N'Java, Ptyhon etc computer programming books', CAST(N'2021-04-09T12:43:01.580' AS DateTime), 16, NULL, NULL, 1)
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2006, N'Reference books', N'Any Reference book', CAST(N'2021-04-09T12:46:48.077' AS DateTime), 16, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[ContactUs] ON 

INSERT [dbo].[ContactUs] ([ContactID], [FullName], [EmailAddress], [Subject], [Comments], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'Samar Shah', N'ss@gmail.com', N'Appointment', N'just wanted to book appointment', CAST(N'2021-03-03T21:54:27.703' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[ContactUs] ([ContactID], [FullName], [EmailAddress], [Subject], [Comments], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, N'Neha Vyas', N'nkvyas99@gmail.com', N'Appointment', N'Please Schedule an appointment.', CAST(N'2021-03-21T18:10:39.950' AS DateTime), NULL, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[ContactUs] OFF
GO
SET IDENTITY_INSERT [dbo].[Country] ON 

INSERT [dbo].[Country] ([CountryID], [CountryName], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, N'India', N'+91', CAST(N'2021-04-02T15:43:11.213' AS DateTime), 15, CAST(N'2021-04-02T15:43:11.213' AS DateTime), 15, 0)
INSERT [dbo].[Country] ([CountryID], [CountryName], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3, N'China', N'+86', CAST(N'2021-04-02T15:43:16.060' AS DateTime), 15, CAST(N'2021-04-02T15:43:16.060' AS DateTime), 15, 1)
INSERT [dbo].[Country] ([CountryID], [CountryName], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4, N'USA', N'+1', CAST(N'2021-04-02T15:43:20.980' AS DateTime), 15, CAST(N'2021-04-02T15:43:20.980' AS DateTime), 15, 1)
INSERT [dbo].[Country] ([CountryID], [CountryName], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (5, N'Germany', N'+49', CAST(N'2021-04-02T15:43:57.800' AS DateTime), 15, CAST(N'2021-04-02T15:43:57.800' AS DateTime), 15, 1)
INSERT [dbo].[Country] ([CountryID], [CountryName], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1002, N'Italy', N'39', CAST(N'2021-04-09T13:17:31.853' AS DateTime), 16, NULL, NULL, 1)
INSERT [dbo].[Country] ([CountryID], [CountryName], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1003, N'UK', N'44', CAST(N'2021-04-09T13:18:44.087' AS DateTime), 16, NULL, NULL, 1)
INSERT [dbo].[Country] ([CountryID], [CountryName], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1004, N'NewZealnad', N'64', CAST(N'2021-04-09T13:20:10.870' AS DateTime), 16, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[Country] OFF
GO
SET IDENTITY_INSERT [dbo].[Downloads] ON 

INSERT [dbo].[Downloads] ([DownloadID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [SellFor], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3, 5, 12, 13, 1, N'Attachment_1_07032021.pdf;', 1, NULL, N'Paid', CAST(50 AS Decimal(18, 0)), N'A game of queens', 3, NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[Downloads] ([DownloadID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [SellFor], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1002, 1006, 13, 12, 1, N'~/Member/13/1006/Attachment_1_24032021.pdf;', 1, CAST(N'2021-03-25T23:39:09.773' AS DateTime), N'Free', CAST(0 AS Decimal(18, 0)), N'India 2020', 2, CAST(N'2021-03-25T23:39:09.773' AS DateTime), 12, NULL, NULL, 1)
INSERT [dbo].[Downloads] ([DownloadID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [SellFor], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1003, 1007, 12, 13, 1, N'/Member/12/1007/attachment/Attachment_1_25032021.pdf;', 1, CAST(N'2021-03-26T00:01:15.987' AS DateTime), N'Free', CAST(0 AS Decimal(18, 0)), N'DBMS', 1, CAST(N'2021-03-26T00:01:15.987' AS DateTime), 13, NULL, NULL, 1)
INSERT [dbo].[Downloads] ([DownloadID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [SellFor], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1005, 1008, 13, 12, 1, NULL, 0, CAST(N'2021-04-03T19:05:19.593' AS DateTime), N'Paid', CAST(20 AS Decimal(18, 0)), N'Operating Systems', 1, CAST(N'2021-04-03T19:05:19.593' AS DateTime), 12, NULL, NULL, 1)
INSERT [dbo].[Downloads] ([DownloadID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [SellFor], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1006, 1007, 12, 12, 1, N'/Member/12/1007/attachment/Attachment_1_25032021.pdf;', 1, CAST(N'2021-04-05T23:29:07.987' AS DateTime), N'Free', CAST(0 AS Decimal(18, 0)), N'DBMS', 1, CAST(N'2021-04-05T23:29:07.987' AS DateTime), 12, NULL, NULL, 1)
INSERT [dbo].[Downloads] ([DownloadID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [SellFor], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2006, 6, 13, 15, 1, N'/Member/13/1006/attachment/Attachment_1_24032021.pdf;', 0, NULL, N'Paid', CAST(5 AS Decimal(18, 0)), N'Python', 1, CAST(N'2021-04-09T01:04:48.357' AS DateTime), 15, CAST(N'2021-04-10T13:12:10.490' AS DateTime), 13, 1)
INSERT [dbo].[Downloads] ([DownloadID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [SellFor], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2007, 1009, 12, 13, 1, N'/Member/12/1009/attachment/Attachment_1_09042021.pdf;', 0, NULL, N'Paid', CAST(8 AS Decimal(18, 0)), N'Two States', 2002, CAST(N'2021-04-09T09:52:02.633' AS DateTime), 13, CAST(N'2021-04-09T09:58:08.743' AS DateTime), 12, 1)
INSERT [dbo].[Downloads] ([DownloadID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [SellFor], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2008, 6, 13, 12, 1, N'/Member/13/1006/attachment/Attachment_1_24032021.pdf;', 0, NULL, N'Paid', CAST(5 AS Decimal(18, 0)), N'Python', 1, CAST(N'2021-04-10T13:09:12.487' AS DateTime), 12, CAST(N'2021-04-10T13:16:19.183' AS DateTime), 13, 1)
SET IDENTITY_INSERT [dbo].[Downloads] OFF
GO
SET IDENTITY_INSERT [dbo].[Notes] ON 

INSERT [dbo].[Notes] ([NoteID], [NotesDp], [SellerID], [NotesTitle], [NotesCategory], [NotesType], [UniversityInformation], [Country], [CourseName], [CourseCode], [Professor], [SellType], [SellPrice], [PreviewUpload], [NoOfPages], [Description], [NotesPublishedDateTime], [AdminRemarks], [IsInappropriate], [Status], [ActionedBy], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (5, N'~/Member/12/5/DP_07032021.jpg', 12, N'A game of Queens', 3, 4, N'MSU Baroda', 2, N'Chess ', 53, N'Judgit Polghar', N'Paid', CAST(50 AS Decimal(18, 0)), N'~/Member/12/5/PREVIEW_07032021.pdf', 50, N'Knowledge giving books', NULL, NULL, NULL, 3, NULL, CAST(N'2021-03-07T15:11:10.267' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Notes] ([NoteID], [NotesDp], [SellerID], [NotesTitle], [NotesCategory], [NotesType], [UniversityInformation], [Country], [CourseName], [CourseCode], [Professor], [SellType], [SellPrice], [PreviewUpload], [NoOfPages], [Description], [NotesPublishedDateTime], [AdminRemarks], [IsInappropriate], [Status], [ActionedBy], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (6, N'~/Member/13/6/DP_24032021.jpg', 13, N'Python', 1, 2, N'NMIMS Mumbai ', 2, N'Information Technology', 116, N'Prof J.P Singh', N'Paid', CAST(5 AS Decimal(18, 0)), N'~/Member/13/6/PREVIEW_24032021.pdf', 250, N'Well explained concepts of Python Programming.', CAST(N'2021-03-24T12:57:12.623' AS DateTime), NULL, NULL, 2, NULL, CAST(N'2021-03-24T12:57:12.623' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Notes] ([NoteID], [NotesDp], [SellerID], [NotesTitle], [NotesCategory], [NotesType], [UniversityInformation], [Country], [CourseName], [CourseCode], [Professor], [SellType], [SellPrice], [PreviewUpload], [NoOfPages], [Description], [NotesPublishedDateTime], [AdminRemarks], [IsInappropriate], [Status], [ActionedBy], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1006, N'~/Member/13/1006/DP_24032021.jpg', 13, N'India 2020', 2, 4, N'Jaipur University', 2, N'History', 88, N'Judgit Polghar', N'Free', CAST(0 AS Decimal(18, 0)), N'~/Member/13/1006/PREVIEW_24032021.pdf', 200, N'A Book Describing Future Scenarios of Nation India.', NULL, N'Not good content ', NULL, 2, 15, CAST(N'2021-03-24T22:43:43.833' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Notes] ([NoteID], [NotesDp], [SellerID], [NotesTitle], [NotesCategory], [NotesType], [UniversityInformation], [Country], [CourseName], [CourseCode], [Professor], [SellType], [SellPrice], [PreviewUpload], [NoOfPages], [Description], [NotesPublishedDateTime], [AdminRemarks], [IsInappropriate], [Status], [ActionedBy], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1007, N'~/Member/12/1007/DP_25032021.jpg', 12, N'DBMS', 1, 1, N'LD  College of Engineering', 2, N'Computer Science', 118, N'Sunita Pawar', N'Free', CAST(0 AS Decimal(18, 0)), N'~/Member/12/1007/PREVIEW_25032021.pdf', 150, N'Database Management concepts', CAST(N'2021-04-09T08:36:04.053' AS DateTime), NULL, NULL, 2, 15, CAST(N'2021-03-25T23:57:21.543' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Notes] ([NoteID], [NotesDp], [SellerID], [NotesTitle], [NotesCategory], [NotesType], [UniversityInformation], [Country], [CourseName], [CourseCode], [Professor], [SellType], [SellPrice], [PreviewUpload], [NoOfPages], [Description], [NotesPublishedDateTime], [AdminRemarks], [IsInappropriate], [Status], [ActionedBy], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1008, N'~/Member/13/1008/DP_27032021.jpg', 13, N'Operating Systems', 1, 1, N'DDU', 2, N'Computer Engineering', 53, N'P.K. Shah', N'Paid', CAST(20 AS Decimal(18, 0)), N'~/Member/13/1008/PREVIEW_27032021.pdf', 280, N'Some important notes on Operating System.', CAST(N'2021-03-27T11:38:48.153' AS DateTime), NULL, NULL, 2, 15, CAST(N'2021-03-27T11:38:48.153' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Notes] ([NoteID], [NotesDp], [SellerID], [NotesTitle], [NotesCategory], [NotesType], [UniversityInformation], [Country], [CourseName], [CourseCode], [Professor], [SellType], [SellPrice], [PreviewUpload], [NoOfPages], [Description], [NotesPublishedDateTime], [AdminRemarks], [IsInappropriate], [Status], [ActionedBy], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1009, N'~/Member/12/1009/DP_09042021.jpg', 12, N'Two States', 2002, 3, N'Library', 2, N'Novel Reading', 222, N'K.K Singh', N'Paid', CAST(8 AS Decimal(18, 0)), N'~/Member/12/1009/PREVIEW_09042021.pdf', 450, N'Love Story ', CAST(N'2021-04-09T09:43:48.473' AS DateTime), NULL, NULL, 4, 15, CAST(N'2021-04-09T08:25:07.863' AS DateTime), 12, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[Notes] OFF
GO
SET IDENTITY_INSERT [dbo].[NotesStatus] ON 

INSERT [dbo].[NotesStatus] ([StatusID], [Status], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'Draft', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[NotesStatus] ([StatusID], [Status], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, N'Published', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[NotesStatus] ([StatusID], [Status], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3, N'Rejected', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[NotesStatus] ([StatusID], [Status], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4, N'Submitted For Review', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[NotesStatus] ([StatusID], [Status], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (5, N'InReview', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[NotesStatus] ([StatusID], [Status], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (6, N'Removed', NULL, NULL, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[NotesStatus] OFF
GO
SET IDENTITY_INSERT [dbo].[Reference] ON 

INSERT [dbo].[Reference] ([ReferenceID], [Value], [DataValue], [ReferenceCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'Male', N'0', N'Gender', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[Reference] ([ReferenceID], [Value], [DataValue], [ReferenceCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, N'Female', N'1', N'Gender', NULL, NULL, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[Reference] OFF
GO
SET IDENTITY_INSERT [dbo].[SellerNoteAttachment] ON 

INSERT [dbo].[SellerNoteAttachment] ([NoteAttachmentID], [NoteID], [FileName], [FilePath], [FileSize], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, 5, N'Attachment_1_07032021.pdf;', N'/Member/13/6/attachment/Attachment_1_24032021.pdf;', NULL, CAST(N'2021-03-07T15:11:10.883' AS DateTime), 12, NULL, NULL, 1)
INSERT [dbo].[SellerNoteAttachment] ([NoteAttachmentID], [NoteID], [FileName], [FilePath], [FileSize], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1002, 6, N'Attachment_1_24032021.pdf;', N'/Member/13/1006/attachment/Attachment_1_24032021.pdf;', NULL, CAST(N'2021-03-24T12:57:17.673' AS DateTime), 13, NULL, NULL, 1)
INSERT [dbo].[SellerNoteAttachment] ([NoteAttachmentID], [NoteID], [FileName], [FilePath], [FileSize], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2002, 1006, N'Attachment_1_24032021.pdf;', N'~/Member/13/1006/Attachment_1_24032021.pdf;', NULL, CAST(N'2021-03-24T22:43:51.447' AS DateTime), 13, NULL, NULL, 1)
INSERT [dbo].[SellerNoteAttachment] ([NoteAttachmentID], [NoteID], [FileName], [FilePath], [FileSize], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2003, 1007, N'Attachment_1_25032021.pdf;', N'/Member/12/1007/attachment/Attachment_1_25032021.pdf;', NULL, CAST(N'2021-03-25T23:57:28.577' AS DateTime), 12, NULL, NULL, 1)
INSERT [dbo].[SellerNoteAttachment] ([NoteAttachmentID], [NoteID], [FileName], [FilePath], [FileSize], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2004, 1008, N'Attachment_1_27032021.pdf;', N'/Member/13/1008/attachment/Attachment_1_27032021.pdf;', NULL, CAST(N'2021-03-27T11:38:58.763' AS DateTime), 13, NULL, NULL, 1)
INSERT [dbo].[SellerNoteAttachment] ([NoteAttachmentID], [NoteID], [FileName], [FilePath], [FileSize], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2005, 1009, N'Attachment_1_09042021.pdf;', N'/Member/12/1009/attachment/Attachment_1_09042021.pdf;', NULL, CAST(N'2021-04-09T08:25:13.433' AS DateTime), 12, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[SellerNoteAttachment] OFF
GO
SET IDENTITY_INSERT [dbo].[SpamReports] ON 

INSERT [dbo].[SpamReports] ([SpamID], [NoteID], [UserID], [DownloadID], [Remarks], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, 1006, 12, 1002, N'Not proper content.', CAST(N'2021-04-08T13:15:58.527' AS DateTime), 12, CAST(N'2021-04-08T13:15:58.527' AS DateTime), 12, 1)
SET IDENTITY_INSERT [dbo].[SpamReports] OFF
GO
SET IDENTITY_INSERT [dbo].[Type] ON 

INSERT [dbo].[Type] ([TypeID], [TypeName], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'HandwrittenNotes', N'Hand wriiten notes by somebody', CAST(N'2021-04-02T13:55:04.010' AS DateTime), 15, CAST(N'2021-04-02T13:55:04.010' AS DateTime), 15, 0)
INSERT [dbo].[Type] ([TypeID], [TypeName], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, N'UniversityNotes', N' Notes by Some University', CAST(N'2021-04-02T13:55:00.160' AS DateTime), 15, CAST(N'2021-04-02T13:55:00.160' AS DateTime), 15, 1)
INSERT [dbo].[Type] ([TypeID], [TypeName], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3, N'Novel', N' Novels by different author', CAST(N'2021-04-02T13:54:54.890' AS DateTime), 15, CAST(N'2021-04-02T13:54:54.890' AS DateTime), 15, 1)
INSERT [dbo].[Type] ([TypeID], [TypeName], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4, N'Story books', N'Different Story books', CAST(N'2021-04-02T13:54:15.883' AS DateTime), 15, CAST(N'2021-04-02T13:54:15.883' AS DateTime), 15, 1)
INSERT [dbo].[Type] ([TypeID], [TypeName], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (5, N'Autobiography', N'Any autobiagraphy', CAST(N'2021-04-09T12:54:03.703' AS DateTime), 16, NULL, NULL, 1)
INSERT [dbo].[Type] ([TypeID], [TypeName], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (6, N'Placement Preparations', N'Any book book that can help freshers in placement', CAST(N'2021-04-09T12:55:24.953' AS DateTime), 16, NULL, NULL, 1)
INSERT [dbo].[Type] ([TypeID], [TypeName], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (7, N'Funny', N'Some joke sharing books', CAST(N'2021-04-09T12:56:04.957' AS DateTime), 16, NULL, NULL, 1)
INSERT [dbo].[Type] ([TypeID], [TypeName], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (8, N'Exam papers solutions', N'some solved questions of impoertant exams', CAST(N'2021-04-09T12:57:45.210' AS DateTime), 16, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[Type] OFF
GO
SET IDENTITY_INSERT [dbo].[UserRoles] ON 

INSERT [dbo].[UserRoles] ([RoleID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1003, N'Admin', N'Controls Activities of Users', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[UserRoles] ([RoleID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1004, N'Super Admin', N'Master', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[UserRoles] ([RoleID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1005, N'Customers', N'Anyone who uses the Web Application', NULL, NULL, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[UserRoles] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserID], [RoleID], [FirstName], [LastName], [EmailId], [Password], [IsEmailVerified], [PhoneNoCountryCode], [PhoneNo], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (12, 1005, N'Nidhi', N'Vyas', N'vyasnidhi11@gmail.com', N'Nidhi99&', 1, NULL, NULL, CAST(N'2021-03-06T20:51:27.303' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([UserID], [RoleID], [FirstName], [LastName], [EmailId], [Password], [IsEmailVerified], [PhoneNoCountryCode], [PhoneNo], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (13, 1005, N'Yash', N'Patel', N'nkvyas99@gmail.com', N'Nidhi11&', 1, NULL, NULL, CAST(N'2021-03-07T11:52:33.700' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([UserID], [RoleID], [FirstName], [LastName], [EmailId], [Password], [IsEmailVerified], [PhoneNoCountryCode], [PhoneNo], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (15, 1003, N'Pritam', N'Vyas', N'vyaspratima301@gmail.com', N'Xyz987&', 1, NULL, NULL, CAST(N'2021-03-14T17:41:37.740' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([UserID], [RoleID], [FirstName], [LastName], [EmailId], [Password], [IsEmailVerified], [PhoneNoCountryCode], [PhoneNo], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (16, 1003, N'Varun', N'Shah', N'abc@gmail.com', N'Admin123&', 0, N'2', N'8846692897', CAST(N'2021-04-07T13:42:11.533' AS DateTime), 15, CAST(N'2021-04-07T13:42:11.533' AS DateTime), 15, 1)
INSERT [dbo].[Users] ([UserID], [RoleID], [FirstName], [LastName], [EmailId], [Password], [IsEmailVerified], [PhoneNoCountryCode], [PhoneNo], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (18, 1005, N'Roberts', N'Evans', N'rbevans@gmail.com', N'Rb278$', 1, NULL, NULL, CAST(N'2021-04-09T13:08:45.290' AS DateTime), NULL, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET IDENTITY_INSERT [dbo].[UsersProfile] ON 

INSERT [dbo].[UsersProfile] ([ProfileID], [UserID], [FirstName], [LastName], [EmailID], [DOB], [Gender], [SecondaryEmailAddress], [PhoneCountryCode], [PhoneNumber], [ProfilePic], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, 12, N'Nidhi', N'Vyas', N'vyasnidhi11@gmail.com', CAST(N'1999-02-11T00:00:00.000' AS DateTime), 2, NULL, N'2', N'9725167344', N'~/Member/12/1/DP_07042021.jpg', N'27, Avadh Upvan', N'Atladra-Bill Road', N'Vadodara', N'Gujarat', N'390012', 2, N'GTU', N'GEC GANDHINAGAR', CAST(N'2021-04-06T20:25:40.980' AS DateTime), 12, CAST(N'2021-04-06T20:25:40.980' AS DateTime), 12, 1)
INSERT [dbo].[UsersProfile] ([ProfileID], [UserID], [FirstName], [LastName], [EmailID], [DOB], [Gender], [SecondaryEmailAddress], [PhoneCountryCode], [PhoneNumber], [ProfilePic], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4, 18, N'Roberts', N'Evans', N'rbevans@gmail.com', CAST(N'2002-06-10T00:00:00.000' AS DateTime), 1, NULL, N'4', N'6099061768', NULL, N'667 Madison eve', N'Avenue of America', N'Albany', N'New York', N'10001', 4, N'New york university', N'Cornell college', CAST(N'2021-04-10T16:27:09.770' AS DateTime), 18, CAST(N'2021-04-10T16:27:09.770' AS DateTime), 18, 1)
SET IDENTITY_INSERT [dbo].[UsersProfile] OFF
GO
SET IDENTITY_INSERT [dbo].[UsersReviews] ON 

INSERT [dbo].[UsersReviews] ([ReviewID], [NoteID], [ReviewedBy], [AgainstDownload], [Ratings], [Comments], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, 1006, 12, 1002, CAST(2.0 AS Decimal(2, 1)), N'Good book.', CAST(N'2021-03-27T08:03:03.860' AS DateTime), 12, CAST(N'2021-03-27T08:03:03.860' AS DateTime), 12, 1)
SET IDENTITY_INSERT [dbo].[UsersReviews] OFF
GO
ALTER TABLE [dbo].[Categories] ADD  CONSTRAINT [DF_Categories_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[ContactUs] ADD  CONSTRAINT [DF_ContactUs_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Country] ADD  CONSTRAINT [DF_Country_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Downloads] ADD  CONSTRAINT [DF_Downloads_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Notes] ADD  CONSTRAINT [DF_Notes_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Reference] ADD  CONSTRAINT [DF_Reference_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[SpamReports] ADD  CONSTRAINT [DF_SpamReports_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[SystemConfigurations] ADD  CONSTRAINT [DF_SystemConfigurations_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Type] ADD  CONSTRAINT [DF_Type_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[UserRoles] ADD  CONSTRAINT [DF_UserRoles_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[UsersProfile] ADD  CONSTRAINT [DF_UsersProfile_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[UsersReviews] ADD  CONSTRAINT [DF_UsersReviews_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Downloads]  WITH CHECK ADD  CONSTRAINT [FK_Downloads_Categories] FOREIGN KEY([NoteCategory])
REFERENCES [dbo].[Categories] ([CategoryID])
GO
ALTER TABLE [dbo].[Downloads] CHECK CONSTRAINT [FK_Downloads_Categories]
GO
ALTER TABLE [dbo].[Downloads]  WITH CHECK ADD  CONSTRAINT [FK_Downloads_Notes] FOREIGN KEY([NoteID])
REFERENCES [dbo].[Notes] ([NoteID])
GO
ALTER TABLE [dbo].[Downloads] CHECK CONSTRAINT [FK_Downloads_Notes]
GO
ALTER TABLE [dbo].[Downloads]  WITH CHECK ADD  CONSTRAINT [FK_Downloads_Users] FOREIGN KEY([Seller])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Downloads] CHECK CONSTRAINT [FK_Downloads_Users]
GO
ALTER TABLE [dbo].[Downloads]  WITH CHECK ADD  CONSTRAINT [FK_Downloads_Users1] FOREIGN KEY([Downloader])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Downloads] CHECK CONSTRAINT [FK_Downloads_Users1]
GO
ALTER TABLE [dbo].[Notes]  WITH CHECK ADD  CONSTRAINT [FK_Notes_Categories] FOREIGN KEY([NotesCategory])
REFERENCES [dbo].[Categories] ([CategoryID])
GO
ALTER TABLE [dbo].[Notes] CHECK CONSTRAINT [FK_Notes_Categories]
GO
ALTER TABLE [dbo].[Notes]  WITH CHECK ADD  CONSTRAINT [FK_Notes_Country] FOREIGN KEY([Country])
REFERENCES [dbo].[Country] ([CountryID])
GO
ALTER TABLE [dbo].[Notes] CHECK CONSTRAINT [FK_Notes_Country]
GO
ALTER TABLE [dbo].[Notes]  WITH CHECK ADD  CONSTRAINT [FK_Notes_Notes] FOREIGN KEY([NoteID])
REFERENCES [dbo].[Notes] ([NoteID])
GO
ALTER TABLE [dbo].[Notes] CHECK CONSTRAINT [FK_Notes_Notes]
GO
ALTER TABLE [dbo].[Notes]  WITH CHECK ADD  CONSTRAINT [FK_Notes_Notes1] FOREIGN KEY([NoteID])
REFERENCES [dbo].[Notes] ([NoteID])
GO
ALTER TABLE [dbo].[Notes] CHECK CONSTRAINT [FK_Notes_Notes1]
GO
ALTER TABLE [dbo].[Notes]  WITH CHECK ADD  CONSTRAINT [FK_Notes_Notes2] FOREIGN KEY([NoteID])
REFERENCES [dbo].[Notes] ([NoteID])
GO
ALTER TABLE [dbo].[Notes] CHECK CONSTRAINT [FK_Notes_Notes2]
GO
ALTER TABLE [dbo].[Notes]  WITH CHECK ADD  CONSTRAINT [FK_Notes_Notes3] FOREIGN KEY([NoteID])
REFERENCES [dbo].[Notes] ([NoteID])
GO
ALTER TABLE [dbo].[Notes] CHECK CONSTRAINT [FK_Notes_Notes3]
GO
ALTER TABLE [dbo].[Notes]  WITH CHECK ADD  CONSTRAINT [FK_Notes_NotesStatus] FOREIGN KEY([Status])
REFERENCES [dbo].[NotesStatus] ([StatusID])
GO
ALTER TABLE [dbo].[Notes] CHECK CONSTRAINT [FK_Notes_NotesStatus]
GO
ALTER TABLE [dbo].[Notes]  WITH CHECK ADD  CONSTRAINT [FK_Notes_Type] FOREIGN KEY([NotesType])
REFERENCES [dbo].[Type] ([TypeID])
GO
ALTER TABLE [dbo].[Notes] CHECK CONSTRAINT [FK_Notes_Type]
GO
ALTER TABLE [dbo].[Notes]  WITH CHECK ADD  CONSTRAINT [FK_Notes_Users] FOREIGN KEY([SellerID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Notes] CHECK CONSTRAINT [FK_Notes_Users]
GO
ALTER TABLE [dbo].[Notes]  WITH CHECK ADD  CONSTRAINT [FK_Notes_Users1] FOREIGN KEY([ActionedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Notes] CHECK CONSTRAINT [FK_Notes_Users1]
GO
ALTER TABLE [dbo].[SellerNoteAttachment]  WITH CHECK ADD  CONSTRAINT [FK_SellerNoteAttachment_Notes] FOREIGN KEY([NoteID])
REFERENCES [dbo].[Notes] ([NoteID])
GO
ALTER TABLE [dbo].[SellerNoteAttachment] CHECK CONSTRAINT [FK_SellerNoteAttachment_Notes]
GO
ALTER TABLE [dbo].[SpamReports]  WITH CHECK ADD  CONSTRAINT [FK_SpamReports_Downloads] FOREIGN KEY([DownloadID])
REFERENCES [dbo].[Downloads] ([DownloadID])
GO
ALTER TABLE [dbo].[SpamReports] CHECK CONSTRAINT [FK_SpamReports_Downloads]
GO
ALTER TABLE [dbo].[SpamReports]  WITH CHECK ADD  CONSTRAINT [FK_SpamReports_Notes] FOREIGN KEY([NoteID])
REFERENCES [dbo].[Notes] ([NoteID])
GO
ALTER TABLE [dbo].[SpamReports] CHECK CONSTRAINT [FK_SpamReports_Notes]
GO
ALTER TABLE [dbo].[SpamReports]  WITH CHECK ADD  CONSTRAINT [FK_SpamReports_SpamReports] FOREIGN KEY([SpamID])
REFERENCES [dbo].[SpamReports] ([SpamID])
GO
ALTER TABLE [dbo].[SpamReports] CHECK CONSTRAINT [FK_SpamReports_SpamReports]
GO
ALTER TABLE [dbo].[SpamReports]  WITH CHECK ADD  CONSTRAINT [FK_SpamReports_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[SpamReports] CHECK CONSTRAINT [FK_SpamReports_Users]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_UserRoles] FOREIGN KEY([RoleID])
REFERENCES [dbo].[UserRoles] ([RoleID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_UserRoles]
GO
ALTER TABLE [dbo].[UsersReviews]  WITH CHECK ADD  CONSTRAINT [FK_UsersReviews_Downloads] FOREIGN KEY([AgainstDownload])
REFERENCES [dbo].[Downloads] ([DownloadID])
GO
ALTER TABLE [dbo].[UsersReviews] CHECK CONSTRAINT [FK_UsersReviews_Downloads]
GO
ALTER TABLE [dbo].[UsersReviews]  WITH CHECK ADD  CONSTRAINT [FK_UsersReviews_Notes] FOREIGN KEY([NoteID])
REFERENCES [dbo].[Notes] ([NoteID])
GO
ALTER TABLE [dbo].[UsersReviews] CHECK CONSTRAINT [FK_UsersReviews_Notes]
GO
ALTER TABLE [dbo].[UsersReviews]  WITH CHECK ADD  CONSTRAINT [FK_UsersReviews_Users] FOREIGN KEY([ReviewedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[UsersReviews] CHECK CONSTRAINT [FK_UsersReviews_Users]
GO
USE [master]
GO
ALTER DATABASE [NotesMarketPlace] SET  READ_WRITE 
GO
