SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BarCodes](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Format] [nvarchar](50) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](200) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [bigint] NOT NULL,
	[Manufacturer] [nvarchar](200) NULL,
 CONSTRAINT [PK_BarCodes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[BarCodes]  WITH CHECK ADD  CONSTRAINT [FK_BarCodes_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[BarCodes] CHECK CONSTRAINT [FK_BarCodes_Users]
GO

ALTER TABLE [dbo].[BarCodes] ADD  CONSTRAINT [DF_BarCodes_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO


