USE [db7f2bc721b4dc4f51a9c29fc80128fe45]
GO

/****** Object:  Table [dbo].[Log]    Script Date: 01/24/2012 21:36:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Log](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TimeStamp] [datetime] NULL,
	[AuthorId] [bigint] NOT NULL,
	[BarCodeId] [bigint] NULL,
	[Event] [varchar](50) NULL,
	[Data] [varchar](255) NULL,
	[Ip] [varchar](50) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Log] ADD  CONSTRAINT [DF_Log_TimeStamp]  DEFAULT (getdate()) FOR [TimeStamp]
GO

