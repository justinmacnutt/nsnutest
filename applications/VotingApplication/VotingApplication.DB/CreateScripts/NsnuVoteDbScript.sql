SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Vote](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[siteid] [int] NOT NULL,
	[title] [varchar](500) NOT NULL,
	[lure] [varchar](2000) NOT NULL,
	[question] [varchar](4000) NOT NULL,
	[answer1] [varchar](1000) NOT NULL,
	[answer2] [varchar](1000) NOT NULL,
	[answer1Count] [int] NOT NULL,
	[answer2Count] [int] NOT NULL,
	[displayDate] [datetime] NOT NULL,
	[expiryDate] [datetime] NOT NULL,
	[lastModifiedBy] [varchar](50) NOT NULL,
	[lastModifiedDate] [datetime] NOT NULL,
	[isDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Vote] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserVote]    Script Date: 01/28/2013 10:37:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserVote](
	[voteId] [int] NOT NULL,
	[userId] [varchar](50) NOT NULL,
	[voteDate] [datetime] NOT NULL,
 CONSTRAINT [PK_UserVote] PRIMARY KEY CLUSTERED 
(
	[voteId] ASC,
	[userId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Default [DF_Vote_answer1Count]    Script Date: 01/28/2013 10:37:12 ******/
ALTER TABLE [dbo].[Vote] ADD  CONSTRAINT [DF_Vote_answer1Count]  DEFAULT ((0)) FOR [answer1Count]
GO
/****** Object:  Default [DF_Vote_answer2Count]    Script Date: 01/28/2013 10:37:12 ******/
ALTER TABLE [dbo].[Vote] ADD  CONSTRAINT [DF_Vote_answer2Count]  DEFAULT ((0)) FOR [answer2Count]
GO

/****** Object:  Default [DF_Vote_isDeleted]    Script Date: 01/28/2013 10:37:12 ******/
ALTER TABLE [dbo].[Vote] ADD  CONSTRAINT [DF_Vote_isDeleted]  DEFAULT ((0)) FOR [isDeleted]
GO


/****** Object:  ForeignKey [FK_UserVote_voteId]    Script Date: 01/28/2013 10:37:12 ******/
ALTER TABLE [dbo].[UserVote]  WITH CHECK ADD  CONSTRAINT [FK_UserVote_voteId] FOREIGN KEY([voteId])
REFERENCES [dbo].[Vote] ([id])
GO
ALTER TABLE [dbo].[UserVote] CHECK CONSTRAINT [FK_UserVote_voteId]
GO
