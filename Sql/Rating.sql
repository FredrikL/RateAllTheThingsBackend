SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Rating](
	[BarCodeId] [bigint] NOT NULL,
	[Author] [bigint] NOT NULL,
	[Rating] [tinyint] NOT NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Rating]  WITH CHECK ADD  CONSTRAINT [FK_Rating_BarCodes] FOREIGN KEY([BarCodeId])
REFERENCES [dbo].[BarCodes] ([ID])
GO

ALTER TABLE [dbo].[Rating] CHECK CONSTRAINT [FK_Rating_BarCodes]
GO

ALTER TABLE [dbo].[Rating]  WITH CHECK ADD  CONSTRAINT [FK_Rating_Users] FOREIGN KEY([Author])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Rating] CHECK CONSTRAINT [FK_Rating_Users]
GO


