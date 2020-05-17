USE [FlightsDB]
GO

/****** Object:  Table [dbo].[Flight]    Script Date: 05/17/2020 12:34:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

DROP TABLE [dbo].[Flight];

CREATE TABLE [dbo].[Flight](
	[Id] [bigint] NOT NULL,
	[Passengers] [int] NOT NULL,
	[CompanyName] [varchar](50) NOT NULL,
	[Longitude] [float] NOT NULL,
	[Latitude] [float] NOT NULL,
	[DateTime] [datetime] NOT NULL,
	[LocationId] [bigint] NOT NULL,
	
 CONSTRAINT [PK_Flights] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [FlightsDB]
GO

ALTER TABLE [dbo].[Flight]  WITH CHECK ADD  CONSTRAINT [FK_Flight_Location] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO

ALTER TABLE [dbo].[Flight] CHECK CONSTRAINT [FK_Flight_Location]
GO


