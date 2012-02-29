SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Comments](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Text] [varchar](255) NOT NULL,
	[Author] [bigint] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[BarCodeId] [bigint] NOT NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_BarCodes] FOREIGN KEY([BarCodeId])
REFERENCES [dbo].[BarCodes] ([ID])
GO

ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_BarCodes]
GO

ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Users] FOREIGN KEY([Author])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Users]
GO

ALTER TABLE [dbo].[Comments] ADD  CONSTRAINT [DF_Comments_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO


