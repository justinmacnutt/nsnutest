IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'vaGetVotesByDesignation')
	BEGIN
		PRINT 'Dropping Procedure vaGetVotesByDesignation'
		DROP  Procedure  vaGetVotesByDesignation
	END

GO

PRINT 'Creating Procedure vaGetVotesByDesignation'
GO
CREATE Procedure vaGetVotesByDesignation
	/* Param List */
	(
		@voteID	int
	)
AS

/******************************************************************************
**		File: vaGetVotesByDesignation.sql
**		Name: vaGetVotesByDesignation
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
SELECT @TotalVotes = count(*) from UserVote where voteid = @voteID


SELECT count(*) As TotalVote, (count(*)) * 100 / @TotalVotes as PercentVote, nd.nurseDesignationname
FROM nsnu_cm70.dbo.UserVote uv
INNER JOIN nsnu_membership.dbo.Nurse n on uv.userId = n.userId   
INNER JOIN nsnu_membership.dbo.refNurseDesignation nd on n.nursedesignationId = nd.id
WHERE uv.voteid = @voteID
GROUP BY nd.nurseDesignationname
ORDER BY TotalVote DESC

GO

GRANT EXEC ON vaGetVotesByDesignation TO PUBLIC

GO
