IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'vaGetVotesByDate')
	BEGIN
		PRINT 'Dropping Procedure vaGetVotesByDate'
		DROP  Procedure  vaGetVotesByDate
	END

GO

PRINT 'Creating Procedure vaGetVotesByDate'
GO
CREATE Procedure vaGetVotesByDate
	/* Param List */
	(
		@voteID	int
	)
AS

/******************************************************************************
**		File: vaGetVotesByDate.sql
**		Name: vaGetVotesByDate
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

DECLARE @TotalVotes integer
SELECT @TotalVotes = count(*) from UserVote WHERE voteid = @voteID

SELECT   
    dbo.vaTrimTimeFromDateTime(VoteDate) As Votedate, count(*) As TotalVote, (count(*)) * 100 / @TotalVotes as PercentVote
FROM        
    UserVote 
WHERE     
     voteid = @voteID
GROUP BY 
	dbo.vaTrimTimeFromDateTime(VoteDate)
ORDER BY VoteDate 

GO

GRANT EXEC ON vaGetVotesByDate TO PUBLIC

GO
