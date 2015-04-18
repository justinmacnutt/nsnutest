IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'vaGetVotesByFacility')
	BEGIN
		PRINT 'Dropping Procedure vaGetVotesByFacility'
		DROP  Procedure  vaGetVotesByFacility
	END

GO

PRINT 'Creating Procedure vaGetVotesByFacility'
GO
CREATE Procedure vaGetVotesByFacility
	/* Param List */
	(
		@voteID	int
	)
AS

/******************************************************************************
**		File: vaGetVotesByFacility.sql
**		Name: vaGetVotesByFacility
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

DECLARE @temptable TABLE (TotalVote int, FacilityName varchar(100));

	INSERT INTO @temptable
	  select count(*) As TotalVote, f.facilityName
	  FROM nsnu_cm70.dbo.UserVote uv
		INNER JOIN nsnu_membership.dbo.Nurse n on uv.userId = n.userId    
	  INNER JOIN nsnu_membership.dbo.NurseFacility nf on n.userId = nf.nurseId
	  INNER JOIN nsnu_membership.dbo.Facility f on nf.facilityid = f.id

	  WHERE uv.voteid = @voteID
	  AND nf.Priority = 1 -- primary facility 
	  GROUP BY f.facilityName

DECLARE @TotalVotes int
SELECT  @TotalVotes = sum(TotalVote) FROM @temptable
SELECT  TotalVote, TotalVote * 100 / @TotalVotes as PercentVote, FacilityName from @temptable
GROUP BY TotalVote, FacilityName
ORDER BY TotalVote DESC

GO

GRANT EXEC ON vaGetVotesByFacility TO PUBLIC

GO
