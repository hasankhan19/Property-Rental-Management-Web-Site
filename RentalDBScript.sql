USE [master]
GO
/****** Object:  Database [RentalManagementDB]    Script Date: 12/6/2023 11:13:54 PM ******/
CREATE DATABASE [RentalManagementDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RentalManagementDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\RentalManagementDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'RentalManagementDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\RentalManagementDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [RentalManagementDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RentalManagementDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RentalManagementDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [RentalManagementDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [RentalManagementDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [RentalManagementDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [RentalManagementDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [RentalManagementDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [RentalManagementDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [RentalManagementDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [RentalManagementDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [RentalManagementDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [RentalManagementDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [RentalManagementDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [RentalManagementDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [RentalManagementDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [RentalManagementDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [RentalManagementDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [RentalManagementDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [RentalManagementDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [RentalManagementDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [RentalManagementDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [RentalManagementDB] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [RentalManagementDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [RentalManagementDB] SET RECOVERY FULL 
GO
ALTER DATABASE [RentalManagementDB] SET  MULTI_USER 
GO
ALTER DATABASE [RentalManagementDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [RentalManagementDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [RentalManagementDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [RentalManagementDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [RentalManagementDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [RentalManagementDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'RentalManagementDB', N'ON'
GO
ALTER DATABASE [RentalManagementDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [RentalManagementDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [RentalManagementDB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 12/6/2023 11:13:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TblApartment]    Script Date: 12/6/2023 11:13:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TblApartment](
	[ApartmentId] [int] IDENTITY(1,1) NOT NULL,
	[BuildingId] [int] NULL,
	[Address] [nvarchar](max) NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[Bedrooms] [int] NOT NULL,
	[Bathrooms] [int] NOT NULL,
	[Status] [nvarchar](max) NULL,
	[Createdby] [int] NOT NULL,
	[CreatedDateTime] [datetime2](7) NOT NULL,
	[Updatedby] [int] NOT NULL,
	[UpdatedDateTime] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_TblApartment] PRIMARY KEY CLUSTERED 
(
	[ApartmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TblAppointment]    Script Date: 12/6/2023 11:13:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TblAppointment](
	[AppId] [int] IDENTITY(1,1) NOT NULL,
	[TenantId] [int] NOT NULL,
	[ManagerId] [int] NOT NULL,
	[ApartmentId] [int] NOT NULL,
	[SuggestedDateTime] [datetime2](7) NOT NULL,
	[FromDateTime] [datetime2](7) NOT NULL,
	[status] [nvarchar](max) NULL,
	[Createdby] [int] NOT NULL,
	[CreatedDateTime] [datetime2](7) NOT NULL,
	[Updatedby] [int] NOT NULL,
	[UpdatedDateTime] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_TblAppointment] PRIMARY KEY CLUSTERED 
(
	[AppId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TblBuilding]    Script Date: 12/6/2023 11:13:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TblBuilding](
	[BuildingId] [int] IDENTITY(1,1) NOT NULL,
	[BuildingName] [nvarchar](max) NULL,
	[PropertymangerId] [int] NOT NULL,
	[status] [nvarchar](max) NULL,
	[Createdby] [int] NOT NULL,
	[CreatedDateTime] [datetime2](7) NOT NULL,
	[Updatedby] [int] NOT NULL,
	[UpdatedDateTime] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_TblBuilding] PRIMARY KEY CLUSTERED 
(
	[BuildingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TblUser]    Script Date: 12/6/2023 11:13:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TblUser](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[ContactNo] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[Pasword] [nvarchar](max) NULL,
	[status] [nvarchar](max) NULL,
	[Role] [nvarchar](max) NULL,
	[Createdby] [int] NOT NULL,
	[CreatedDateTime] [datetime2](7) NOT NULL,
	[Updatedby] [int] NOT NULL,
	[UpdatedDateTime] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_TblUser] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [RentalManagementDB] SET  READ_WRITE 
GO
