IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'vaGetVotesOverall')
	BEGIN
		PRINT 'Dropping Procedure vaGetVotesOverall'
		DROP  Procedure  vaGetVotesOverall
	END

GO

PRINT 'Creating Procedure vaGetVotesOverall'
GO
CREATE Procedure vaGetVotesOverall
	/* Param List */
	(
		@voteID	int
	)
AS

/******************************************************************************
**		File: vaGetVotesOverall.sql
**		Name: vaGetVotesOverall
**		Desc:
**
**		This template can be customized:
**              
**		Return values:
** 
**		Called by:   
**              
**		Parameters:
**		Input							Output
**     ----------							-----------
**
**		Auth: Simon Anderson
**		Date: Feb 1, 2013
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**
*******************************************************************************/
SET NOCOUNT ON

SELECT answer1count, answer2count
,COALESCE(answer1count * 100 /  NULLIF((answer1count + answer2count),0), 0) as answer1percent
,COALESCE(answer2count * 100 /  NULLIF((answer1count + answer2count),0), 0)  as answer2percent

FROM Vote 
WHERE id = @voteID

GO

GRANT EXEC ON vaGetVotesOverall TO PUBLIC

GO
