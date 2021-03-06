USE [master]
GO

/****** Object:  Database [FlightsDB]    Script Date: 05/17/2020 07:50:53 ******/
CREATE DATABASE [FlightsDB] ON  PRIMARY 
( NAME = N'FlightsDB', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\FlightsDB.mdf' , SIZE = 65536KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'FlightsDB_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\FlightsDB_log.ldf' , SIZE = 65536KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [FlightsDB] SET COMPATIBILITY_LEVEL = 100
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FlightsDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [FlightsDB] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [FlightsDB] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [FlightsDB] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [FlightsDB] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [FlightsDB] SET ARITHABORT OFF 
GO

ALTER DATABASE [FlightsDB] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [FlightsDB] SET AUTO_CREATE_STATISTICS ON 
GO

ALTER DATABASE [FlightsDB] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [FlightsDB] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [FlightsDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [FlightsDB] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [FlightsDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [FlightsDB] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [FlightsDB] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [FlightsDB] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [FlightsDB] SET  DISABLE_BROKER 
GO

ALTER DATABASE [FlightsDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [FlightsDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [FlightsDB] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [FlightsDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [FlightsDB] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [FlightsDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [FlightsDB] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [FlightsDB] SET  READ_WRITE 
GO

ALTER DATABASE [FlightsDB] SET RECOVERY FULL 
GO

ALTER DATABASE [FlightsDB] SET  MULTI_USER 
GO

ALTER DATABASE [FlightsDB] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [FlightsDB] SET DB_CHAINING OFF 
GO


