﻿/****** Object:  Table [dbo].[FluidSurveyLinker]    Script Date: 06/22/2015 09:34:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FluidSurveyLinker](
	[UserId] [int] NOT NULL,
	[SurveyId] [varchar](500) NOT NULL,
	[Link] [varchar](500) NOT NULL,
 CONSTRAINT [PK_FluidSurveyLinker] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[SurveyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING ON
GO

GRANT SELECT, DELETE, INSERT, UPDATE ON [dbo].FluidSurveyLinker TO public
GO